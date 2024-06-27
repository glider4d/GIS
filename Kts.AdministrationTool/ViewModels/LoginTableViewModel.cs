using Kts.AdministrationTool.Data;
using Kts.AdministrationTool.Models;
using Kts.AdministrationTool.ViewModels.Classes;
using Kts.AdministrationTool.Views;
using Kts.WpfUtilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kts.AdministrationTool.ViewModels
{
    class LoginTableViewModel : INotifyPropertyChanged
    {
        public bool m_activeTab
        {
            get;
            set;
        } = false;

        private bool m_noEqualsFlag;
        public bool noEqualsFlag
        {
            get
            {
                return m_noEqualsFlag;
            }
            set
            {
                m_noEqualsFlag = value;
                OnPropertyChanged("noEqualsFlag");
            }
        }

        private int m_rowCount = 0;
        public int rowCount
        {
            get
            {
                return m_rowCount;
            }
            set
            {
                m_rowCount = value;
                OnPropertyChanged("rowCount");
            }
        }

        private int m_columnCount = 0;
        public int columnCount
        {
            get
            {
                return m_columnCount;
            }
            set
            {
                m_columnCount = value;
                OnPropertyChanged("columnCount");
            }
        }

        public RelayCommand EditUser
        {
            get;
        }

        public RelayCommand AddUser
        {
            get;
        }

        public RelayCommand changeList
        {
            get;
        } 



        public ObservableCollection<LoginModel> m_loginModels
        {
            get;
            set;
        } = new ObservableCollection<LoginModel>();
        SqlAdminAccessService m_accessService;
        //private userchangeViewModel m_userEditModel;

        
        private DataGrid m_userGrid;

        public Dictionary<int, string> accessLevelDic
        {
            get;
            set;
        }

        public object DataContext
        {
            get;
            set;
        }

        public List<RegionModel> regionsDic
        {
            get;
            set;
        }

        public LoginTableViewModel(SqlAdminAccessService accessService, DataGrid userGrid, object dataContext)
        {
            // загрузка спарвочников
            DataContext = this;

 





            m_userGrid = userGrid;
            m_accessService = accessService;

            accessLevelDic = m_accessService.getAccessLevels();
            regionsDic = m_accessService.getRegions();

            //m_userEditModel = new userchangeViewModel();
            m_loginModels.CollectionChanged += M_loginModels_CollectionChanged;
            changeList = new RelayCommand(saveButtonClick);
            AddUser = new RelayCommand(showAddUserDialog);
            EditUser = new RelayCommand(showEditUserDialog);
            //AddCommand = new RelayCommand(test);
            

        }
        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      if (obj != null)
                      {
                          if (obj is DataGridCellInfo)
                          {
                              DataGridCellInfo cellInfo = (DataGridCellInfo)obj;
                              if (cellInfo.Column.DisplayIndex == 9)// значит наш номер
                              {
                                  LoginModel retValue = cellInfo.Item as LoginModel;
                                  if (retValue != null)
                                  {
                                      //MessageBox.Show(retValue.region);
                                      
                                      RegionsDialog regionDialog = new RegionsDialog(regionsDic, retValue.region);
                                      regionDialog.ShowDialog();


                                      if (regionDialog.needUpdate)
                                      {
                                          retValue.region = regionDialog.resultString;
                                      }

                                      /*
                                      addNewUser changeUserDialog = new addNewUser(false, m_loginModels[index], m_accessService);

                                      changeUserDialog.changeEvent += editUserPass;
                                      changeUserDialog?.ShowDialog();*/
                                  }
                              }
                          }
                          
                          
                          /*
                          LoginModel retValue = obj as LoginModel;
                          if (retValue != null)
                          {
                              MessageBox.Show(retValue.region);
                          }*/
                          
                      }
                      else
                      {
                          MessageBox.Show("==null");
                      }
                      
                  }));
            }
            
        }
        public void test()
        {
        }

        public void regionCellOnClick(object datagrid)
        {
            //MessageBox.Show("regionCellOnClick");
        }

        public void addNewUser(object send, UserAddEventArgs arg)
        {
            
            int id = m_cacheLoginModels.Max(a => a.Id);

            var selectedItem = from item in m_cacheLoginModels
                               where item.Id == id
                               select item;

            //if (1 == selectedItem.Count<LoginModel>())
            {
                arg.userModel.Id = ++id;
                m_accessService.insertNewLogin(arg.userModel);
                arg.userModel.PropertyChanged += loginModelCachePropertyChanged;
                m_cacheLoginModels.Add(arg.userModel);
                arg.userModel.accessLevelDictionary = accessLevelDic;
                m_loginModels.Add(arg.userModel);
                noEqualsFlag = false;
                
                
            }    
        }

        public void editUserPass(object send, UserChangeEventArgs arg)
        {
            m_accessService.updateUserPassword(arg.userModel);


        }

        public void showEditUserDialog()
        {

            int index = m_userGrid.SelectedIndex;
            if (index >= 0 && index < m_loginModels.Count)
            {


                addNewUser changeUserDialog = new addNewUser(false, m_loginModels[index], m_accessService);

                changeUserDialog.changeEvent += editUserPass;
                changeUserDialog?.ShowDialog();
            }
        }

        public void showAddUserDialog()
        {
            LoginModel newUserLoginModel = new LoginModel(-1, "");
            
            addNewUser addUserDialog = new addNewUser(true, newUserLoginModel, m_accessService);

            addUserDialog.addEvent += addNewUser;
            addUserDialog?.ShowDialog();
        }

        private void M_loginModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (m_activeTab)
                noEqualsFlag = true;
        }
        
        public void saveButtonClick()
        {
            //m_accessService.storedProcedureEdit();
            //m_accessService.storeProcedureCreate();

            //m_accessService.deleteSqlVitim();

            
            m_cacheLoginModels.Clear();
            foreach (var loginModel in m_loginModels)
            {
                if (loginModel.needUpdate())
                {
                    m_accessService.updateLogin(loginModel);
                    loginModel.updateingLogin();
                }
                else if (loginModel.needInsert())
                { 
                }
                m_cacheLoginModels.Add(loginModel);
            }
            noEqualsFlag = false;
        }
        private List<LoginModel> m_cacheLoginModels = new List<LoginModel>();

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

 

        public void isVisible()
        {
            m_activeTab = false;
            if (m_loginModels.Count == 0)
            {
                
                m_cacheLoginModels = m_accessService.getAllUsers();
                foreach (var item in m_cacheLoginModels)
                {
                    item.PropertyChanged += loginModelCachePropertyChanged;
                    item.accessLevelDictionary = accessLevelDic;
                    m_loginModels.Add(item);
                }
                m_activeTab = true;
            }
            else
            {

                if (m_cacheLoginModels.Count != m_loginModels.Count)
                {
                    noEqualsFlag = true;
                }
                else
                {
                    for (int i = 0; i < m_cacheLoginModels.Count; i++)
                    {
                        if (!m_cacheLoginModels[i].Equals(m_loginModels[i]))
                        {
                            noEqualsFlag = true;
                            break;
                        } 

                    }
                }
                m_activeTab = true;
                
            }
            
        }

        private void loginModelCachePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            noEqualsFlag = true;
        }
    }
}
