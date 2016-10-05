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
            builder.Register(b => PayHelper.GetFakePayDoctorService()).SingleInstance();
            builder.Register(b => PayHelper.GetFakePayResearchService()).SingleInstance();
            builder.Register(b => PayHelper.GetFakePayIdentService()).SingleInstance();
        }

        private static void LoadService(ContainerBuilder builder)
        {
            builder.RegisterType<PayService>().As<IPayResearch, IPayDoctor, IPayIdent>().SingleInstance();
        }

    }
}