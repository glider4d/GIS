using Kts.Gis.Data;
using Kts.Gis.Models;
using Kts.Messaging;
using Kts.Settings;
using Kts.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kts.Gis.ViewModels
{
    public class MetersViewModel : BaseViewModel
    {
        IDataService m_serverData;
        IMessageService m_messageService;
        ISettingService m_settingService;

        public AdvancedObservableCollection<BoilerMeterReportModel> boilerMeterReportModels
        {
            get;
        } = new AdvancedObservableCollection<BoilerMeterReportModel>();

        public BoilerMeterReportModel SelectedItem
        {
            get;
            set;
        }

        public string TitleName
        {
            get;set;
        }


        private bool m_notNull = false;
        public bool notNull
        {
            get
            {
                return m_notNull;
            }
            set
            {
                m_notNull = value;
                RefreshView();
                this.NotifyPropertyChanged(nameof(this.notNull));
            }
        }

        public object DataContext
        {
            get;set;
        }

        public MetersViewModel(IDataService serverData, IMessageService messageService, ISettingService settingService)
        {
            
            m_serverData = serverData;
            m_messageService = messageService;
            m_settingService = settingService;

            TitleName = "Измерительные приборы";

            RefreshView();
        }

        public void RefreshView()
        {
            boilerMeterReportModels.Clear();

            boilerMeterReportModels.AddRange(m_serverData.MeterAccessService.GetBoilerMeterReportModels(m_notNull));
            

        }
    }
}
