using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    class FakeVisit : IPayVisit
    {
        public Task<PayIdValue> DeleteVisit(PayCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Task<List<PayVisit>> GetVisits(PayGetVisits criteria)
        {
            return Task.FromResult(new List<PayVisit>
            {
                new PayVisit
                {
                    Id = "23",
                    Doctor = new PayIdValue { Id = "1", Value = "Бугаева Антон Васильевич" },
                    Speciality = new PayIdValue{Id = "1", Value = "Акушер" },
                    Room = "каб 200",
                    Lpu = "1".PayValue("ЛПУ"),
                    Service = new PayServiceItem { Id = "1", Value = "приём в кабинете", Price = "300ye" },
                    VisitDate = DateTime.Today.ToString("yyyy-MM-dd"),
                    VisitTime = "12:00",
                    CreatedAt = "2016-04-02",
                    PatientCard = "123345",
                    Patient = "1".PayValue("Иванов Иван Иванович")
                }
            });
        }
    }
}
