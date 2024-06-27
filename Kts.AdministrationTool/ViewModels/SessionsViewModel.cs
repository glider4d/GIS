using Kts.AdministrationTool.Data;
using Kts.AdministrationTool.Models;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Kts.AdministrationTool.ViewModels
{
    public class SessionsViewModel
    {

        public ObservableCollection<SessionModel> sessionModels
        {
            get;
            set;
        } = new ObservableCollection<SessionModel>();


        private RelayCommand m_sessionClose;

        public RelayCommand sessionClose
        {
            get
            {
                return m_sessionClose ??
                  (m_sessionClose = new RelayCommand(obj =>
                  {
                      if (obj != null)
                      {
                          if (obj is DataGridCellInfo)
                          {
                              DataGridCellInfo cellInfo = (DataGridCellInfo)obj;
                              if (cellInfo != null)
                              {
                                  SessionModel retValue = cellInfo.Item as SessionModel;
                                  if (retValue != null)
                                  {
                                      bool result = m_accessService.closeSession(retValue);
                                      if (result)
                                      {
                                          sessionModels.Remove(retValue);
                                      }
                                      
                                  }
                              }
                                  
                              

                              if (cellInfo.Column.DisplayIndex == 9)// значит наш номер
                              {
                                  //LoginModel retValue = cellInfo.Item as LoginModel;
                                  /*
                                  if (retValue != null)
                                  {
                                    
                                  }*/
                              }
                          }


                  

                      }
                      else
                      {
                          //System.Windows.MessageBox.Show("==null");
                      }

                  }));
            }
        }

        private SqlAdminAccessService m_accessService;
        public SessionsViewModel()
        {
        }
        public SessionsViewModel(SqlAdminAccessService adminService)
        {
            m_accessService = adminService;


            //sessionClose = new RelayCommand(closeOneSessionClick);


        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //System.Windows.MessageBox.Show("Button_Click");
        }

        public void closeOneSessionClick()
        {
            //System.Windows.MessageBox.Show("closeOneSessionClick");
        }


        public void isVisible()
        {
            List<SessionModel> tmpSessionsModel = m_accessService.getOpenSession();
            if (tmpSessionsModel != null)
            {

                sessionModels.Clear();

                foreach (var itemSession in tmpSessionsModel)
                {
                    sessionModels.Add(itemSession);
                }
            }
        }

    }
}
