using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 通用类库.获取计算机信息;

namespace 测试App
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip4 = NetAddressHelper.GetLocalIP4();
            Console.WriteLine("IPV4===" + ip4);

            var isIP4 = NetAddressHelper.CheckIsIP4(ip4);
            Console.WriteLine(isIP4);
            Console.ReadKey();
        }
    }
}
