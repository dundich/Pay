using Maybe2.Configuration;

namespace Ait.Infrastructure.Web
{
    public class InfraConsts : ShellConsts
    {
        public const string SmtpUserName_KEY = "SmtpUserName";
        public const string SmtpPassword_KEY = "SmtpPassword";
        public const string SmtpHost_KEY = "SmtpHost";
        public const string SmtpPort_KEY = "SmtpPort";
        public const string SmtpEnableSsl_KEY = "SmtpEnableSsl";
        public const string SmtpUseDefaultCredentials_KEY = "SmtpUseDefaultCredentials";
        public const string SmtpDeliveryMethod_KEY = "SmtpDeliveryMethod";
    }
}