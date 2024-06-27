using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KtsInstall
{
    public partial class Form1 : Form
    {

        private string installFolder = "\\KtsFolder";
        private string shortCutName = @"\Инженерно-картографическая система.lnk";
        private string applicationName = "Инженерно-картографическая система";
        private string ktsExe = @"Kts.Gis.exe";
        public Form1()
        {
            InitializeComponent();
            //PathTextBoth.Text = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + installFolder;
            //PathTextBoth.Text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile) + installFolder;
            PathTextBoth.Text = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + installFolder;
        }

        private string GetTargetPath()
        {
            string result = "";
            string linkPathName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + shortCutName;

            if (System.IO.File.Exists(linkPathName))
            {
                // WshShellClass shell = new WshShellClass();
                WshShell shell = new WshShell(); //Create a new WshShell Interface
                IWshShortcut link = (IWshShortcut)shell.CreateShortcut(linkPathName); //Link the interface to our shortcut
                result = link.TargetPath;
                //MessageBox.Show(link.TargetPath); //Show the target in a MessageBox using IWshShortcut
            }
            return result;
        }

        private void CreateShortcut()
        {
            WshShell shell = new WshShell();

            //путь к ярлыку
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + shortCutName;
            
            //создаем объект ярлыка
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);

            //задаем свойства для ярлыка
            //описание ярлыка в всплывающей подсказке
            shortcut.Description = applicationName;
            //горячая клавиша
            shortcut.Hotkey = "Ctrl+Shift+N";
            //путь к самой программе
            shortcut.TargetPath = PathTextBoth.Text + @"\" + ktsExe;

            //Создаем ярлык
            shortcut.Save();
        }
        /*
        private static void configStep_addShortcutToStartupGroup()
        {
            using (ShellLink shortcut = new ShellLink())
            {
                shortcut.Target = Application.ExecutablePath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                shortcut.Description = "My Shorcut Name Here";
                shortcut.DisplayMode = ShellLink.LinkDisplayMode.edmNormal;
                shortcut.Save(STARTUP_SHORTCUT_FILEPATH);
            }
        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isInstall)
            {
                if (PathTextBoth.Text.Length > 0)
                {
                    string message = "Установить приложение " + applicationName;
                    string title = "";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.CreateDirectory(PathTextBoth.Text);
                            string myPath = @".\SourcePath\";
                            string[] filePaths = Directory.GetFiles(myPath);


                            foreach (string file in filePaths)
                            {
                                FileInfo info = new FileInfo(file);
                                if (System.IO.File.Exists(info.FullName) && !System.IO.File.Exists(PathTextBoth.Text + "\\" + info.Name))
                                {
                                    System.IO.File.Copy(info.FullName, PathTextBoth.Text + "\\" + info.Name);
                                }
                            }
                            if (System.IO.File.Exists(PathTextBoth.Text + @"\" + ktsExe))
                                CreateShortcut();
                        } catch(System.UnauthorizedAccessException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            } else
            {
                string message = "Удалить приложение " + applicationName;
                string title = "";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {

                    string targetPath = GetTargetPath();
                    if (PathTextBoth.Text.Length > 0)
                    {
                        string pathInstalledFolder = Path.GetDirectoryName(targetPath);
                        //MessageBox.Show(pathInstalledFolder);
                        string[] filePath = Directory.GetFiles(pathInstalledFolder);
                        foreach (String file in filePath)
                        {
                            try
                            {
                                System.IO.File.Delete(file);
                            }
                            catch
                            {

                            }
                        }
                        try { System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + shortCutName); } catch { }
                        try { System.IO.File.Delete(pathInstalledFolder); } catch { }
                    }
                }
            }

            InstallButton.Text = isInstall ? "Удалить" : "Установить";
            SwitchPathButton.Enabled = !isInstall;
        }

        private void SwitchPathButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                PathTextBoth.Text = folderBrowserDialog1.SelectedPath + installFolder;
            }
        }

        string uninstallFolder = "";

        bool isInstall
        {
            get
            {
                return System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + shortCutName);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = applicationName;

            InstallButton.Text = isInstall ? "Удалить" : "Установить";
            SwitchPathButton.Enabled = !isInstall;
            if (!isInstall) 
                if ( !System.IO.File.Exists(@".\SourcePath\" + ktsExe) )
                {
                    InstallButton.Enabled = false;
                    SwitchPathButton.Enabled = false;
                }
            if (isInstall) PathTextBoth.Text = Path.GetDirectoryName(GetTargetPath());

        
            

        }
    }
}
