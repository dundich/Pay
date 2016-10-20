using Maybe2;
using Maybe2.Classes;
using Maybe2.Configuration;
using Postal;
using System.Net;
using System.Web.Mvc;

namespace Ait.Infrastructure.Web
{
    public class InfraShell : Shell
    {

        readonly Maybe2.Classes.LazyCache<IEmailService> emailService = null;


        public InfraShell(string tenat = null) : base(tenat)
        {
            emailService = new LazyCache<IEmailService>(() =>
                new EmailService(ViewEngines.Engines, () =>
                {
                    var credential = new NetworkCredential
                    {
                        UserName = this.Config[InfraConsts.SmtpUserName_KEY],
                        Password = this.Config[InfraConsts.SmtpPassword_KEY]
                    };

                    return new System.Net.Mail.SmtpClient()
                    {

                        Credentials = credential,
                        Host = this.Config[InfraConsts.SmtpHost_KEY],
                        Port = this.Config[InfraConsts.SmtpPort_KEY].StrToInt(),
                        EnableSsl = this.Config[InfraConsts.SmtpEnableSsl_KEY].StrToBool(),
                        UseDefaultCredentials = false,
                        DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network
                    };
                }));

            Email.CreateEmailService = () => emailService.Value;
        }

        public IEmailService EmailService => emailService.Value;
    }
}