using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn.Provider.Common
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 获取一周的开始日期
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime ToCurremtStartWeek(this DateTime currentDateTime)
        {
            var startWeek = currentDateTime.AddDays(1 - Convert.ToInt32(currentDateTime.DayOfWeek.ToString("d")));
            var _startWeek = DateTime.Parse(startWeek.ToString("d"));
            return _startWeek;
        }

        /// <summary>
        /// 获取本周星期天日期
        /// </summary>
        /// <param name="startWeek"></param>
        /// <returns></returns>
        public static DateTime ToCurrentEndWeek(this DateTime startWeek)
        {
            var _endweek = DateTime.Parse(startWeek.AddDays(6).ToString("d"));
            return _endweek;
        }

        /// <summary>
        /// 数字转换时间格式
        /// </summary>
        /// <param name="timeStr">数字,如:42095.7069444444/0.650694444444444</param>
        /// <returns>日期/时间格式</returns>
        public static string ToDateTimeValue(string strNumber)
        {
            if (!string.IsNullOrWhiteSpace(strNumber))
            {
                Decimal tempValue;
                //先检查 是不是数字;
                if (Decimal.TryParse(strNumber, out tempValue))
                {
                    //天数,取整
                    int day = Convert.ToInt32(Math.Truncate(tempValue));
                    //这里也不知道为什么. 如果是小于32,则减1,否则减2
                    //日期从1900-01-01开始累加 
                    // day = day < 32 ? day - 1 : day - 2;
                    DateTime dt = new DateTime(1900, 1, 1).AddDays(day < 32 ? (day - 1) : (day - 2));

                    //小时:减掉天数,这个数字转换小时:(* 24) 
                    Decimal hourTemp = (tempValue - day) * 24;//获取小时数
                                                              //取整.小时数
                    int hour = Convert.ToInt32(Math.Truncate(hourTemp));
                    //分钟:减掉小时,( * 60)
                    //这里舍入,否则取值会有1分钟误差.
                    Decimal minuteTemp = Math.Round((hourTemp - hour) * 60, 2);//获取分钟数
                    int minute = Convert.ToInt32(Math.Truncate(minuteTemp));
                    //秒:减掉分钟,( * 60)
                    //这里舍入,否则取值会有1秒误差.
                    Decimal secondTemp = Math.Round((minuteTemp - minute) * 60, 2);//获取秒数
                    int second = Convert.ToInt32(Math.Truncate(secondTemp));
                    if (second >= 60) second = 59;
                    if (minute >= 60) { minute = 59; second = 59; }
                    if (hour >= 24) { hour = 23; minute = 59; second = 59; }

                    //时间格式:00:00:00
                    string resultTimes = string.Format("{0}:{1}:{2}",
                            (hour < 10 ? ("0" + hour) : hour.ToString()),
                            (minute < 10 ? ("0" + minute) : minute.ToString()),
                            (second < 10 ? ("0" + second) : second.ToString()));

                    if (day > 0)
                        return string.Format("{0} {1}", dt.ToString("yyyy-MM-dd"), resultTimes);
                    else
                        return resultTimes;
                }
                else
                {
                    DateTime Dt = DateTime.Parse("1900-01-01");
                    if (DateTime.TryParse(strNumber, out Dt))
                    {
                        return Dt.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            return string.Empty;
        }
    }
}
