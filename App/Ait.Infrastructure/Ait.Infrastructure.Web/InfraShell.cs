using Maybe2;
using Maybe2.Classes;
using Maybe2.Configuration;
using Postal;
using System.Net;
using System.Net.Mail;
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
                        UserName = this.Config[InfraConsts.SmtpUserName_KEY] ?? "",
                        Password = this.Config[InfraConsts.SmtpPassword_KEY] ?? ""
                    };


                    SmtpDeliveryMethod deliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    switch ((this.Config[InfraConsts.SmtpDeliveryMethod_KEY] ?? "").ToLower())
                    {
                        case "pickupdirectoryfromiis":
                            deliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                            break;
                        case "specifiedpickupdirectory":
                            deliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                            break;
                    }

                    return new System.Net.Mail.SmtpClient()
                    {
                        UseDefaultCredentials = this.Config[InfraConsts.SmtpUseDefaultCredentials_KEY].StrToBool(),
                        Credentials = credential,
                        Host = this.Config[InfraConsts.SmtpHost_KEY],
                        Port = this.Config[InfraConsts.SmtpPort_KEY].StrToInt(),
                        EnableSsl = this.Config[InfraConsts.SmtpEnableSsl_KEY].StrToBool(),
                        DeliveryMethod = deliveryMethod
                    };
                }));

            Email.CreateEmailService = () => emailService.Value;
        }

        public IEmailService EmailService => emailService.Value;
    }
}