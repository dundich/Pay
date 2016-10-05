using Ait.Pay.IContract;
using Ait.Pay.Web.Providers;
using Autofac;
using Maybe2.Configuration;

namespace Ait.Pay.Web
{
    public class PayModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IShellSettings>(b => ShellFileSettings.CreateWebShellSettings()).SingleInstance();
            builder.RegisterType<PayConfig>().As<IPayConfig>().SingleInstance();

            LoadDemo(builder);
            //LoadService(builder);
        }

        private static void LoadDemo(ContainerBuilder builder)
        {
            builder.Register(b => Fake.Doctor).SingleInstance();
            builder.Register(b => Fake.Ident).SingleInstance();
            builder.Register(b => Fake.Research).SingleInstance();
            builder.Register(b => Fake.Visit).SingleInstance();
        }

        private static void LoadService(ContainerBuilder builder)
        {
            builder
                .RegisterType<PayService>()
                .As<IPayResearch>()
                .As<IPayDoctor>()
                .As<IPayIdent>()
                .As<IPayVisit>()
                .SingleInstance();
        }

    }
}