using System;
using System.Text;

namespace Infrastructure.Common
{
    public class RandomMaker
    {

        #region CreateRandomName

        /// <summary>
        /// ساخت یک عبارت رندوم
        /// </summary>
        public string CreateRandomName(int length)
        {
            var rnd = new Random((int) DateTime.Now.Ticks);
            var temp = new StringBuilder();
            var flag = 1;

            for (var i = 0; i < length; i++)
            {
                flag = rnd.Next(0, 15);

                if (flag < 5)
                    temp.Append(Convert.ToChar(rnd.Next(97, 123))); // lower
                else if (flag >= 5 && flag < 10)
                    temp.Append(Convert.ToChar(rnd.Next(49, 58))); // numeric
                else
                    temp.Append(Convert.ToChar(rnd.Next(65, 91))); // biger
            }

            return temp.ToString();
        }

        #endregion


        #region CreateRandomNumbers

        /// <summary>
        /// ساخت یک عبارت عددی رندوم
        /// </summary>
        public string CreateRandomNumbers(int length)
        {
            var rnd = new Random((int) DateTime.Now.Ticks);
            var temp = new StringBuilder();

            for (var i = 0; i < length; i++)
                temp.Append(Convert.ToChar(rnd.Next(49, 58)));

            return temp.ToString();
        }

        #endregion


        #region CreateVeryUniqeString
        /// <summary>
        /// ساخت یک عبارت یونیک
        /// </summary>
        public string CreateVeryUniqeString()
        {
            return Guid.NewGuid().ToString("N");
        }
        #endregion


        #region CreateRandomFileName
        /// <summary>
        /// ساخت نام تصادفی برای فایل ها
        /// </summary>
        /// <param name="userId">آی دی کاربر</param>
        /// <param name="extension">پسوند فایل</param>
        /// <returns></returns>
        public string CreateRandomFileName(string additionalInfo)
        {
            return string.Format("{0}_{1}{2}",
                DateTime.Now.ToString("yyyy-MM-dd"), 
                CreateVeryUniqeString(),
                additionalInfo);
        }

        #endregion

    }

}