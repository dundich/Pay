namespace Ait.Pay.IContract
{
    public class Fake
    {
        public static IPayVisit Visit => new FakeVisit();
        public static IPayResearch Research => new FakeResearch();
        public static IPayDoctor Doctor => new FakeDoctor();
        public static IPayIdent Ident => new FakeIdent();

    }
}
