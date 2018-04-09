namespace PingPorts
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;


    class Program
    {
        public static IPAddress IncrementIPAddress(ref IPAddress ip)
        {
            byte[] tempIp = ip.GetAddressBytes();
            tempIp[3] = (byte)(tempIp[3] + 1);
            if (tempIp[3] == 0)
            {
                tempIp[2] = (byte)(tempIp[2] + 1);
                if (tempIp[2] == 0)
                {
                    tempIp[1] = (byte)(tempIp[1] + 1);
                    if (tempIp[1] == 0)
                    {
                        tempIp[0] = (byte)(tempIp[0] + 1);
                    }
                }
            }

            ip = new IPAddress(tempIp);
            return ip;
        }

        private const string nameResult = "IPs.txt";
        private static string path = Directory.GetCurrentDirectory() + @"\" + nameResult;

        static void Main(string[] args)
        {
            if (!string.IsNullOrEmpty(Directory.GetFiles(Directory.GetCurrentDirectory(), nameResult).FirstOrDefault()))
            {
                File.Delete(path);
            }
            File.Create(path).Close();
            IPAddress ip1, ip2;
            int fPort;
            int sPort;
            Console.WriteLine("Введите по очереди ограничение для ip и портов:");
            if (IPAddress.TryParse(Console.ReadLine(), out ip1) && IPAddress.TryParse(Console.ReadLine(), out ip2) && int.TryParse(Console.ReadLine(), out fPort) && int.TryParse(Console.ReadLine(), out sPort))
            {
                while (ip1.ToString() != ip2.ToString())
                {
                    for (var i = fPort; i <= sPort; i++)
                    {
                        try
                        {
                            var soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            var result = soc.BeginConnect(ip1, i, null, null);
                            bool success = result.AsyncWaitHandle.WaitOne(200, true);
                            if (success)
                            {
                                using (var sw = new StreamWriter(path, true))
                                {

                                    sw.WriteLine(ip1.ToString() + ":" + i);
                                    Console.WriteLine(ip1.ToString() + ":" + i);
                                }
                            }
                            soc.Disconnect(false);
                        }
                        catch
                        {
                        }
                    }
                    IncrementIPAddress(ref ip1);
                }
            }
            else
            {
                Console.WriteLine("Введены некорректные данные");
            }
        }
    }
}




//инфо о ip
//Console.WriteLine("Информация о Ip");
//var req = WebRequest.Create("https://www.nic.ru/whois/?query=" + ipadd);
//var resp = req.GetResponse().GetResponseStream();
//using (StreamReader sr = new StreamReader(resp))
//{
//    Regex a = new Regex("<div class=\"b-whois-info__info\">([\\s\\S]*?)<\\/div>");
//    Console.WriteLine(a.Match(sr.ReadToEnd()).Value
//        .Replace("<div class=\"b-whois-info__info\">", "")
//        .Replace("</div>", "")
//        .Replace("<br>", "")
//        .Replace("&nbsp;", "")
//        .Trim());
//}