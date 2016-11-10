using System;

namespace Maybe2
{
    public static class DateTimeExtensions
    {
        public static string[] months = new string[] { "Янв", "Фев", "Мрт", "Апр", "Мая", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек" };
        public static string[] MONTHS = new string[] { "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа", "Сентября", "Октября", "Ноября", "Декабря" };


        public static Tuple<string, string, string> GetRelativeMailString(this DateTime date, DateTime? point = null)
        {
            var hhmm = date.ToString("HH:mm");

            DateTime pointDate = point.NoNull(c => point.Value, DateTime.Now);

            TimeSpan ts = pointDate.TrimHour() - date.TrimHour();


            var rs = GetRelativeString(date, point);

            var days = (int)ts.TotalDays;

            if (days == 0)
            {
                return Tuple.Create(hhmm, "Сегодня, " + hhmm, rs);
            }

            if (days == 1)
            {
                return Tuple.Create(date.Day + " " + months[date.Month - 1].ToLower(), "Вчера, " + hhmm, rs);
            }

            if (pointDate.Year == date.Year)
            {
                return Tuple.Create(date.Day + " " + months[date.Month - 1].ToLower(), date.Day + " " + MONTHS[date.Month - 1].ToLower() + ", " + hhmm, rs);
            }

            return Tuple.Create(date.ToString("dd.MM.yy"), date.Day + " " + MONTHS[date.Month - 1].ToLower() + " " + date.Year + ", " + hhmm, rs);
        }


        public static string GetRelativeString(this DateTime date, DateTime? point = null)
        {
            DateTime pointDate = point.NoNull(c => point.Value, DateTime.Now);

            TimeSpan ts = pointDate - date; //var ts = new TimeSpan(DateTime.UtcNow.Ticks - dt.Ticks);

            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 60)
            {
                return ts.Seconds == 1 ? "сейчас" : ts.Seconds + " сек";
            }
            if (delta < 120)
            {
                return "1 мин";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " мин";
            }
            if (delta < 5400) // 90 * 60
            {
                return "1 ч";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " ч";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "1 дн";//"вчера";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " дн";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months + " мес";
            }
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years.getForm(" год", " лет", " года");
        }


        public static int GetLastDayOfMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }
    }

    public static class TimeTrim
    {

        #region Month

        // ----------------------------------------------------------------------
        public static DateTime TrimMonth(this DateTime dateTime, int month = 1, int day = 1,
            int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
        {
            return new DateTime(dateTime.Year, month, day, hour, minute, second, millisecond);
        } // Month

        #endregion

        #region Day

        // ----------------------------------------------------------------------
        public static DateTime TrimDay(this DateTime dateTime, int day = 1,
            int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
        {
            if (day < 1 || day > dateTime.GetLastDayOfMonth())
                throw new ArgumentException("Некорректое значение дня!");

            return new DateTime(dateTime.Year, dateTime.Month, day, hour, minute, second, millisecond);
        } // Day

        #endregion


        public static DateTime TrimWeek(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            return dateTime.StartOfWeek(startOfWeek);
        }


        #region Hour

        // ----------------------------------------------------------------------
        public static DateTime TrimHour(this DateTime dateTime, int hour = 0,
            int minute = 0, int second = 0, int millisecond = 0)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, minute, second, millisecond);
        } // Hour

        #endregion

        #region Minute

        // ----------------------------------------------------------------------
        public static DateTime TrimMinute(this DateTime dateTime, int minute = 0, int second = 0, int millisecond = 0)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, second, millisecond);
        } // Minute

        #endregion

        #region Second

        // ----------------------------------------------------------------------
        public static DateTime TrimSecond(this DateTime dateTime, int second = 0, int millisecond = 0)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, second, millisecond);
        } // Second

        #endregion

        #region Millisecond

        // ----------------------------------------------------------------------
        public static DateTime TrimMillisecond(this DateTime dateTime, int millisecond = 0)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, millisecond);
        } // Millisecond

        #endregion

    } // class TimeTrim

}
