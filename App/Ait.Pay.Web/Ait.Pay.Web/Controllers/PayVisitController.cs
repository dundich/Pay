using Ait.Pay.IContract;
using Ait.Pay.Web.Models;
using Maybe2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ait.Pay.Web.Controllers
{
    public class PayVisitController : ApiController
    {
        readonly IPayVisit payVisit;
        readonly IPayReport report;

        public PayVisitController(IPayVisit payVisit, IPayReport report)
        {
            this.payVisit = payVisit;
            this.report = report;
        }


        [HttpPost]
        public async Task<PayVisitResult> CreateDoctorVisit(PayCreateDoctorVisit criteria)
        {
            return (await payVisit.CreateDoctorVisit(criteria));
        }

        [HttpPost]
        public async Task<object> CreateResearchVisit(PayCreateResearchVisit criteria)
        {
            return await payVisit.CreateResearchVisit(criteria);
        }

        [HttpPost]
        public Task<PayIdValue> DeleteVisit(PayCriteria criteria)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<List<PayVisit>> GetVisits(PayGetVisits criteria)
        {
            return await payVisit.GetVisits(criteria);
        }


        public class getReport
        {
            public string VisitId { get; set; }
        }

        [HttpPost]
        public async Task<object> GetReport(getReport criteria)
        {
            if (criteria == null) throw new ArgumentNullException("GetReport");
            if (criteria.VisitId == null) throw new ArgumentNullException("VisitId");

            var visit = (
                await GetVisits(new PayGetVisits
                {
                    VisitId = criteria.VisitId
                }))
                .FirstOrDefault();


            if (visit == null)
                throw new Exception("Визит не найден!");

            //HARD CODE!
            var rep = await report.report2(new ReportRequest
            {
                repId = "ПриглНаКонс",
                data = new
                {
                    пацФИО = visit.Patient.Value,
                    исслНазвание = visit.Service.NoNull(c => c.Value),
                    врачФИО = visit.Doctor.NoNull(c => c.Value),
                    врачСпециальность = visit.Speciality.NoNull(c => c.Value),
                    врачФИОСпециальность = visit.Doctor.NoNull(c => c.Value) + " " + visit.Speciality.NoNull(c => c.Value),
                    визитДеньНеделиДата = visit.VisitDate.NoNull(c => c.StrToDate().ToString("ddd, dd.MM.yyyy", new CultureInfo("ru-RU", false))),
                    местоПриема = visit.Room,
                    визитВремя = visit.VisitTime,
                    медкартаНомер = visit.PatientCard,
                    пацДатаРождения = visit.PatientBirthdate.NoNull(c => c.StrToDate().ToString("dd.MM.yyyy")),
                    визитСоздан = visit.CreatedAt.NoNull(c => c.StrToDate().ToString("dd.MM.yyyy HH:mm")),
                    лпуАдрес = visit.LpuAddress,
                    лпуНаименование = visit.NoNull(c => c.Value),
                    лпуКонтакты = visit.Contacts,
                    _распечатанДатаВремя = DateTime.Now.ToString("dd.MM.yyyy"),
                    визитПамятка = visit.Note
                }
                .AsJson(),
                //lpuId = "GKB15",
                viewFmt = "pdf"
            });

            rep.view = rep.baseUrl.ReplaceLoopbackToProxyHost(Request.RequestUri.Host).EnsureTrailingSlash() + rep.view;

            return new
            {
                visit = visit,
                report = rep
            };
        }
    }
}