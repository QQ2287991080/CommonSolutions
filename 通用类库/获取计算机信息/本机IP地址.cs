using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.获取计算机信息
{
    public class 本机IP地址
    {
       
    }

    public static class NetAddressHelper
    {
        public static string GetLocalIP6()
        {
            return string.Empty;
        }

        private static string GetLocalIP4Temp()
        {
            IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
            IPAddress ip4 = addressList.FirstOrDefault
                    (
                            _ => _.AddressFamily == AddressFamily.InterNetwork
                                    && !IPAddress.IsLoopback(_)
                    );

            if (ip4 == null)
            {
                ip4 = addressList.FirstOrDefault();
                if (ip4 == null)
                {
                    ip4 = IPAddress.Loopback;
                }
            }

            return ip4.ToString();
        }
        public static string GetLocalIP4()
        {
            //IPAddress[] addressList = Dns.GetHostAddresses(Dns.GetHostName());
            //IPAddress ip4 = addressList.FirstOrDefault
            //        (
            //                _ => _.AddressFamily == AddressFamily.InterNetwork
            //                        && !IPAddress.IsLoopback(_)
            //        );

            //if (ip4 == null)
            //{
            //        ip4 = addressList.FirstOrDefault();
            //        if (ip4 == null)
            //        {
            //                ip4 = IPAddress.Loopback;
            //        }
            //}

            //return ip4.ToString();


            try
            {
                UnicastIPAddressInformation mostSuitableIp = null;

                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
                string strAddress = string.Empty;

                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                var xx = host.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);

                foreach (var network in networkInterfaces)
                {
                    if (network.OperationalStatus != OperationalStatus.Up)
                        continue;

                    var properties = network.GetIPProperties();

                    //if (properties == null)
                    //{
                    //        Console.WriteLine("network.GetIPProperties returns null.");
                    //}

                    //if (properties.GatewayAddresses == null)
                    //{
                    //        Console.WriteLine("properties.GatewayAddresses is null");
                    //}

                    if (properties.GatewayAddresses.Count == 0)
                    {
                        continue;
                    }

                    foreach (var address in properties.UnicastAddresses)
                    {
                        //if (address.Address == null)
                        //{
                        //        Console.WriteLine("address.Address is null.");
                        //}

                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            continue;
                        }

                        if (IPAddress.IsLoopback(address.Address))
                        {
                            continue;
                        }

                        if (!address.IsDnsEligible)
                        {
                            if (mostSuitableIp == null)
                                mostSuitableIp = address;
                            continue;
                        }

                        // The best IP is the IP got from DHCP server
                        if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                        {
                            if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                                mostSuitableIp = address;
                            continue;
                        }

                        strAddress = address.Address.ToString();
                    }
                }

                strAddress = mostSuitableIp != null
                    ? mostSuitableIp.Address.ToString()
                    : strAddress;

                if (string.IsNullOrEmpty(strAddress))
                {
                    return GetLocalIP4Temp();
                }
                else
                {
                    return strAddress;
                }
            }
            catch (Exception)
            {
                //return "127.0.0.1";

                return GetLocalIP4Temp();
            }











            //NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

            //var local = from p in interfaces
            //            where p.OperationalStatus == OperationalStatus.Up
            //            let ips = p.GetIPProperties()
            //            from ip in ips.UnicastAddresses
            //            where ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
            //            select ip.Address.ToString();





            //var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //ManagementObjectCollection moc = mc.GetInstances();
            //string mac = string.Empty;

            //foreach (ManagementObject mo in moc)
            //{
            //        if (mo["IPEnabled"].ToString() == "True")
            //        {
            //                Debug.WriteLine(mo["MacAddress"].ToString());
            //                Debug.WriteLine(((string[])mo["IPAddress"])[0]);
            //        }
            //}



            //var info = new ProcessStartInfo
            //{
            //        FileName = "ipconfig",
            //        Arguments = "/all",
            //        RedirectStandardOutput = true,
            //        UseShellExecute = false
            //};
            //var p1 = Process.Start(info);
            //var result = p1.StandardOutput.ReadToEnd();
            //return string.Empty;
        }

        public static bool CheckIsIP4(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                return false;
            }

            string[] str = ip.Split('.');

            if (str.Length == 4)
            {
                int a, b, c, d;
                if (int.TryParse(str[0], out a)
                        && int.TryParse(str[1], out b)
                        && int.TryParse(str[2], out c)
                        && int.TryParse(str[3], out d))
                {
                    if (a <= 255 && b <= 255 && c <= 255 && d <= 255)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
