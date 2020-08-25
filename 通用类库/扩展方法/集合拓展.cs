using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.扩展方法
{
    public static class 集合拓展
    {
        /// <summary>
        /// 判断是集合是否为空或者数量等于0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source != null && source.Count > 0;
        }
    }
}
