using System;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    class FakeIdent : IPayIdent
    {
        public Task<PayIdValue> Register(PayPatientReg patient)
        {

            if (patient == null) throw new ArgumentNullException("Пациент не задан!");

            return Task.Run(async () =>
            {
                await Task.Delay(2000);

                if (patient.Sex != "M") throw new ArgumentException("Только мужики! " + DateTime.Now.ToLongTimeString());

                return new PayIdValue
                {
                    Id = Guid.NewGuid().ToString(),
                    Value = string.Format("{0} {1} {2}", patient.LastName, patient.FirstName, patient.MiddleName)
                };
            });
        }
    }
}
