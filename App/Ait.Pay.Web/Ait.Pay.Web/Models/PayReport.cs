using Ait.Pay.IContract;
using Maybe2;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Ait.Pay.Web.Models
{
    public class PayReport : IPayReport, IPayVisitReport
    {

        readonly IPayConfig cfg;

        public PayReport(IPayConfig cfg)
        {
            this.cfg = cfg;
        }


        public async Task<ReportResponse> GetInviteVisitReport(PayVisit visit)
        {
            //HARD CODE!
            var rep = await report2(new ReportRequest
            {
                repId = visit.Kind == 0 ? "ПриглНаКонс" : "ПриглНаИссл",
                data = new
                {
                    пацФИО = visit.Patient.Value,
                    исслНазвание = visit.Service.NoNull(c => c.Value),
                    врачФИО = visit.Doctor.NoNull(c => c.Value),
                    врачСпециальность = visit.Speciality.NoNull(c => c.Value),
                    врачФИОСпециальность = visit.Doctor.NoNull(c => c.Value) + " → " + visit.Speciality.NoNull(c => c.Value),
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
                lpuId = visit.Lpu.NoNull(c => c.Id),
                viewFmt = "pdf"
            });

            return rep;
        }


        //****************
        //  IREPORT
        //***********

        public async Task<ReportResponse> report2(ReportRequest criteria)
        {
            var setts = await cfg.GetSettings();
            string key = consts.KEY_URL_REP.Trim();
            var url = setts.GetOrDefault(key).EnsureTrailingSlash() + "report2";
            var rep = await url.PostAsJson<ReportResponse, ReportRequest>(criteria);
            //hard code
            rep.view = rep.baseUrl.ReplaceLoopbackToProxyHost(url).EnsureTrailingSlash() + rep.view;
            return rep;
        }
    }
}