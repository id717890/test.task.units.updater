namespace updater
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            //Проверяем что пришло как минимум два аргумента
            if (args.Length < 2)
            {
                Console.WriteLine("Обновление невозможно. Нужно указать два аргумента (новому файлу, старый файлы)");
                Console.ReadLine();
                Environment.Exit(0);
            }

            Console.WriteLine(args[0]);
            Console.WriteLine(args[1]);
            Console.WriteLine("Начато обновление файла");
            Thread.Sleep(1000);

            var file1 = args[0];
            var file2 = args[1];
            var pathToExecutable = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            //Проверяем существует ли файл №1 (новая версия)
            if (!File.Exists(file1))
            {
                Console.WriteLine("Файл {0} не найден", file1);
                Console.ReadLine();
                Environment.Exit(0);
            }
            
            //Проверяем существует ли файл №2 (обновляемый файл)
            if (!File.Exists(file2))
            {
                Console.WriteLine("Файл {0} не найден", file2);
                Console.ReadLine();
                Environment.Exit(0);
            }

            var versionNew = FileVersionInfo.GetVersionInfo(file1).FileVersion;
            var versionOld = FileVersionInfo.GetVersionInfo(file2).FileVersion;

            //Если версии файлов одинаковые то обновление не требуется
            if (versionNew == versionOld)
            {
                Console.WriteLine("Версии файлов совпадают, обновление не требуется.");
                Console.WriteLine("Для продолжения нажмите Enter");
                Console.ReadLine();
                var upgradeProcess = new ProcessStartInfo(file2) { WorkingDirectory = pathToExecutable };
                Process.Start(upgradeProcess);
                Environment.Exit(0);
            }

            isrun:
            //Проверяем запущен ли файл который нужно обновить
            bool isRunning = Process.GetProcessesByName(ConfigurationManager.AppSettings["name_app"]).FirstOrDefault() != default(Process);
            if (isRunning)
            {
                Console.WriteLine("Файл {0} используется. Зайкройте его и нажмите Enter", file2);
                Console.ReadLine();
                goto isrun;
            }

            File.Delete(file2);
            File.Copy(file1, file2, true);

            if (File.Exists(file2))
            {
                ProcessStartInfo upgradeProcess = new ProcessStartInfo(file2) {WorkingDirectory = pathToExecutable};
                Process.Start(upgradeProcess);
                Environment.Exit(0);
            }
        }
    }
}
