
using Hardcodet.Wpf.Util;
using Kts.AdministrationTool.Data;
using Kts.AdministrationTool.Models;
using Kts.AdministrationTool.ViewModels.Classes;
using Kts.AdministrationTool.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using Kts.WpfUtilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Kts.AdministrationTool.ViewModels
{
    public class paramertersTypeViewModel : INotifyPropertyChanged //: DependencyObject
    {
        private SqlAdminAccessService m_accessService;



        public Dictionary<int, string> typesName
        {

            get;
            set;
        }
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

        public bool m_activeTab
        {
            get;
            set;
        } = false;

        public ObservableCollection<ParametersTypeModel> listParameterType
        {
            get;
            set;
        } = new ObservableCollection<ParametersTypeModel>();

        public ObservableCollection<ParameterModel> listParameters
        {
            get;
            set;
        } = new ObservableCollection<ParameterModel>();

        public RelayCommand changeType
        {
            get;
        }

        List<ParameterModel> m_tmpParamModelList;
        string currentTypeName;
        int currentTypeId;

        public void selectComboBox(object send, ComboBoxSelectEvent arg)
        {
            //MessageBox.Show("!!!parametersType "+arg.item.ToString());
            noEqualsFlag = false;
            int id = arg.item.Key;
            currentTypeName = arg.item.Value;
            currentTypeId = id;
            listParameterType.Clear();
            listParameters.Clear();


            if (m_tmpParamModelList == null)
                m_tmpParamModelList = m_accessService.getAllParameters();

            //List<ParametersTypeModel> tmpList = m_accessService.getAllTypes(id);

            List<ParametersTypeModel> tmpList = m_accessService.getAllTypes(id, m_tmpParamModelList);


            //List<ParameterModel> tmpParamModelList = m_accessService.getAllParameters();


            foreach (var item in tmpList)
            {
                item.setUpdateFlag(false);
                listParameterType.Add(item);
            }
            //!!!var paramNames = new HashSet<string>(tmpList.Select(s => s.paramName));

            //!!!tmpParamModelList.RemoveAll(c => paramNames.Contains(c.name));
            //tmpParamModelList.RemoveAll(c => c.name.Contains("Адрес"));

            foreach (var item in m_tmpParamModelList)
            {
                //listParameterType.Where(s => s.paramName == item.name);
                //var pm = m_tmpParamModelList.Where(c => paramNames.Contains(c.name));
                //!!!var pm = listParameterType.Where(c => item.name.Contains(c.paramName));
                var pm = listParameterType.Where(c => item.name.Contains(c.paramModel.name));
                if (pm == null || pm.Count() == 0)
                    listParameters.Add(item);
            }








            /*
            if (m_tmpParamModelList.Count > 0)
                currentParameterName = m_tmpParamModelList[0].name;
            */
            /*

            if (tmpParamModelList.Count > 0)
                currentParameterName = tmpParamModelList[0].name;*/

        }

        public void saveButtonClick()
        {
            //currentParameterId
            //m_accessService.GetAll(
            //////////////m_accessService.se
            List<ParametersTypeModel> parametersTypeModelList = m_accessService.getParametersType(currentTypeId);

            // listParameterType - приоритетный список
            //List<ParametersTypeModel>  tt=listParameterType.Except(parametersTypeModelList);

            //var pm = listParameterType.Where(c => item.name.Contains(c.paramModel.name));


            List<ParametersTypeModel> tmpList = new List<ParametersTypeModel>();


            //listParameterType.RemoveAt(2);
            foreach (var item in listParameterType)
            {
                tmpList.Add(item);
            }

            //tmpList[0].paramModel.Id = 25;

            // проверяем нужно ли удалить
            // проверяем есть ли элементы в базе, которых нет в итоговом списке listParameterType
            foreach (var item in parametersTypeModelList)
            {
                if (!tmpList.Exists(x => x.paramModel.Id == item.id_param))
                {
                    // если есть удаляем из базы!
                    MessageBox.Show("need delete " + item.id_param);
                }
            }

            // проверяем нужно ли добавить
            foreach (var item in tmpList)
            {
                if (!parametersTypeModelList.Exists(x => x.id_param == item.paramModel.Id))
                {
                    //нужно добавить
                    //MessageBox.Show("need insert"+item.paramModel.name);

                    m_accessService.insertParameterType(item, currentTypeId);
                    item.setUpdateFlag(false);
                }
            }

            foreach (var item in tmpList)
            {
                if (item.needUpdate())
                {
                    MessageBox.Show("needUpdate " + item.paramName);
                    m_accessService.updateParameterType(item, currentTypeId);
                    item.setUpdateFlag(false);
                }
            }

            // проверяем нужно ли обновить


            /*
            foreach (var item in listParameterType)
            {
                parametersTypeModelList.Find(x => x.id_param == item.id_param);
            }*/
            //parametersTypeModelList.Exists



            // проверяем нужно ли обновить
        }

        private DataGrid m_paramType;
        private DataGrid m_parametersGrid;
        Popup popup1;
        Grid m_layout;
        ComboBox m_comboBox;
        public paramertersTypeViewModel(SqlAdminAccessService accessService, DataGrid paramType, DataGrid parametersGrid, Popup popup1, Grid layout, ComboBox combobox)
        {
            m_comboBox = combobox;
            m_layout = layout;
            this.popup1 = popup1;

            listParameters.Clear();



            m_paramType = paramType;
            m_parametersGrid = parametersGrid;

            m_paramType.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(productsDataGrid_PreviewMouseLeftButtonDown);
            m_paramType.Drop += new DragEventHandler(productsDataGrid_Drop);

            m_parametersGrid.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(parametersDataGrid_PreviewMouseLeftButtonDown);
            m_parametersGrid.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(parametersDataGrid_MouseLeftButtonUp);
            m_paramType.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(parametersDataGrid_MouseLeftButtonUp);
            m_accessService = accessService;
            typesName = m_accessService.getDictTypes();
            //typesName = m_accessService.getDictObsTypes();
            changeType = new RelayCommand(saveButtonClick);


        }


        public delegate Point GetPosition(IInputElement element);
        int rowIndex = -1;
        int rowIndexParameters = -1;


        enum dragT
        {
            none = 0,
            dragParameters,
            dragParametersType
        }

        private dragT m_dragTflag = dragT.none;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void parametersDataGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //m_dragTflag = dragT.none;
            //MessageBox.Show("!mouseleftButtonUP!");
        }

        void parametersDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_dragTflag = dragT.dragParameters;
            //m_dragTflag = dragT.none;
            rowIndexParameters = GetCurrentRowIndex(e.GetPosition, m_parametersGrid);
            if (rowIndexParameters < 0)
                return;
            m_parametersGrid.SelectedIndex = rowIndexParameters;

            ParameterModel selectedEmp = m_parametersGrid.Items[rowIndexParameters] as ParameterModel;
            if (selectedEmp == null)
            {
                return;
            }

            DragDropEffects dragdropeffects = DragDropEffects.Move;
            if (DragDrop.DoDragDrop(m_parametersGrid, selectedEmp, dragdropeffects)
                                != DragDropEffects.None)
            {
                m_parametersGrid.SelectedItem = selectedEmp;
            }
            //m_dragTflag = dragT.dragParameters;
        }

        void productsDataGrid_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            m_dragTflag = dragT.dragParametersType;
            //m_dragTflag = dragT.none;
            rowIndex = GetCurrentRowIndex(e.GetPosition, m_paramType);
            if (rowIndex < 0)
                return;
            m_paramType.SelectedIndex = rowIndex;
            ParametersTypeModel selectedEmp = m_paramType.Items[rowIndex] as ParametersTypeModel;
            if (selectedEmp == null)
                return;
            DragDropEffects dragdropeffects = DragDropEffects.Move;
            if (DragDrop.DoDragDrop(m_paramType, selectedEmp, dragdropeffects)
                                != DragDropEffects.None)
            {
                m_paramType.SelectedItem = selectedEmp;
            }
            //            m_dragTflag = dragT.dragParametersType;
        }

        private int GetCurrentRowIndex(GetPosition pos, DataGrid currentDataGrid)
        {
            int curIndex = -1;
            for (int i = 0; i < currentDataGrid.Items.Count; i++)
            {
                DataGridRow itm = GetRowItem(i, currentDataGrid);
                if (GetMouseTargetRow(itm, pos))
                {
                    curIndex = i;
                    break;
                }
            }
            return curIndex;
        }



        private DataGridRow GetRowItem(int index, DataGrid currentGrid)
        {
            if (currentGrid.ItemContainerGenerator.Status
                    != GeneratorStatus.ContainersGenerated)
                return null;
            return currentGrid.ItemContainerGenerator.ContainerFromIndex(index)
                                                            as DataGridRow;

            /*
            if (m_paramType.ItemContainerGenerator.Status
                    != GeneratorStatus.ContainersGenerated)
                return null;
            return m_paramType.ItemContainerGenerator.ContainerFromIndex(index)
                                                            as DataGridRow;*/
        }


        private bool GetMouseTargetRow(Visual theTarget, GetPosition position)
        {
            if (theTarget != null)
            {
                Rect rect = VisualTreeHelper.GetDescendantBounds(theTarget);
                Point point = position((IInputElement)theTarget);
                return rect.Contains(point);
            }
            return false;
        }


        void productsDataGrid_Drop(object sender, DragEventArgs e)
        {

            if (m_dragTflag == dragT.dragParametersType)
            {
                if (rowIndex < 0)
                    return;
                int index = this.GetCurrentRowIndex(e.GetPosition, m_paramType);
                if (index < 0)
                    return;
                if (index == rowIndex)
                    return;
                if (index == m_paramType.Items.Count - 1)
                {
                    MessageBox.Show("This row-index cannot be drop");
                    return;
                }




                ObservableCollection<ParametersTypeModel> productCollection = listParameterType;

                int rowIndexOrder = productCollection[rowIndex].order;
                int indexOrder = productCollection[index].order;

                productCollection[index].order = rowIndexOrder;
                productCollection[rowIndex].order = indexOrder;

                ParametersTypeModel changedProduct = productCollection[rowIndex];



                productCollection.RemoveAt(rowIndex);
                productCollection.Insert(index, changedProduct);
                noEqualsFlag = true;

                for (int i = 0; i < productCollection.Count; i++)
                {
                    if (productCollection[i].order != (i + 1))
                        productCollection[i].order = i + 1;
                }

            }
            else if (m_dragTflag == dragT.dragParameters)
            {

                if (rowIndexParameters < 0)
                    return;
                int index = this.GetCurrentRowIndex(e.GetPosition, m_paramType);
                if (index < 0)
                    return;
                if (index == m_paramType.Items.Count - 1)
                {
                    MessageBox.Show("This row-index cannot be drop");
                    return;
                }

                ObservableCollection<ParameterModel> parameterModelCollection = listParameters;
                ObservableCollection<ParametersTypeModel> parameterTypeModelCollection = listParameterType;
                ParameterModel tmpParameterModel = parameterModelCollection[rowIndexParameters];
                //ParametersTypeModel newChangeParametersType = new ParametersTypeModel(currentParameterName, tmpParameterModel.name, tmpParameterModel.format, parameterTypeModelCollection[index].order, tmpParameterModel.Id);
                ParametersTypeModel newChangeParametersType = new ParametersTypeModel(currentTypeName, tmpParameterModel, parameterTypeModelCollection[index].order, currentTypeId);

                ObservableCollection<ParametersTypeModel> tmpList = new ObservableCollection<ParametersTypeModel>();

                for (int i = (index); i < parameterTypeModelCollection.Count; i++)
                {
                    tmpList.Add(parameterTypeModelCollection[i]);
                }

                for (int i = (parameterTypeModelCollection.Count - 1); i >= index; i--)
                {
                    parameterTypeModelCollection.RemoveAt(i);
                }
                parameterTypeModelCollection.Add(newChangeParametersType);

                for (int i = 0; i < tmpList.Count; i++)
                {
                    tmpList[i].order++;
                    parameterTypeModelCollection.Add(tmpList[i]);
                }

                listParameters.Remove(tmpParameterModel);
                newChangeParametersType.setUpdateFlag(false);
                noEqualsFlag = true;



            }
        }











        public void isVisible()
        {
            if (typesName == null)
            {
                typesName = m_accessService.getDictTypes();
                //typesName = m_accessService.getDictObsTypes();

                //foreach (var item in typesName)
                {
                    //MessageBox.Show(item.Value);
                }
            }
            else
            {
                //m_comboBox.Text = "труа бутте де водка";
                if (currentTypeName != null && currentTypeName.Length > 0)
                {
                    foreach (var item in typesName)
                    {
                        if (item.Value.Equals(currentTypeName))
                        {
                            m_comboBox.SelectedIndex = item.Key;
                            break;
                        }
                    }
                    //m_comboBox.SelectedIndex = m_comboBox.Items.IndexOf(currentTypeName);

                }
                else
                {
                    m_comboBox.SelectedIndex = 0;
                }
            }

        }







        public void OnMouseMove(object sender, MouseEventArgs e)
        {

        }






        public void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs eArgs)
        {
            //MessageBox.Show("mouseleftButtonUp");
        }

        public void OnBeginEdit(object sender, DataGridBeginningEditEventArgs e)
        {

        }

        public void OnEndEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
        }




    }
}
