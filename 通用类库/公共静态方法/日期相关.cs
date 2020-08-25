using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 通用类库.公共静态方法
{
    public static class 日期相关
    {
        #region 时间戳
        /// <summary>
        /// 获取时间戳（毫秒级）
        /// </summary>
        /// <returns></returns>
        public static long TimeLongUnxt(DateTime date)
        {
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (date.Ticks - dtFrom.Ticks) / 10000;
        }
        /// <summary>
        /// 获取时间戳（秒级）
        /// </summary>
        /// <returns></returns>
        public static long TimeIntUnxt(DateTime date)
        {
            DateTime dtFrom = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (date.Ticks - dtFrom.Ticks) / 10000000;
        }
        #endregion

        #region 日期
        /// <summary>
        /// 根据不同的文化来获取时间属于年份第几周
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="cultue"></param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime dt, string cultue = "zh-CN")
        {
            CultureInfo ci = new CultureInfo("zh-CN");
            return ci.Calendar.GetWeekOfYear(dt, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        }
        /// <summary>
        ///  获取星期几
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static string GetWeekStr(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Sunday)
            {
                return "星期日";
            }
            else if (dayOfWeek == DayOfWeek.Monday)
            {
                return "星期一";
            }
            else if (dayOfWeek == DayOfWeek.Tuesday)
            {
                return "星期二";
            }
            else if (dayOfWeek == DayOfWeek.Wednesday)
            {
                return "星期三";
            }
            else if (dayOfWeek == DayOfWeek.Thursday)
            {
                return "星期四";
            }
            else if (dayOfWeek == DayOfWeek.Friday)
            {
                return "星期五";
            }
            else if (dayOfWeek == DayOfWeek.Saturday)
            {
                return "星期六";
            }
            return "";
        }
        /// <summary>
        /// 判断当前日期是不是周末
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public static bool IsWeek(DayOfWeek dayOfWeek)
        {
            bool isWeek = false;
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    isWeek = true;
                    break;
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    break;
                case DayOfWeek.Wednesday:
                    break;
                case DayOfWeek.Thursday:
                    break;
                case DayOfWeek.Friday:
                    break;
                case DayOfWeek.Saturday:
                    isWeek = true;
                    break;
                default:
                    break;
            }
            return isWeek;
        }
        #endregion
    }
}
