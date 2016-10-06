using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    class FakeVisit : IPayVisit
    {
        public Task<PayVisitResult> CreateDoctorVisit(PayCreateDoctorVisit criteria)
        {
            return Task.FromResult(new PayVisitResult { Id = "23", Value = "ОК!" });
        }


        public Task<PayIdValue> DeleteVisit(PayCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Task<List<PayVisit>> GetVisits(PayGetVisits criteria)
        {

            var items = new[]
            {
                new PayVisit
                {
                    Id = "23",
                    Doctor = new PayIdValue { Id = "1", Value = "Бугаев Антон Васильевич" },
                    Speciality = new PayIdValue { Id = "1", Value = "Акушер" },
                    Room = "каб.200 эт.2",
                    Lpu = "1".PayValue("ЛПУ"),
                    Contacts = "+7(444) 123-45-67",
                    Service = new PayServiceItem { Id = "1", Value = "приём в кабинете", Price = "300ye" },
                    VisitDate = DateTime.Today.ToString("yyyy-MM-dd"),
                    VisitTime = "12:00",
                    PatientBirthdate = "2001-03-01",
                    CreatedAt = "2016-04-02 13:33",
                    PatientCard = "123345",
                    Note = "помятка",
                    Patient = "1".PayValue("Иванов Иван Иванович")
                }
            };


            return Task.FromResult(new List<PayVisit>(items));
        }
    }
}
