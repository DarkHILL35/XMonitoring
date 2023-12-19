using System;
using System.IO;
using System.Management;
using System.Windows.Media;
using System.Windows;

namespace HDD
{
    public class HDD1
    {
        public (string[], Brush, Thickness) SMART()
        {
            string[] smart = new string[4];
            Brush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF13FB00"));
            smart[0] = "";
            smart[3] = "0, 0, 0, 0";
            Thickness thickness = (Thickness)new ThicknessConverter().ConvertFromString(smart[3]);
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady)
                {
                    ManagementScope scope = new ManagementScope(@"\\.\root\microsoft\windows\storage");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM MSFT_PhysicalDisk");
                    scope.Connect();
                    searcher.Scope = scope;

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        switch (Convert.ToInt16(queryObj["MediaType"]))
                        {
                            case 1:
                                smart[0] += "Unspecified";
                                break;

                            case 3:
                                smart[0] += "HDD";
                                break;

                            case 4:
                                smart[0] += "SSD";
                                break;

                            case 5:
                                smart[0] += "SCM";
                                break;

                            default:
                                smart[0] += "Unspecified";
                                break;
                        }
                    }
                    searcher.Dispose();
                    smart[0] += "(" + d.Name + ")" + "\r\n\r\n";
                    smart[0] += "Общий объём: " + d.TotalSize / 1024 / 1024 / 1024 + " ГБ\r\n";
                    smart[0] += "Свободно: " + d.AvailableFreeSpace / 1024 / 1024 / 1024 + " ГБ\r\n";
                    smart[0] += "Использовано: " + (d.TotalSize / 1024 / 1024 / 1024 - d.AvailableFreeSpace / 1024 / 1024 / 1024) + " ГБ\r\n";
                    var search = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                    // Итерация по каждому найденному физическому диску
                    foreach (var disk in search.Get())
                    {
                        smart[0] += "S.M.A.R.T: " + disk["Status"] + "\r\n";
                        break;
                    }
                    ManagementObjectSearcher searche = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSStorageDriver_ATAPISmartData");
                    try
                    {
                        foreach (ManagementObject queryObj in searche.Get())
                        {

                            byte[] arrVendorSpecific = (byte[])queryObj["VendorSpecific"];
                            int intLife = 0;
                            foreach (byte b in arrVendorSpecific)
                            {
                                intLife += (int)b;
                            }
                            smart[0] += "Ресурс: " + (((float)intLife / 50000f * 100f - 100f) * (-1f)).ToString("0") + "%" + "\r\n\r\n\r\n";
                            if ((((float)intLife / 50000f * 100f - 100f) * (-1f)) >= 80)
                            {
                                smart[1] = "Отлично(" + (((float)intLife / 50000f * 100f - 100f) * (-1f)).ToString("0") + "%" + ")";
                                smart[2] = "#FF658E49";
                                smart[3] = "245, 238, 0, 0";
                                thickness = (Thickness)new ThicknessConverter().ConvertFromString(smart[3]);
                                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(smart[2]));
                            }
                            else if ((((float)intLife / 50000f * 100f - 100f) * (-1f)) <= 80 && (((float)intLife / 50000f * 100f - 100f) * (-1f)) >= 25)
                            {
                                smart[1] = "Хорошо(" + (((float)intLife / 50000f * 100f - 100f) * (-1f)).ToString("0") + "%" + ")";
                                smart[2] = "#FFBF9000";
                                smart[3] = "238, 238, 0, 0";
                                thickness = (Thickness)new ThicknessConverter().ConvertFromString(smart[3]);
                                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(smart[2]));
                            }
                            else
                            {
                                smart[1] = "Плохо(" + (((float)intLife / 50000f * 100f - 100f) * (-1f)).ToString("0") + "%" + ")";
                                smart[2] = "#FFFB0000";
                                smart[3] = "259, 238, 0, 0";
                                thickness = (Thickness)new ThicknessConverter().ConvertFromString(smart[3]);
                                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(smart[2]));
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return (smart, brush, thickness);
        }
    }
}