using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Linq;
using System.Web;

namespace Seti
{
    public class NetworkCardInfo
    {
        public string LocalIPAddress { get; set; }
    }
    //Контроль активных сетевых сессий
    class NetworkSessionWatcher
    {
        public string SessionWatcher()
        {
            NetworkInterface[] networkCards = NetworkInterface.GetAllNetworkInterfaces();
            string s = "";
            foreach (NetworkInterface networkCard in networkCards)
            {
                NetworkCardInfo cardInfo = new NetworkCardInfo
                {
                    LocalIPAddress = GetLocalIPAddress()
                };

                s = "Активные сетевые сессии:\r\n";
                s += "Локальный IP-адрес: " + cardInfo.LocalIPAddress + "\r\n";
                if (cardInfo.LocalIPAddress != "Сетевое подключение отсутствует")
                {
                    TcpConnectionInformation connection = GetActiveTcpConnection();
                    if (connection != null)
                        s += "Удаленный IP-адрес: " + connection.RemoteEndPoint.Address + "\r\n\r\n";
                }
                else
                    s += "Удаленный IP-адрес: Сетевое подключение отсутствует" + "\r\n\r\n";
                break;
            }
            return s;
        }

        public static string GetLocalIPAddress()
        {
            string ipAddress = "";
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface ni in interfaces)
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    IPInterfaceProperties ipProps = ni.GetIPProperties();
                    foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                    {
                        if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipAddress = addr.Address.ToString();
                            break;
                        }
                    }
                }
                try
                {
                    Ping ping = new Ping();
                    PingReply reply = ping.Send("google.com", 1000);

                    if (reply.Status != IPStatus.Success)
                        ipAddress = "Сетевое подключение отсутствует";
                }
                catch (Exception ex)
                {
                    ipAddress = "Сетевое подключение отсутствует";
                }
            }
            return ipAddress;
        }
            TcpConnectionInformation GetActiveTcpConnection()
            {
                // Получение всех активных TCP-соединений
                IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                TcpConnectionInformation[] connections = ipProperties.GetActiveTcpConnections();

                // Фильтрация и выбор первого соединения
                TcpConnectionInformation connection = connections.FirstOrDefault(c => c.State == TcpState.Established);

                return connection;
            }
        public (string[], Brush, Thickness) Internet()
        {
            string[] internet = new string[4];
            Brush brush;
            Thickness thickness;
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("google.com", 1000); 

                if (reply.Status == IPStatus.Success)
                {
                    internet[0] = "Соединение с интернетом: Установлено";
                    internet[1] = "Доступ к интернету";
                    internet[2] = "#FF658E49";
                    internet[3] = "405, 238, 0, 0";
                    thickness = (Thickness)new ThicknessConverter().ConvertFromString(internet[3]);
                    brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(internet[2]));
                }
                else
                {
                    internet[0] = "Соединение с интернетом: Нет соединения";
                    internet[1] = "Не подключено";
                    internet[2] = "#FFFB0000";
                    internet[3] = "422, 238, 0, 0";
                    thickness = (Thickness)new ThicknessConverter().ConvertFromString(internet[3]);
                    brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(internet[2]));
                }
                return (internet, brush, thickness);
            }
            catch (Exception ex)
            {
                internet[0] = "Соединение с интернетом: Нет соединения";
                internet[1] = "Не подключено";
                internet[2] = "#FFFB0000";
                internet[3] = "422, 238, 0, 0";
                thickness = (Thickness)new ThicknessConverter().ConvertFromString(internet[3]);
                brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(internet[2]));
                return (internet, brush, thickness);
            }
        }
        public string[] Adapter()
        {
            // Получаем все сетевые интерфейсы компьютера
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            string[] adapter = new string[3];
            // Ищем интерфейс, через который идет подключение (он должен иметь статус "Up")
            NetworkInterface activeInterface = null;
            // Проходимся в цикле по всем интерфейсам
            foreach (NetworkInterface nic in interfaces)
            {
                if (nic.OperationalStatus == OperationalStatus.Up &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    IPInterfaceProperties ipProps = nic.GetIPProperties();
                    if (ipProps.GatewayAddresses.Count > 0)
                    {
                        activeInterface = nic;
                        break;
                    }
                }
            }

            // Выводим имя активного интерфейса, если он найден
            if (activeInterface != null)
            {
                adapter[0] += activeInterface.Description;
                adapter[1] += "Состояние сетевого адаптера: Включен\r\n";
                adapter[2] = "Yes";
                return adapter;
            }
            else
            {
                adapter[0] += "Сетевое подключение не обнаружено\r\n";
                adapter[1] += "Состояние сетевого адаптера: Отключен\r\n";
                adapter[2] = "No";
                return adapter;
            }
        }
    }
}
