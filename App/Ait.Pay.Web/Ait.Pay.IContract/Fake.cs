using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
