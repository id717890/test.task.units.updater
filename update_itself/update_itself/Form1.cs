namespace update_itself
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetArguments();
            GetCurrentVersion();
        }

        /// <summary>
        /// Проверка аргументов с которыми запущено приложение
        /// </summary>
        private void GetArguments()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length >= 2)
            {
                if (args[1].ToLower() == "showversion")
                {
                    if (MessageBox.Show(String.Format("Версия файла {0}", GetVersion().FileVersion), @"Info", MessageBoxButtons.OK) == DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }

        /// <summary>
        /// Выводит в Label текущую версию файла
        /// </summary>
        private void GetCurrentVersion()
        {
            label1.Text = @"File Version - " + GetVersion().FileVersion;
        }

        /// <summary>
        /// Получает экзепляр FileVersionInfo текущего приложения
        /// </summary>
        /// <returns></returns>
        private FileVersionInfo GetVersion()
        {
            return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetEntryAssembly().Location);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var currentFile = System.Reflection.Assembly.GetEntryAssembly().Location;
            var newFile = Application.StartupPath + "\\new\\" + ConfigurationManager.AppSettings["name_new_file"];

            //Запускаем updater, который обновит файлы
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Application.StartupPath + "\\" + ConfigurationManager.AppSettings["name_updater"],
                Arguments = newFile + " " + currentFile
            };
            Process.Start(startInfo);
            Environment.Exit(0);
        }
    }
}
