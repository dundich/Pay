using Ait.Pay.IContract;
using Ait.Pay.Web.Models;
using Autofac;
using Maybe2.Configuration;

namespace Ait.Pay.Web
{
    public class PayModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IShellSettings>(b => new ShellSettings(SettingsProvider.CreateWebSettings())).SingleInstance();            

#if (DEBUG && DEMO)
            builder.Register(b => Fake.Doctor).SingleInstance();
            builder.Register(b => Fake.Ident).SingleInstance();
            builder.Register(b => Fake.Research).SingleInstance();
            builder.Register(b => Fake.Visit).SingleInstance();
#else
            builder
                .RegisterType<PayService>()
                .As<IPayResearch>()
                .As<IPayDoctor>()
                .As<IPayIdent>()
                .As<IPayVisit>()
                .SingleInstance();
#endif


            builder
                .RegisterType<PayReport>()
                .As<IPayReport, IPayVisitReport>()
                .SingleInstance();
        }

    }
}