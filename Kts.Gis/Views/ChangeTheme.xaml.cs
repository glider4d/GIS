using Kts.Gis.Models.Enums;
using Kts.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Kts.Gis.Views
{
    /// <summary>
    /// Логика взаимодействия для ChangeTheme.xaml
    /// </summary>
    public partial class ChangeTheme : Window
    {
        private ISettingService m_IsettingService;
        public List<string> ThemeList
        {
            get; set;
        }

        public string CurrentTheme
        {
            get;set;
        }

        private int currentIndex;
        public ChangeTheme(ISettingService settingService)
        {
            m_IsettingService = settingService;

            ThemeList = new List<string>();
            ThemeList.Add("Classic");
            ThemeList.Add("Dark blue");
            ThemeList.Add("Orange");
            InitializeComponent();
            var currentTheme = (int)this.m_IsettingService.Settings["theme"];
            if (currentTheme < ThemeList.Count && currentTheme >= 0) CurrentTheme = ThemeList[currentTheme];
            currentIndex = currentTheme;


            this.DataContext = this;
        }

        void changeTheme(int number)
        {
            var tmpp = Application.Current.Resources;


            Uri currentUri = null;
            if (number == (int)Themes.Classic)
            {
                currentUri = new Uri("Views/Resources/DarkTheme/classicTheme.xaml", UriKind.Relative);
            }
            else if (number == (int)Themes.DarkBlue)
            {
                currentUri = new Uri("Views/Resources/DarkTheme/Core.xaml", UriKind.Relative);
            }
            else if (number == (int)Themes.Orange)
            {
                currentUri = new Uri("Views/Resources/DarkTheme/Core.xaml", UriKind.Relative);
            }
            else
                return;
            ResourceDictionary resourceDict = Application.LoadComponent(currentUri) as ResourceDictionary;
            

            tmpp.MergedDictionaries[tmpp.MergedDictionaries.Count - 1] = resourceDict;


        }

        //ok
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            m_IsettingService.Settings["theme"] = currentIndex;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            changeTheme((int)m_IsettingService.Settings["theme"]);
            this.Close();
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = (sender as ComboBox).SelectedIndex;
            currentIndex = index;
            changeTheme(currentIndex);
            
        }
    }
}
