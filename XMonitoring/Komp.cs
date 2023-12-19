using DK.DatabaseSchema;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Management;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Media;

namespace Komputer
{
    public class Komp
    {
        public (string[], Brush, Thickness) SaveConfig()
        {
            Brush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13FB00"));
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\LOG\Log.txt";
            string[] changekomp = new string[5];
            string izm = "";
            Thickness thickness;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            ManagementObjectCollection collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {
                izm += changekomp[3] = obj["Name"] + "";
            }
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
            collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {
                izm += changekomp[0] += "Материнская плата:             " + obj["Product"] + " (SN: " + obj["SerialNumber"] + ")\r\n";
            }
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            collection = searcher.Get();
            foreach (ManagementObject obj in collection)
            {
                izm += changekomp[0] += "Центральный процессор:     " + obj["Name"] + " (ID: " + obj["ProcessorId"] + ")\r\n";
            }
            int i = 0;
            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                string model = queryObj["PartNumber"].ToString();
                ulong capacityInBytes = Convert.ToUInt64(queryObj["Capacity"]);
                double capacityInGb = capacityInBytes / (1024 * 1024 * 1024);
                uint frequency = Convert.ToUInt32(queryObj["Speed"]);
                i++;
                izm += changekomp[0] += "Оперативная память " + i + $":        {model} {frequency} МГц {capacityInGb} ГБ\r\n";
            }
            int p = 0;
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE MediaType='Fixed hard disk media'");
            foreach (ManagementObject obj in searcher.Get())
            {
                string model = obj["Model"].ToString();
                ulong capacityInBytes = Convert.ToUInt64(obj["Size"]);
                double capacityInGb = capacityInBytes / (1024 * 1024 * 1024);
                p++;
                izm += changekomp[0] += "Внутренний накопитель " + p + $":  {model} {capacityInGb}ГБ\r\n";
            }

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
            foreach (ManagementObject obj in searcher.Get())
            {
                string model = obj["Name"].ToString();
                ulong videoMemoryInBytes = Convert.ToUInt64(obj["AdapterRAM"]);
                double videoMemoryInGb = videoMemoryInBytes / (1024 * 1024 * 1024);
                izm += changekomp[0] += $"Видеокарта:                           {model} {videoMemoryInGb}ГБ\r\n";
            }
            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE PNPDeviceID LIKE 'PCI%'");
            foreach (ManagementObject obj in searcher.Get())
            {
                    string macAddress = (string)obj["MACAddress"];
                    string model = (string)obj["Name"];
                    izm += changekomp[0] += "Сетевая карта" + $":                      {model} (MAC: {macAddress})\r\n";
                    break;
            }
            string[] fileLines = File.ReadAllLines(path);
            foreach (string line in fileLines)
            {
                if (!izm.Contains(line))
                {
                    changekomp[4] = DateTime.Now.ToString() + "  изменен(а) " + line + " на компонент из списка выше\r\n";    
                }
            }
            string fileContent;
            using (StreamReader reader = new StreamReader(path))
            {
                fileContent = reader.ReadToEnd();
            }
            if (fileContent == changekomp[3] + changekomp[0])
            {
                changekomp[1] = "Нет изменений";
                changekomp[2] = "624, 238, 0, 0";
                thickness = (Thickness)new ThicknessConverter().ConvertFromString(changekomp[2]);
                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF286586"));
            }
            else
            {
                changekomp[1] = "Изменены";
                changekomp[2] = "643, 238, 0, 0";
                thickness = (Thickness)new ThicknessConverter().ConvertFromString(changekomp[2]);
                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFB0000"));
                File.WriteAllText(path, changekomp[3] + changekomp[0]);
            }
            return (changekomp, brush, thickness);
        }
    }
}
