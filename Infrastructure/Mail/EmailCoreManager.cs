using System.IO;
using System.Web.Hosting;
// ReSharper disable All

namespace Infrastructure.Mail
{
    public static class EmailCoreManager
    {

        #region New Member Creation Email Pattern

        private const string NEW_MEMBER_CREATION_PATTERN_FILE_PATH = "~/Views/EmailPattern/_NewUserCreation.cshtml";

        public static string GetNewMemberCreationEmailPattern()
        {
            using (var reader= new StreamReader(HostingEnvironment.MapPath(NEW_MEMBER_CREATION_PATTERN_FILE_PATH)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion


        #region Forget Password Email Pattern

        private const string FORGET_PASSWORD_PATTERN_FILE_PATH = "~/Views/EmailPattern/_ForgetPassword.cshtml";

        public static string GetForgetPasswordEmailPattern()
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(FORGET_PASSWORD_PATTERN_FILE_PATH)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion


        #region Change User Account Email Pattern

        private const string CHANGE_USER_ACCOUNT_PATTERN_FILE_PATH = "~/Views/EmailPattern/_ChangeUserAccount.cshtml";

        public static string GetChangeUserAccountEmailPattern()
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(CHANGE_USER_ACCOUNT_PATTERN_FILE_PATH)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion


        #region CLose Ticket Email Pattern

        private const string CLOSE_TICKET_PATTERN_FILE_PATH = "~/Views/EmailPattern/_CloseTicket.cshtml";

        public static string GetCLoseTicketEmailPattern()
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(CLOSE_TICKET_PATTERN_FILE_PATH)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion


        #region CLose Ticket Email Pattern

        private const string SAVED_ORDER_PATTERN_FILE_PATH = "~/Views/EmailPattern/_SavedOrder.cshtml";

        public static string GetSavedOrderEmailPattern()
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(SAVED_ORDER_PATTERN_FILE_PATH)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region Order Information Email Pattern

        private const string ORDER_INFORMATION_PATTERN_FILE_PATH = "~/Views/EmailPattern/_OrderInformation.cshtml";

        public static string GetOrderInformationEmailPattern()
        {
            using (var reader = new StreamReader(HostingEnvironment.MapPath(ORDER_INFORMATION_PATTERN_FILE_PATH)))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion

    }
}