using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Common
{

    public enum ShamsiDateFormatType
    {
        ToPersian,
        ToPersianWithTime,
        ToPersianMonthChar,
        ToPersianChar,
        ToPersianChar2,
        ToPersianPro
    }


    public enum HijriDateFormatType
    {
        ToHijri,
        ToHijriWithTime
    }


    public static class ExtensionMethods
    {

        // ======================================== object ======================================== //
        public static void CopyTo<T>(this T source, ref T destination)
        {
            object propValue = null;

            foreach (var srcProp in source.GetType().GetProperties())
            {
                propValue = srcProp.GetValue(source, null);

                if (srcProp.Name == "Id" || !IsCopyable(srcProp.PropertyType))
                    continue;

                destination.GetType().GetProperty(srcProp.Name).SetValue(destination, propValue);
            }
        }

        private static bool IsCopyable(Type type)
        {
            return type == typeof(String)
                     || type == typeof(long)
                     || type == typeof(int)
                     || type == typeof(byte)
                     || type == typeof(Decimal)
                     || type == typeof(Boolean)
                     || type == typeof(DateTime)
                     || type == typeof(Nullable<long>)
                     || type == typeof(Nullable<int>)
                     || type == typeof(Nullable<byte>)
                     || type == typeof(Nullable<Decimal>)
                     || type == typeof(Nullable<Boolean>)
                     || type == typeof(Nullable<DateTime>);
        }


        // ======================================== StringBuilder ======================================== //

        public static StringBuilder Trim(this StringBuilder builder, char letter = ' ')
        {
            return builder.TrimStart().TrimEnd();
        }

        public static StringBuilder TrimEnd(this StringBuilder builder, char letter = ' ')
        {
            while (builder.Length > 0 && builder[builder.Length - 1] == letter)
                builder.Length -= 1;

            return builder;
        }

        public static StringBuilder TrimStart(this StringBuilder builder, char letter = ' ')
        {
            while (builder.Length > 0 && builder[0] == letter)
                builder.Remove(0, 1);

            return builder;
        }


        // ======================================== Enum ======================================== //

        #region Enum

        /// <summary>
        /// دستیابی به اتریبیوت های دسکریپشن اینام ها
        /// </summary>
        public static string Description(this Enum value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }


        #endregion

        // ======================================== DateTime ======================================== //

        #region miladi

        public static string ToMiladiDateTime(this DateTime value)
        {
            return value.ToString("yyyy/MM/dd HH:mm");
        }

        public static string ToMiladiDate(this DateTime value)
        {
            return value.ToString("yyyy/MM/dd");
        }

        public static string ToMiladiDateTime(this DateTime? value)
        {
            if (!value.HasValue)
                return "---";

            return value.Value.ToString("yyyy/MM/dd HH:mm");
        }

        #endregion


        #region DateTime To Persian

        #region prop
        private static readonly string[] pDay = new string[] { "اول", "دوم", "سوم", "چهارم", "پنجم", "ششم", "هفتم", "هشتم", "نهم", "دهم", "یازدهم", "دوازدهم", "سیزدهم", "چهاردهم", "پانزدهم", "شانزدهم", "هفدهم", "هجدهم", "نوزدهم", "بیستم", "بیست و یکم", "بیست و دوم", "بیست و سوم", "بیست و چهارم", "بیست و پنجم", "بیست و ششم", "بیست و هفتم", "بیست و هشتم", "بیست و نهم", "سی ام", "سی و یکم" };
        private static readonly string[] pMonth = new string[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        #endregion

        #region method

        private static string ToPersianDayOfWeek(DayOfWeek value)
        {
            switch (value)
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنجشنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
                default:
                    return string.Empty;
            }
        }

        #endregion

        // --------------------------------------------------------------------------------[ 1392/6/2 ]-------------------------------------- //
        #region ToPersian
        public static string ToPersian(this DateTime value)
        {
            var pc = new PersianCalendar();

            return string.Format("{0}/{1}/{2}", pc.GetYear(value), pc.GetMonth(value) < 10 ? "0" + pc.GetMonth(value).ToString() : pc.GetMonth(value).ToString(), pc.GetDayOfMonth(value) < 10 ? "0" + pc.GetDayOfMonth(value).ToString() : pc.GetDayOfMonth(value).ToString());
        }

        public static string ToPersian(this DateTime? value)
        {
            if (value.HasValue)
            {
                var dt = (DateTime)value;
                var pc = new PersianCalendar();

                return string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt) < 10 ? "0" + pc.GetMonth(dt).ToString() : pc.GetMonth(dt).ToString(), pc.GetDayOfMonth(dt) < 10 ? "0" + pc.GetDayOfMonth(dt).ToString() : pc.GetDayOfMonth(dt).ToString());
            }
            else
            {
                return "...";
            }

        }
        #endregion


        // -------------------------------------------------------------------------------[ 1392/6/2 - 13:02 ]------------------------------- //
        #region ToPersianWithTime
        public static string ToPersianWithTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            return string.Format("{0}/{1}/{2} - {3}:{4}", pc.GetYear(value), pc.GetMonth(value), pc.GetDayOfMonth(value), value.Hour < 10 ? string.Format("0{0}", value.Hour) : value.Hour.ToString(), value.Minute < 10 ? string.Format("0{0}", value.Minute) : value.Minute.ToString());
        }

        public static string ToPersianWithTime(this DateTime? value)
        {
            if (value.HasValue)
            {
                DateTime dt = (DateTime)value;
                PersianCalendar pc = new PersianCalendar();

                return string.Format("{0}/{1}/{2} - {3}:{4}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt), dt.Hour < 10 ? string.Format("0{0}", dt.Hour) : dt.Hour.ToString(), dt.Minute < 10 ? string.Format("0{0}", dt.Minute) : dt.Minute.ToString());
            }
            else
            {
                return "...";
            }
        }
        #endregion


        // -------------------------------------------------------------------------------[ ا 2 شهریور 1392]--------------------------------- //
        #region ToPersianMonthChar
        public static string ToPersianMonthChar(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            //  2 
            // شهریور 
            // 1392 
            return string.Format("{0} {1} {2}", pc.GetDayOfMonth(value), pMonth[pc.GetMonth(value) - 1], pc.GetYear(value));

        }

        public static string ToPersianMonthChar(this DateTime? value)
        {
            if (value.HasValue)
            {

                DateTime dt = (DateTime)value;
                PersianCalendar pc = new PersianCalendar();

                //  2 
                // شهریور 
                // 1392 
                return string.Format("{0} {1} {2}", pc.GetDayOfMonth(dt), pMonth[pc.GetMonth(dt) - 1], pc.GetYear(dt));
            }
            else
            {
                return "...";
            }

        }
        #endregion


        // ----------------------------------------------------------------------------[ شنبه 2  شهریور 1392 ]------------------------------- //
        #region ToPersianChar
        public static string ToPersianChar(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            //  شنبه
            //  2 
            // شهریور 
            // 1392 
            return string.Format("{0} {1} {2} {3}", ToPersianDayOfWeek(pc.GetDayOfWeek(value)), pc.GetDayOfMonth(value), pMonth[pc.GetMonth(value) - 1], pc.GetYear(value));

        }

        public static string ToPersianChar(this DateTime? value)
        {
            if (value.HasValue)
            {
                DateTime dt = (DateTime)value;
                PersianCalendar pc = new PersianCalendar();

                //  شنبه
                //  2 
                // شهریور 
                // 1392 

                return string.Format("{0} {1} {2} {3}", ToPersianDayOfWeek(pc.GetDayOfWeek(dt)), pc.GetDayOfMonth(dt), pMonth[pc.GetMonth(dt) - 1], pc.GetYear(dt));
            }
            else
            {
                return "...";
            }

        }
        #endregion


        // ----------------------------------------------------------------------------[ شنبه دوم  شهریور ماه 1392 ]----------------------- //
        #region ToPersianChar2
        public static string ToPersianChar2(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();

            //  شنبه
            //  دوم 
            // شهریور ماه
            // 1392 

            return string.Format("{0} {1} {2} ماه {3}", ToPersianDayOfWeek(pc.GetDayOfWeek(value)), pDay[(pc.GetDayOfMonth(value) - 1)], pMonth[(pc.GetMonth(value) - 1)], pc.GetYear(value));

        }

        public static string ToPersianChar2(this DateTime? value)
        {
            if (value.HasValue)
            {

                DateTime dt = (DateTime)value;
                PersianCalendar pc = new PersianCalendar();

                //  شنبه
                //  دوم 
                // شهریور ماه
                // 1392 
                return string.Format("{0} {1} {2} ماه {3}", ToPersianDayOfWeek(pc.GetDayOfWeek(dt)), pDay[pc.GetDayOfMonth(dt) - 1], pMonth[pc.GetMonth(dt) - 1], pc.GetYear(dt));
            }
            else
            {
                return "...";
            }

        }
        #endregion


        // ----------------------------------------------------------------------------[ دیروز | امروز |  2 روز پیش | 4 روز بعد ]------------------------------- //
        #region ToPersianPro

        public static string ToPersianPro(this DateTime value)
        {
            int day = 0, month = 0, year = 0;
            var now = DateTime.Now;

            year = now.Year - value.Year;
            month = now.Month - value.Month;
            day = (year * 365) + (month * 30) + (now.Day - value.Day);

            if (now.Date == value.Date) return "امروز";

            if (now > value)
            {
                if (day == 1)   return "دیروز";
                else            return string.Format("{0} روز قبل", day);
            }

            if (now.Date < value.Date) return string.Format("{0} روز بعد", day);

            return "فردا";
        }

        public static string ToPersianPro(this DateTime? value)
        {
            if (!value.HasValue)
            {
                return "---";
            }

            return ToPersianPro(value.Value);
        }
        #endregion


        #region DateTime To Hijri


        // --------------------------------------------------------------------------------[ 1423/6/2 ]-------------------------------------- //
        #region ToHijri
        public static string ToHijri(this DateTime value)
        {
            var hc = new HijriCalendar();

            return string.Format("{0}/{1}/{2}", hc.GetYear(value), hc.GetMonth(value) < 10 ? "0" + hc.GetMonth(value).ToString() : hc.GetMonth(value).ToString(), hc.GetDayOfMonth(value) < 10 ? "0" + hc.GetDayOfMonth(value).ToString() : hc.GetDayOfMonth(value).ToString());
        }

        public static string ToHijri(this DateTime? value)
        {
            if (value.HasValue)
            {
                var dt = (DateTime)value;
                var hc = new HijriCalendar();

                return string.Format("{0}/{1}/{2}", hc.GetYear(dt), hc.GetMonth(dt) < 10 ? "0" + hc.GetMonth(dt).ToString() : hc.GetMonth(dt).ToString(), hc.GetDayOfMonth(dt) < 10 ? "0" + hc.GetDayOfMonth(dt).ToString() : hc.GetDayOfMonth(dt).ToString());
            }
            else
            {
                return "...";
            }

        }
        #endregion


        // -------------------------------------------------------------------------------[ 1423/6/2 - 13:02 ]------------------------------- //
        #region ToHijriWithTime
        public static string ToHijriWithTime(this DateTime value)
        {
            var hc = new HijriCalendar();

            return string.Format("{0}/{1}/{2} - {3}:{4}", hc.GetYear(value), hc.GetMonth(value), hc.GetDayOfMonth(value), value.Hour < 10 ? string.Format("0{0}", value.Hour) : value.Hour.ToString(), value.Minute < 10 ? string.Format("0{0}", value.Minute) : value.Minute.ToString());
        }

        public static string ToHijriWithTime(this DateTime? value)
        {
            if (value.HasValue)
            {
                var dt = (DateTime)value;
                var hc = new HijriCalendar();

                return string.Format("{0}/{1}/{2} - {3}:{4}", hc.GetYear(dt), hc.GetMonth(dt), hc.GetDayOfMonth(dt), dt.Hour < 10 ? string.Format("0{0}", dt.Hour) : dt.Hour.ToString(), dt.Minute < 10 ? string.Format("0{0}", dt.Minute) : dt.Minute.ToString());
            }
            else
            {
                return "...";
            }
        }
        #endregion


        #endregion

        #endregion

        // ======================================== String ======================================== //

        #region string

        public static string MekePageTitle(this string value)
        {
            return string.Format("| {0}", value);
        }

        public static string JustAlphabicCharacters(this string value)
        {
            return Regex.Replace(value, @"[^a-zA-Z0-9\s\u0600-\u06FF\uFB8A\u067E\u0686\u06AF]", string.Empty);
        }

        public static string ToBase64UrlEncode(this string value)
        {
            char[] padding = { '=' };
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return  System.Convert.ToBase64String(valueBytes).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
        }

        public static string FromBase64UrlEncode(this string value)
        {
            string input = value.Replace('_', '/').Replace('-', '+');
            switch (value.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
            }
            var inputBytes = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(inputBytes);
        }


        // ----------------------------------------------------------------------------------------[ Cut ]------------------------------------- //
        public static string Cut(this string value, int cutLength, string additionalData, UseAdditionalDataMode useAdditionalDataMode = UseAdditionalDataMode.IfValueNeedToCut)
        {
            bool isValueNeedToCut = (value.Length > cutLength);

            string output = isValueNeedToCut ? value.Substring(0, cutLength) : value;

            switch (useAdditionalDataMode)
            {
                case UseAdditionalDataMode.Never:
                    return output;
                case UseAdditionalDataMode.IfValueNeedToCut:
                    return isValueNeedToCut ? output + additionalData : output;
                case UseAdditionalDataMode.Always:
                    return output + additionalData;
            }

            throw new ArgumentOutOfRangeException("useAdditionalDataMode is out of range");
        }

        public enum UseAdditionalDataMode
        {
            Never,
            IfValueNeedToCut,
            Always
        }


        // ----------------------------------------------------------------------------------------[ Contains ]------------------------------------- //
        public static bool Contains(this string value, List<string> list)
        {
            return list.Any(item => value.ToLowerInvariant().IndexOf(item.ToLowerInvariant(), StringComparison.Ordinal) > -1);
        }


        public static string ToPersianNumber(this string value)
        {
            var result = string.Empty;


            foreach (char c in value.ToCharArray())
            {

                switch (c)
                {
                    case '0':
                        result += "٠";
                        break;
                    case '1':
                        result += "١";
                        break;
                    case '2':
                        result += "٢";
                        break;
                    case '3':
                        result += "٣";
                        break;
                    case '4':
                        result += "۴";
                        break;
                    case '5':
                        result += "۵";
                        break;
                    case '6':
                        result += "۶";
                        break;
                    case '7':
                        result += "٧";
                        break;
                    case '8':
                        result += "٨";
                        break;
                    case '9':
                        result += "٩";
                        break;
                    default:
                        result += c;
                        break;
                }
            }


            return result;

        }


        // ----------------------------------------------------------------------------------------[ ContainsNumber ]------------------------------------- //
        public static bool ContainsNumber(this string value)
        {
            foreach (char chr in value.ToCharArray())
            {
                if (Char.IsDigit(chr))
                    return true;
            }

            return false;
        }


        // ----------------------------------------------------------------------------------------[ ToMiladiDate ]------------------------------------- //
        public static DateTime ToMiladiDate(this string value, char seperator = '/')
        {
            PersianCalendar pc = new PersianCalendar();

            string[] a = value.Split(seperator);

            if (a.Length != 3)
                throw new ArgumentException("Invalid Date Format");

            DateTime outputDate = new DateTime(int.Parse(a[0]), int.Parse(a[1]), int.Parse(a[2]), pc);

            return outputDate;
        }

        public static string AddHtmlTags(this string value)
        {
            return value.Replace("\r", "<br/>");
        }

        public static string RemoveHtmlTags(this string value)
        {
            return value.Replace("<br/>", "\r");
        }


        // ----------------------------------------------------------------------------------------[ NoCamma ]------------------------------------- //
        #region NoCamma
        public static string NoCamma(this string value)
        {
            return value.Trim().Replace(",", "");
        }
        #endregion

        #endregion


        // ==================================================================int====================================================================== //

        #region int

        public static string ToPersianNumber(this int value)
        {

            string result = string.Empty;


            foreach (char c in value.ToString().ToCharArray())
            {

                switch (c)
                {
                    case '0':
                        result += "٠";
                        break;
                    case '1':
                        result += "١";
                        break;
                    case '2':
                        result += "٢";
                        break;
                    case '3':
                        result += "٣";
                        break;
                    case '4':
                        result += "۴";
                        break;
                    case '5':
                        result += "۵";
                        break;
                    case '6':
                        result += "۶";
                        break;
                    case '7':
                        result += "٧";
                        break;
                    case '8':
                        result += "٨";
                        break;
                    case '9':
                        result += "٩";
                        break;
                    default:
                        result += c;
                        break;
                }
            }

            return result;
        }


        public static string ToPersianNumber(this int? value)
        {

            if (!value.HasValue)
            {
                return "٠";
            }

            string result = string.Empty;


            foreach (char c in value.ToString().ToCharArray())
            {

                switch (c)
                {
                    case '0':
                        result += "٠";
                        break;
                    case '1':
                        result += "١";
                        break;
                    case '2':
                        result += "٢";
                        break;
                    case '3':
                        result += "٣";
                        break;
                    case '4':
                        result += "۴";
                        break;
                    case '5':
                        result += "۵";
                        break;
                    case '6':
                        result += "۶";
                        break;
                    case '7':
                        result += "٧";
                        break;
                    case '8':
                        result += "٨";
                        break;
                    case '9':
                        result += "٩";
                        break;
                    default:
                        result += c;
                        break;
                }
            }


            return result;

        }

        #endregion


        // ==================================================================bool====================================================================== //

        #region bool

        public static string ToPersian(this bool value)
        {
            return value ? "بله" : "خیر";
        }

        public static string ToPersian(this bool? value)
        {
            if (value.HasValue)
                return value.Value ? "بله" : "خیر";
            else
                return string.Empty;
        }


        #endregion

    }

}