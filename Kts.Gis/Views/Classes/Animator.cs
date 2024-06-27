using Kts.Gis.ViewModels;
using System;
using System.Timers;
using System.Windows;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Представляет класс, анимирующий объекты.
    /// </summary>
    internal sealed partial class Animator : IDisposable
    {
        #region Закрытые константы

        /// <summary>
        /// Скорость анимации.
        /// </summary>
        private const int animationRate = 250;

        #endregion

        #region Закрытые поля

        /// <summary>
        /// Анимированный объект.
        /// </summary>
        private IHighlightableObjectViewModel animatedObject;

        /// <summary>
        /// Значение, указывающее на то, что анимирован ли объект.
        /// </summary>
        private bool isAnimated;

        /// <summary>
        /// Значение, указывающее на то, что высвобождены ли ресурсы.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Таймер анимации.
        /// </summary>
        private Timer timer;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Animator"/>.
        /// </summary>
        public Animator()
        {
        }

        #endregion

        #region Деструкторы

        /// <summary>
        /// Финализирует экземпляр класса <see cref="Animator"/>.
        /// </summary>
        ~Animator()
        {
            this.Dispose(false);
        }

        #endregion

        #region Обработчики событий

        /// <summary>
        /// Обрабатывает событие <see cref="Timer.Elapsed"/> таймера анимации.
        /// </summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Аргументы события.</param>
        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Используем тут try, так как при закрытии приложения, если таймер еще работает, может выброситься исключение.
            try
            {
                if (Application.Current != null)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.isAnimated = !this.isAnimated;

                        this.SetAnimatedOnOff(this.isAnimated);
                    }));
            }
            catch
            {
            }
        }

        #endregion

        #region Закрытые методы

        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        /// <param name="isDisposing">Значение, указывающее на то, что нужно ли высвободить ресурсы.</param>
        private void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                    if (this.timer != null)
                        this.timer.Dispose();

                this.isDisposed = true;
            }
        }

        /// <summary>
        /// Применяет/убирает анимированность.
        /// </summary>
        /// <param name="isOn">Значение анимированности.</param>
        private void SetAnimatedOnOff(bool isOn)
        {
            if (this.animatedObject == null)
                // Иногда происходит неведомая херня и таймер срабатывает даже после открепления обработчика. Поэтому чтобы избежать исключения выходим из метода.
                return;

            if (isOn)
                this.animatedObject.HighlightOn();
            else
                this.animatedObject.HighlightOff();
        }

        /// <summary>
        /// Начинает анимацию.
        /// </summary>
        private void StartAnimation()
        {
            timer = new Timer(animationRate);

            timer.Elapsed += this.timer_Elapsed;

            timer.Start();
        }

        /// <summary>
        /// Заканчивает анимацию.
        /// </summary>
        private void StopAnimation()
        {
            timer.Stop();

            timer.Elapsed -= this.timer_Elapsed;

            timer.Dispose();

            timer = null;

            this.animatedObject.ResetHighlight();

            this.isAnimated = false;

            this.animatedObject = null;
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Задает значение анимированности.
        /// </summary>
        /// <param name="target">Цель.</param>
        /// <param name="value">Значение анимированности.</param>
        public void SetAnimated(IHighlightableObjectViewModel target, bool value)
        {
            if (value)
            {
                if (this.animatedObject != null)
                    // Если уже имеется анимированный объект, то убираем с него анимацию.
                    this.StopAnimation();

                this.animatedObject = target;

                this.StartAnimation();
            }
            else
                if (this.animatedObject == target)
                    this.StopAnimation();
        }

        #endregion
    }

    // Реализация IDisposable.
    internal sealed partial class Animator : IDisposable
    {
        #region Открытые методы
    
        /// <summary>
        /// Высвобождает ресурсы.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}