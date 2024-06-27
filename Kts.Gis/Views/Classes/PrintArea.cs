using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет инструмент задания области печати.
    /// </summary>
    internal sealed partial class PrintArea : Control, IMapObject
    {
        #region Закрытые поля

        /// <summary>
        /// Кастомный угол поворота области печати.
        /// </summary>
        private double customAngle;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли перемещение области печати.
        /// </summary>
        private bool isMoving;

        /// <summary>
        /// Предыдущее положение мыши.
        /// </summary>
        private Point mousePrevPosition;

        /// <summary>
        /// Контейнер страниц.
        /// </summary>
        private Grid pageContainer;

        /// <summary>
        /// Положение области печати.
        /// </summary>
        private Point position;

        /// <summary>
        /// Принтер.
        /// </summary>
        private PrintQueue printer;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Курсоры линий-изменялок размера.
        /// </summary>
        private readonly List<Cursor> lineCursors = new List<Cursor>();

        /// <summary>
        /// Линии-изменялки размера.
        /// </summary>
        private readonly List<PrintAreaLineThumb> lineThumbs = new List<PrintAreaLineThumb>();

        /// <summary>
        /// Карта.
        /// </summary>
        private readonly Map map;

        /// <summary>
        /// Изменялки размера.
        /// </summary>
        private readonly List<PrintAreaResizeThumb> resizeThumbs = new List<PrintAreaResizeThumb>();

        #endregion

        #region Закрытые статические поля

        /// <summary>
        /// Кисть границ страниц.
        /// </summary>
        private static SolidColorBrush borderBrush = new SolidColorBrush(Colors.Gray);

        /// <summary>
        /// Кисть заигноренной страницы.
        /// </summary>
        private static SolidColorBrush ignoredBrush = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));

        #endregion

        #region Открытые статические поля

        /// <summary>
        /// Толщина границ.
        /// </summary>
        public static DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(Thickness), typeof(PrintArea), new PropertyMetadata(new Thickness()));

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="PrintArea"/>.
        /// </summary>
        /// <param name="map">Карта.</param>
        public PrintArea(Map map)
        {
            this.map = map;

            this.MouseLeave += this.PrintArea_MouseLeave;
            this.PreviewMouseLeftButtonDown += this.PrintArea_PreviewMouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += this.PrintArea_PreviewMouseLeftButtonUp;
            this.PreviewMouseMove += this.PrintArea_PreviewMouseMove;

            this.SizeChanged += this.PrintArea_SizeChanged;

            this.Redraw();
        }

        #endregion

        #region Статичные конструкторы

        /// <summary>
        /// Инициализирует статичные члены класса <see cref="PrintArea"/>.
        /// </summary>
        static PrintArea()
        {
            if (borderBrush.CanFreeze)
                borderBrush.Freeze();

            if (ignoredBrush.CanFreeze)
                ignoredBrush.Freeze();
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает размер страницы области печати.
        /// </summary>
        public Size AreaPageSize
        {
            get
            {
                var border = this.pageContainer.Children[0] as Border;

                return new Size(border.ActualWidth, border.ActualHeight);
            }
        }

        /// <summary>
        /// Возвращает или задает кастомный угол поворота области печати.
        /// </summary>
        public double CustomAngle
        {
            get
            {
                return this.customAngle;
            }
            private set
            {
                if (value != 0 && this.CustomAngle == 0)
                    foreach (var thumb in this.lineThumbs)
                        thumb.Cursor = Cursors.Arrow;

                if (value == 0 && this.CustomAngle != 0)
                    for (int i = 0; i < this.lineThumbs.Count; i++)
                        this.lineThumbs[i].Cursor = this.lineCursors[i];

                this.customAngle = value;
            }
        }

        /// <summary>
        /// Возвращает список заигноренных страниц. Каждая запись представляет тюпл из двух значений, где первое - это индекс строки, на которой располагается страница, а второе - столбец.
        /// </summary>
        public List<Tuple<int, int>> IgnoredPages
        {
            get;
        } = new List<Tuple<int, int>>();

        /// <summary>
        /// Возвращает количество печатаемых страниц (исключая заигноренные страницы).
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.pageContainer.ColumnDefinitions.Count * this.pageContainer.RowDefinitions.Count - this.IgnoredPages.Count;
            }
        }

        /// <summary>
        /// Возвращает или задает отступы страницы.
        /// </summary>
        public Thickness PageMargin
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает размер страницы.
        /// </summary>
        public Size PageSize
        {
            get;
            private set;
        }

        /// <summary>
        /// Возвращает или задает положение области печати.
        /// </summary>
        public Point Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;

                System.Windows.Controls.Canvas.SetLeft(this, value.X);
                System.Windows.Controls.Canvas.SetTop(this, value.Y);
            }
        }

        /// <summary>
        /// Возвращает или задает принтер.
        /// </summary>
        public PrintQueue Printer
        {
            get
            {
                return this.printer;
            }
            set
            {
                this.printer = value;

                this.UpdateSizeAndMargin();
            }
        }

        /// <summary>
        /// Возвращает или задает толщину границ.
        /// </summary>
        public Thickness Thickness
        {
            get
            {
                return (Thickness)this.GetValue(ThicknessProperty);
            }
            set
            {
                this.SetValue(ThicknessProperty, value);
            }
        }

        /// <summary>
        /// Возвращает общее количество страниц.
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                return this.pageContainer.ColumnDefinitions.Count * this.pageContainer.RowDefinitions.Count;
            }
        }

        #endregion
        
        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Map.ScaleChanged"/> карты.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void Map_ScaleChanged(object sender, EventArgs e)
        {
            this.Redraw();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.MouseLeave"/> области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintArea_MouseLeave(object sender, MouseEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonDown"/> области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintArea_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Необходимо проверить, не находится ли курсор мыши над объектом, представляющим изменялку размеров области печати.
            if ((this.Canvas.Map.CanSelect || this.Canvas.Map.CanEdit) && !(Mouse.DirectlyOver is Ellipse) && !(Mouse.DirectlyOver is Rectangle))
            {
                this.CaptureMouse();

                this.mousePrevPosition = e.GetPosition(this.Canvas);

                this.isMoving = true;

                // Если было совершено двойное нажатие, то нужно определить на каком листе оно было совершено и включить/исключить его из печати.
                if (e.ClickCount == 2)
                {
                    // Получаем координаты мыши относительно контейнера.
                    var relPos = e.GetPosition(this.pageContainer);

                    var colCount = this.pageContainer.ColumnDefinitions.Count;
                    var rowCount = this.pageContainer.RowDefinitions.Count;

                    // Вычисляем высоту и ширину страниц в контейнере.
                    var height = this.pageContainer.ActualHeight / rowCount;
                    var width = this.pageContainer.ActualWidth / colCount;

                    // Определяем строку и столбец.
                    int row = Convert.ToInt32(Math.Ceiling(relPos.Y / height) - 1);
                    int column = Convert.ToInt32(Math.Ceiling(relPos.X / width) - 1);

                    // Получаем элемент контейнера по найденным строке и столбцу и заменяем у него фон.
                    var child = this.pageContainer.Children.OfType<Border>().Where(x => Grid.GetColumn(x) == column && Grid.GetRow(x) == row).First();
                    var ignored = this.IgnoredPages.FirstOrDefault(x => x.Item1 == row && x.Item2 == column);
                    if (ignored != null)
                    {
                        this.IgnoredPages.Remove(ignored);

                        child.Background = null;
                    }
                    else
                    {
                        this.IgnoredPages.Add(new Tuple<int, int>(row, column));

                        child.Background = ignoredBrush;
                    }
                }

                // Блокируем "всплытие" события.
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseLeftButtonUp"/> области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintArea_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.OnLeaveOrLeftButtonUp();
        }

        /// <summary>
        /// Обрабатывает событие <see cref="UIElement.PreviewMouseMove"/> области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintArea_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!this.isMoving)
                return;

            var p = e.GetPosition(this.Canvas);

            var deltaX = p.X - this.mousePrevPosition.X;
            var deltaY = p.Y - this.mousePrevPosition.Y;

            // Сдвигаем область печати.
            var point = this.Position;
            this.Position = new Point(point.X + deltaX, point.Y + deltaY);

            this.mousePrevPosition = p;
        }

        /// <summary>
        /// Обрабатывает событие <see cref="FrameworkElement.SizeChanged"/> области печати.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void PrintArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var first = this.pageContainer.Children[0] as Border;

            if (first.ActualWidth < 200 || first.ActualHeight < 200)
                // Если первая страница становится слишком маленькой, то уменьшаем количество страниц.
                this.DecreasePageCount();
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Выполняет действия, связанные с уходом мыши с области печати или отжатии левой кнопки мыши.
        /// </summary>
        private void OnLeaveOrLeftButtonUp()
        {
            if (!this.isMoving)
                return;

            this.ReleaseMouseCapture();

            this.isMoving = false;
        }

        /// <summary>
        /// Перерисовывает элементы управления области печати.
        /// </summary>
        private void Redraw()
        {
            var thickness = 5.0 / this.map.Scale;

            this.Thickness = new Thickness(0, 0, thickness, thickness);

            foreach (var thumb in this.resizeThumbs)
                thumb.Thickness = thickness;

            foreach (var thumb in this.lineThumbs)
                thumb.Thickness = thickness;
        }

        /// <summary>
        /// Обновляет размер и отступы области печати.
        /// </summary>
        private void UpdateSizeAndMargin()
        {
            var area = this.Printer.GetPrintCapabilities(this.Printer.UserPrintTicket).PageImageableArea;

            this.Width = area.ExtentWidth;
            this.Height = area.ExtentHeight;

            this.PageSize = new Size(this.Width, this.Height);
            this.PageMargin = new Thickness(area.OriginWidth, area.OriginHeight, area.OriginWidth, area.OriginHeight);
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Центрирует область печати относительно ее положения.
        /// </summary>
        public void Center()
        {
            var position = this.Position;

            this.Position = new Point(position.X - this.Width / 2, position.Y - this.Height / 2);
        }

        /// <summary>
        /// Уменьшает количество страниц.
        /// </summary>
        public void DecreasePageCount()
        {
            var lastColumnIndex = this.pageContainer.ColumnDefinitions.Count - 1;
            var lastRowIndex = this.pageContainer.RowDefinitions.Count - 1;

            if (lastColumnIndex == 0 || lastRowIndex == 0)
                return;

            var lastChildren = this.pageContainer.Children.OfType<Border>().Where(x => Grid.GetColumn(x) == lastColumnIndex || Grid.GetRow(x) == lastRowIndex).ToList();

            foreach (var child in lastChildren)
                this.pageContainer.Children.Remove(child);

            this.pageContainer.ColumnDefinitions.RemoveAt(lastColumnIndex);
            this.pageContainer.RowDefinitions.RemoveAt(lastRowIndex);

            // Убираем из списка заигноренных страниц страницы, находящиеся на удаленных строке и столбце.
            this.IgnoredPages.RemoveAll(x => x.Item1 == lastRowIndex || x.Item2 == lastColumnIndex);
        }

        /// <summary>
        /// Подгонят размер области печати к заданному максимальному размеру.
        /// </summary>
        /// <param name="maxSize">Максимальный размер.</param>
        public void Fit(Size maxSize)
        {
            double deltaWidth;
            double deltaHeight;

            double coeff;

            // Будем добавлять небольшой отступ со всех сторон.
            double offset = 40;

            // Сперва проверяем ширину.
            if (this.Width > maxSize.Width)
            {
                deltaWidth = this.Width - maxSize.Width + offset;

                coeff = this.Width / this.Height;

                this.Width -= deltaWidth;
                this.Height = this.Width / coeff;
            }

            // Теперь проверяем высоту.
            if (this.Height > maxSize.Height)
            {
                deltaHeight = this.Height - maxSize.Height + offset;

                coeff = this.Height / this.Width;

                this.Height -= deltaHeight;
                this.Width = this.Height / coeff;
            }
        }

        /// <summary>
        /// Увеличивает количество страниц.
        /// </summary>
        public void IncreasePageCount()
        {
            // Ограничиваем возможность дробления исходя от размера первой страницы.
            var first = this.pageContainer.Children[0] as Border;
            if (first.ActualWidth < 200 || first.ActualHeight < 200)
                return;

            this.pageContainer.ColumnDefinitions.Add(new ColumnDefinition());
            this.pageContainer.RowDefinitions.Add(new RowDefinition());

            var lastColumnIndex = this.pageContainer.ColumnDefinitions.Count - 1;
            var lastRowIndex = this.pageContainer.RowDefinitions.Count - 1;

            for (int i = 0; i < lastColumnIndex; i++)
            {
                var border = new Border()
                {
                    BorderBrush = borderBrush
                };

                // Привязываем толщину границы.
                var binding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Thickness")
                };
                border.SetBinding(Border.BorderThicknessProperty, binding);

                Grid.SetColumn(border, i);
                Grid.SetRow(border, lastRowIndex);

                this.pageContainer.Children.Add(border);
            }

            for (int i = 0; i < lastRowIndex + 1; i++)
            {
                var border = new Border()
                {
                    BorderBrush = borderBrush
                };

                // Привязываем толщину границы.
                var binding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("Thickness")
                };
                border.SetBinding(Border.BorderThicknessProperty, binding);

                Grid.SetColumn(border, lastColumnIndex);
                Grid.SetRow(border, i);

                this.pageContainer.Children.Add(border);
            }
        }

        /// <summary>
        /// Возвращает значение, указывающее на то, что проигнорирована ли заданная страница.
        /// </summary>
        /// <param name="row">Номер строки.</param>
        /// <param name="column">Номер столбца.</param>
        /// <returns>true, если страница проигнорирована, иначе - false.</returns>
        public bool IsPageIgnored(int row, int column)
        {
            return this.IgnoredPages.Any(x => x.Item1 == row && x.Item2 == column);
        }

        /// <summary>
        /// Выполняет поворот области печати на заданный угол.
        /// </summary>
        /// <param name="angle">Угол поворота.</param>
        public void Rotate(double angle)
        {
            var transform = this.RenderTransform as RotateTransform;

            transform.Angle = angle;

            this.CustomAngle = angle;
        }

        /// <summary>
        /// Выполняет поворот области печати налево.
        /// </summary>
        public void RotateLeft()
        {
            switch (this.Printer.UserPrintTicket.PageOrientation)
            {
                case PageOrientation.Landscape:
                    this.Printer.UserPrintTicket.PageOrientation = PageOrientation.Portrait;

                    break;

                case PageOrientation.Portrait:
                    this.Printer.UserPrintTicket.PageOrientation = PageOrientation.Landscape;

                    break;
            }

            this.UpdateSizeAndMargin();
        }

        /// <summary>
        /// Выполняет поворот области печати направо.
        /// </summary>
        public void RotateRight()
        {
            switch (this.Printer.UserPrintTicket.PageOrientation)
            {
                case PageOrientation.Landscape:
                    this.Printer.UserPrintTicket.PageOrientation = PageOrientation.Portrait;

                    break;

                case PageOrientation.Portrait:
                    this.Printer.UserPrintTicket.PageOrientation = PageOrientation.Landscape;

                    break;
            }

            this.UpdateSizeAndMargin();
        }

        /// <summary>
        /// Задает область области печати.
        /// </summary>
        /// <param name="leftTop">Левая верхняя координата области.</param>
        /// <param name="size">Размер области.</param>
        public void SetArea(Point leftTop, Size size)
        {
            // Желаемый размер должен быть больше минимального.
            if (size.Width < 100 || size.Height < 100)
                return;

            if (size.Width < size.Height)
            {
                if (this.Printer.UserPrintTicket.PageOrientation == PageOrientation.Landscape)
                    this.RotateRight();
            }
            else
                if (this.Printer.UserPrintTicket.PageOrientation == PageOrientation.Portrait)
                    this.RotateRight();
            
            // Сперва определяем новую ширину области.
            var coeff = this.Height / this.Width;
            this.Width = size.Width;
            this.Height = this.Width * coeff;

            // Теперь подгоняем под высоту.
            if (this.Height < size.Height)
            {
                coeff = this.Width / this.Height;

                this.Height = size.Height;
                this.Width = this.Height * coeff;
            }

            // Находим разницу между желаемым и фактическим размером области.
            var deltaX = this.Width - size.Width;
            var deltaY = this.Height - size.Height;

            this.Position = new Point(leftTop.X - deltaX / 2, leftTop.Y - deltaY / 2);
        }

        #endregion
    }
    
    // Реализация Control.
    internal sealed partial class PrintArea
    {
        #region Открытые переопределенные методы

        /// <summary>
        /// Выполняется при применении шаблона.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.pageContainer = this.GetTemplateChild("pageContainer") as Grid;

            this.resizeThumbs.Add(this.GetTemplateChild("resizeThumbLT") as PrintAreaResizeThumb);
            this.resizeThumbs.Add(this.GetTemplateChild("resizeThumbRT") as PrintAreaResizeThumb);
            this.resizeThumbs.Add(this.GetTemplateChild("resizeThumbRB") as PrintAreaResizeThumb);
            this.resizeThumbs.Add(this.GetTemplateChild("resizeThumbLB") as PrintAreaResizeThumb);

            this.lineThumbs.Add(this.GetTemplateChild("lineThumbL") as PrintAreaLineThumb);
            this.lineThumbs.Add(this.GetTemplateChild("lineThumbT") as PrintAreaLineThumb);
            this.lineThumbs.Add(this.GetTemplateChild("lineThumbR") as PrintAreaLineThumb);
            this.lineThumbs.Add(this.GetTemplateChild("lineThumbB") as PrintAreaLineThumb);

            foreach (var thumb in this.lineThumbs)
                this.lineCursors.Add(thumb.Cursor);

            this.RenderTransformOrigin = new Point(0.5, 0.5);
            this.RenderTransform = new RotateTransform();
        }

        #endregion
    }

    // Реализация IMapObject.
    internal sealed partial class PrintArea
    {
        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает холст, на котором расположен объект.
        /// </summary>
        public IndentableCanvas Canvas
        {
            get;
            private set;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Добавляет объект на холст.
        /// </summary>
        /// <param name="canvas">Холст.</param>
        /// <returns>true, если объект был добавлен, иначе - false.</returns>
        public bool AddToCanvas(IndentableCanvas canvas)
        {
            if (this.Canvas != null)
                return false;

            this.Canvas = canvas;

            this.Canvas.Children.Add(this);

            this.map.ScaleChanged += this.Map_ScaleChanged;

            return true;
        }

        /// <summary>
        /// Удаляет объект с холста.
        /// </summary>
        /// <returns>true, если объект был удален, иначе - false.</returns>
        public bool RemoveFromCanvas()
        {
            if (this.Canvas == null)
                return false;

            this.Canvas.Children.Remove(this);

            this.Canvas = null;

            this.map.ScaleChanged -= this.Map_ScaleChanged;

            return true;
        }

        #endregion
    }
}