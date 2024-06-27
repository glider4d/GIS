using Kts.Settings;
using Kts.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kts.Gis.ViewModels
{
    /// <summary>
    /// Представляет модель представления сетки параметров.
    /// </summary>
    internal sealed class ParameterGridViewModel : BaseViewModel
    {
        #region Закрытые поля

        /// <summary>
        /// Значение, указывающее на то, то имеются ли значения вычисляемых параметров у выбранного объекта.
        /// </summary>
        private bool hasSelectedCalcParameterValues;

        /// <summary>
        /// Значение, указывающее на то, то имеются ли значения общих параметров у выбранного слоя.
        /// </summary>
        private bool hasSelectedCommonParameterValues;

        /// <summary>
        /// Значение, указывающее на то, то имеются ли значения параметров у выбранного объекта.
        /// </summary>
        private bool hasSelectedParameterValues;

        /// <summary>
        /// Ширина столбца, представляющего заголовок параметра.
        /// </summary>
        private double headerColumnWidth;

        /// <summary>
        /// Значение, указывающее на то, что стоит ли скрывать параметры с пустыми значениями.
        /// </summary>
        private bool hideEmptyParameters;

        /// <summary>
        /// Значение, указывающее на то, что выполняется ли загрузка значений параметров выбранного объекта.
        /// </summary>
        private bool isParametersLoading;

        /// <summary>
        /// Сигнализитор токена отмены загрузки параметров выбранного объекта.
        /// </summary>
        private CancellationTokenSource parameterCts;

        /// <summary>
        /// Значения вычисляемых параметров выбранного объекта.
        /// </summary>
        private ParameterValueSetViewModel selectedCalcParamaterValues;

        /// <summary>
        /// Значения общих параметров выбранного объекта.
        /// </summary>
        private ParameterValueSetViewModel selectedCommonParamaterValues;

        /// <summary>
        /// Значения параметров выбранного объекта.
        /// </summary>
        private ParameterValueSetViewModel selectedParamaterValues;

        /// <summary>
        /// Заголовок значений параметров выбранного объекта.
        /// </summary>
        private string selectedParameterValuesTitle;

        /// <summary>
        /// Ширина столбца, представляющего значение параметра.
        /// </summary>
        private double valueColumnWidth;

        #endregion

        #region Закрытые неизменяемые поля

        /// <summary>
        /// Сервис настроек.
        /// </summary>
        private readonly ISettingService settingService;

        #endregion

        #region Конструкторы

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParameterGridViewModel"/>.
        /// </summary>
        /// <param name="settingService">Сервис настроек.</param>
        public ParameterGridViewModel(ISettingService settingService)
        {
            this.settingService = settingService;

            this.hideEmptyParameters = Convert.ToBoolean(this.settingService.Settings["HideEmptyParameters"]);
            this.headerColumnWidth = Convert.ToDouble(this.settingService.Settings["HeaderColumnWidth"]);
            this.valueColumnWidth = Convert.ToDouble(this.settingService.Settings["ValueColumnWidth"]);
        }

        #endregion

        #region Открытые свойства

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, то имеются ли значения вычисляемых параметров у выбранного объекта.
        /// </summary>
        public bool HasSelectedCalcParameterValues
        {
            get
            {
                return this.hasSelectedCalcParameterValues;
            }
            private set
            {
                this.hasSelectedCalcParameterValues = value;

                this.NotifyPropertyChanged(nameof(this.HasSelectedCalcParameterValues));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, то имеются ли значения общих параметров у выбранного объекта.
        /// </summary>
        public bool HasSelectedCommonParameterValues
        {
            get
            {
                return this.hasSelectedCommonParameterValues;
            }
            private set
            {
                this.hasSelectedCommonParameterValues = value;

                this.NotifyPropertyChanged(nameof(this.HasSelectedCommonParameterValues));
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, то имеются ли значения параметров у выбранного объекта.
        /// </summary>
        public bool HasSelectedParameterValues
        {
            get
            {
                return this.hasSelectedParameterValues;
            }
            private set
            {
                this.hasSelectedParameterValues = value;

                this.NotifyPropertyChanged(nameof(this.HasSelectedParameterValues));
            }
        }

        /// <summary>
        /// Возвращает или задает ширину столбца, представляющего заголовок параметра.
        /// </summary>
        public double HeaderColumnWidth
        {
            get
            {
                return this.headerColumnWidth;
            }
            set
            {
                this.headerColumnWidth = value;

                this.NotifyPropertyChanged(nameof(this.HeaderColumnWidth));

                this.settingService.Settings["HeaderColumnWidth"] = value;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что стоит ли скрывать параметры с пустыми значениями.
        /// </summary>
        public bool HideEmptyParameters
        {
            get
            {
                return this.hideEmptyParameters;
            }
            set
            {
                this.hideEmptyParameters = value;

                this.NotifyPropertyChanged(nameof(this.HideEmptyParameters));

                this.settingService.Settings["HideEmptyParameters"] = value;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее на то, что выполняется ли загрузка значений параметров выбранного объекта.
        /// </summary>
        public bool IsParametersLoading
        {
            get
            {
                return this.isParametersLoading;
            }
            private set
            {
                this.isParametersLoading = value;

                this.NotifyPropertyChanged(nameof(this.IsParametersLoading));
            }
        }

        /// <summary>
        /// Возвращает или задает значения вычисляемых параметров выбранного объекта.
        /// </summary>
        public ParameterValueSetViewModel SelectedCalcParameterValues
        {
            get
            {
                return this.selectedCalcParamaterValues;
            }
            private set
            {
                this.selectedCalcParamaterValues = value;

                this.NotifyPropertyChanged(nameof(this.SelectedCalcParameterValues));

                this.HasSelectedCalcParameterValues = value != null;
            }
        }

        /// <summary>
        /// Возвращает или задает значения вычисляемых параметров выбранного объекта.
        /// </summary>
        public ParameterValueSetViewModel SelectedCommonParameterValues
        {
            get
            {
                return this.selectedCommonParamaterValues;
            }
            private set
            {
                this.selectedCommonParamaterValues = value;

                this.NotifyPropertyChanged(nameof(this.SelectedCommonParameterValues));

                this.HasSelectedCommonParameterValues = value != null;
            }
        }

        /// <summary>
        /// Возвращает или задает значения параметров выбранного объекта.
        /// </summary>
        public ParameterValueSetViewModel SelectedParameterValues
        {
            get
            {
                return this.selectedParamaterValues;
            }
            private set
            {
                this.selectedParamaterValues = value;

                this.NotifyPropertyChanged(nameof(this.SelectedParameterValues));

                this.HasSelectedParameterValues = value != null;
            }
        }

        /// <summary>
        /// Возвращает или задает заголовок значений параметров выбранного объекта.
        /// </summary>
        public string SelectedParameterValuesTitle
        {
            get
            {
                return this.selectedParameterValuesTitle;
            }
            private set
            {
                this.selectedParameterValuesTitle = value;

                this.NotifyPropertyChanged(nameof(this.SelectedParameterValuesTitle));
            }
        }

        /// <summary>
        /// Возвращает или задает ширину столбца, представляющего значение параметра.
        /// </summary>
        public double ValueColumnWidth
        {
            get
            {
                return this.valueColumnWidth;
            }
            set
            {
                this.valueColumnWidth = value;

                this.NotifyPropertyChanged(nameof(this.ValueColumnWidth));

                this.settingService.Settings["ValueColumnWidth"] = value;
            }
        }

        #endregion

        #region Открытые методы

        /// <summary>
        /// Отменяет текущую загрузку параметров, если она имеется.
        /// </summary>
        public void CancelLoading()
        {
            if (this.parameterCts != null)
                this.parameterCts.Cancel();
        }

        /// <summary>
        /// Загружает данные заданного слоя объектов.
        /// </summary>
        /// <param name="layer">Слой объектов.</param>
        public void LoadLayerData(LayerViewModel layer)
        {
            layer.LoadParameterValues();

            layer.LoadCommonParameterValues();

            this.SelectedParameterValuesTitle = layer.Type.Name + " (" + layer.ObjectCount + ")";

            this.SelectedParameterValues = layer.ParameterValuesViewModel;
            this.SelectedCommonParameterValues = layer.CommonParameterValuesViewModel;
            this.SelectedCalcParameterValues = null;

            this.IsParametersLoading = false;
        }

        
        

        /// <summary>
        /// Асинхронно загружает данные заданного слоя объектов.
        /// </summary>
        /// <param name="layer">Слой объектов.</param>
        /// <returns>Задача.</returns>
        public async Task LoadLayerDataAsync(LayerViewModel layer)
        {
            // Создаем новый сигнализатор.
            var cts = new CancellationTokenSource();
            this.parameterCts = cts;

            this.IsParametersLoading = true;

            try
            {
                if (BaseSqlDataAccessService.testConnectionFlag)
                    await layer.LoadParameterValuesAsync(cts.Token);


                if (!cts.IsCancellationRequested)
                {
                    if (BaseSqlDataAccessService.testConnectionFlag)
                        await layer.LoadCommonParameterValuesAsync(cts.Token);

                    if (!cts.IsCancellationRequested)
                    {
                        this.SelectedParameterValuesTitle = layer.Type.Name + " (" + layer.ObjectCount + ")";

                        this.SelectedParameterValues = layer.ParameterValuesViewModel;
                        this.SelectedCommonParameterValues = layer.CommonParameterValuesViewModel;
                        this.SelectedCalcParameterValues = null;

                        this.IsParametersLoading = false;
                    }
                }
            }
            catch
            {
                if (!cts.IsCancellationRequested)
                    this.IsParametersLoading = false;
            }
        }

        /// <summary>
        /// Загружает основные данные заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        public void LoadObjectMainData(IParameterizedObjectViewModel obj)
        {
            obj.LoadParameterValues();

            this.SelectedParameterValuesTitle = (obj as ITypedObjectViewModel).Type.SingularName;

            this.SelectedParameterValues = obj.ParameterValuesViewModel;
        }

        /// <summary>
        /// Заполняет данные параметров, значениями по умолчанию
        /// </summary>
        /// <param name="obj">Объект.</param>
        public void LoadObjectDefaultData(IParameterizedObjectViewModel obj)
        {
            obj.LoadParameterDefaultValues();
        }
        /// <summary>
        /// Загружает данные заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Задача.</returns>
        public void LoadObjectData(IParameterizedObjectViewModel obj)
        {
            obj.LoadParameterValues();

            obj.LoadCalcParameterValues();

            this.SelectedParameterValuesTitle = (obj as ITypedObjectViewModel).Type.SingularName;

            this.SelectedParameterValues = obj.ParameterValuesViewModel;
            this.SelectedCommonParameterValues = null;
            this.SelectedCalcParameterValues = obj.CalcParameterValuesViewModel;
        }

        /// <summary>
        /// Асинхронно загружает данные заданного объекта.
        /// </summary>
        /// <param name="obj">Объект.</param>
        /// <returns>Задача.</returns>
        public async Task LoadObjectDataAsync(IParameterizedObjectViewModel obj)
        {
            // Создаем новый сигнализатор.
            var cts = new CancellationTokenSource();
            this.parameterCts = cts;

            this.IsParametersLoading = true;

            try
            {

                if (BaseSqlDataAccessService.testConnectionFlag)
                    await obj.LoadParameterValuesAsync(cts.Token);
                    //obj.LoadParameterDefaultValues();
                else
                {

                    ObjectViewModel objViewModel = obj as ObjectViewModel;
                    if (obj.ParameterValuesViewModel == null && objViewModel != null)
                    {
                        if (objViewModel.Id == Guid.Empty)
                            obj.LoadParameterDefaultValues();
                    }
                    //obj.ParameterValuesViewModel = obj.
                }

                if (!cts.IsCancellationRequested)
                {
                    if (BaseSqlDataAccessService.testConnectionFlag)
                        await obj.LoadCalcParameterValuesAsync(cts.Token);
                    else
                    {
                        ObjectViewModel objViewModel = obj as ObjectViewModel;
                        if (obj.CalcParameterValuesViewModel == null && objViewModel != null)
                            if (objViewModel.Id == Guid.Empty)
                                obj.LoadCalcParameterDefaultValues();
                    }
                        

                    if (!cts.IsCancellationRequested)
                    {
                        this.SelectedParameterValuesTitle = (obj as ITypedObjectViewModel).Type.SingularName;

                        this.SelectedParameterValues = obj.ParameterValuesViewModel;
                        this.SelectedCommonParameterValues = null;
                        this.SelectedCalcParameterValues = obj.CalcParameterValuesViewModel;

                        this.IsParametersLoading = false;
                    }
                }
            }
            catch (Exception e)
            {
                if (!cts.IsCancellationRequested)
                    this.IsParametersLoading = false;
            }
        }

        /// <summary>
        /// Выгружает данные.
        /// </summary>
        public void UnloadData()
        {
            this.SelectedParameterValuesTitle = "";

            this.SelectedParameterValues = null;
            this.SelectedCommonParameterValues = null;
            this.SelectedCalcParameterValues = null;

            this.IsParametersLoading = false;
        }

        #endregion
    }
}