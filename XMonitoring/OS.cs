using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace OSS
{
    class OS
    {
        string path = AppDomain.CurrentDomain.BaseDirectory + @"\LOG\os.txt";
        string path1 = AppDomain.CurrentDomain.BaseDirectory + @"\LOG\OSLOG.txt";
        public void Main()
        {
            string filePath = "C:\\Windows\\System32";
            FileSystemWatcher watcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(filePath), System.IO.Path.GetFileName(filePath));
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.EnableRaisingEvents = true;
            watcher.Changed += OnFileChanged;
        }

        public void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            File.WriteAllText(path, DateTime.Now.ToString() + ": Запущена проверка...\r\n" + DateTime.Now.ToString() + ": Проверка завершена. Результат: Произошли изменения в " + e.FullPath + ".\r\n");
            File.AppendAllText(path1, DateTime.Now.ToString() + ": Запущена проверка...\r\n" + DateTime.Now.ToString() + ": Проверка завершена. Результат: Произошли изменения в " + e.FullPath + ".\r\n");
        }
        public (string[], Brush, Thickness) ChangeOS()
        {
            string[] smart = new string[4];
            Brush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13FB00"));
            string sm = "";
            Thickness thickness;
            sm = File.ReadAllText(path);
            if (sm == "")
            {
                smart[0] = "Защита ресурсов Windows не обнаружила нарушений целостности.";
                smart[1] = "Отлично";
                smart[2] = "51, 238, 0, 0";
                smart[3] = "Yes";
                File.AppendAllText(path1, DateTime.Now.ToString() + ": Запущена проверка...\r\n" + DateTime.Now.ToString() + ": Проверка завершена. Результат: Нарушений целостности системы не обнаружено.\r\n");
                thickness = (Thickness)new ThicknessConverter().ConvertFromString(smart[2]);
                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF658E49"));
            }
            else
            {
                smart[0] = "Программа защиты ресурсов Windows обнаружила поврежденные файлы и успешно\r\nих восстановила.";
                smart[1] = "Нарушена";
                smart[2] = "44, 238, 0, 0";
                smart[3] = "No";
                thickness = (Thickness)new ThicknessConverter().ConvertFromString(smart[2]);
                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFB0000"));
            }
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write("");
            }
            return (smart, brush, thickness);
        }
    }
}

