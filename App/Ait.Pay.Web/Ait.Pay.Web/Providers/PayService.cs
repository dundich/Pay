using Ait.Pay.IContract;
using Maybe2;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;

namespace Ait.Pay.Web.Providers
{
    public class PayService : IPayDoctor, IPayIdent, IPayResearch
    {

        readonly IPayConfig cfg;


        public PayService(IPayConfig cfg)
        {
            this.cfg = cfg;
        }

        /// <summary>
        /// IDOCTOR
        /// </summary>
        public async Task<PayOrderResult> CreateDoctorVisit(PayCreateDoctorVisit criteria)
        {
            return await cfg.GetPayDoctor<PayOrderResult, PayCriteria>("CreateDoctorVisit", criteria);
        }

        public async Task<PayIdValue> DeleteDoctorVisit(PayCriteria criteria)
        {
            return await cfg.GetPayDoctor<PayIdValue, PayCriteria>("DeleteDoctorVisit", criteria);
        }

        public async Task<PayDoctor> GetDoctor(PayGetDoctor criteria)
        {
            return await cfg.GetPayDoctor<PayDoctor, PayCriteria>("GetDoctor", criteria);
        }

        public async Task<List<PayDoctor>> GetDoctorList(PayGetDoctorList criteria)
        {
            return await cfg.GetPayDoctor<List<PayDoctor>, PayGetDoctorList>("GetDoctorList", criteria);
        }

        public async Task<List<PayVisitDay>> GetDoctorVisitDays(PayGetDoctorVisitDays criteria)
        {
            return await cfg.GetPayDoctor<List<PayVisitDay>, PayGetDoctor>("GetDoctorVisitDays", criteria);
        }

        public async Task<List<PaySlot>> GetDoctorVisitSlots(PayGetDoctorVisitSlots criteria)
        {
            return await cfg.GetPayDoctor<List<PaySlot>, PayGetDoctorVisitSlots>("GetDoctorVisitSlots", criteria);
        }

        public async Task<List<PayIdValue>> GetPatientDoctorVisits(PayCriteria criteria)
        {
            return await cfg.GetPayDoctor<List<PayIdValue>, PayCriteria>("GetPatientDoctorVisits", criteria);
        }

        public async Task<List<PayIdValue>> GetSpecialityList(PayCriteria criteria)
        {
            return await cfg.GetPayDoctor<List<PayIdValue>, PayCriteria>("GetSpecialityList", criteria);
        }

        //************
        //  IDENT
        //***********
        public async Task<PayIdValue> Register(PayPatientReg patient)
        {
            return await cfg.GetPayIdent<PayPatientReg, PayIdValue>("Register", patient);
        }


        //****************
        //  IDENT
        public async Task<List<IContract.PayServiceItem>> GetResearchList(PayCriteria criteria)
        {
            return await cfg.GetPayResearch<List<IContract.PayServiceItem>, PayCriteria>("GetResearchList", criteria);
        }

        public async Task<PayResearchLocation> GetResearchLocation(PayCriteria criteria)
        {
            return await cfg.GetPayResearch<PayResearchLocation, PayCriteria>("GetResearchLocation", criteria);
        }

        public async Task<List<PayIdValue>> GetResearchVisitDays(PayCriteria criteria)
        {
            return await cfg.GetPayResearch<List<PayIdValue>, PayCriteria>("GetResearchVisitDays", criteria);
        }

        public async Task<List<PayIdValue>> GetResearchVisitTimes(PayCriteria criteria)
        {
            return await cfg.GetPayResearch<List<PayIdValue>, PayCriteria>("GetResearchVisitTimes", criteria);
        }
    }


    static class PayClientHelper
    {

        internal static async Task<R> GetPayDoctor<R, T>(this IPayConfig cfg, string url, T criteria)
        {
            return await GetPay<R, T>(cfg, url, criteria);
        }

        internal static async Task<R> GetPayIdent<R, T>(this IPayConfig cfg, string url, T criteria)
        {
            return await GetPay<R, T>(cfg, url, criteria);
        }


        internal static async Task<R> GetPayResearch<R, T>(this IPayConfig cfg, string url, T criteria)
        {
            return await GetPay<R, T>(cfg, url, criteria);
        }


        static async Task<R> GetPay<R, T>(this IPayConfig cfg, string url, T criteria)
        {
            var setts = await cfg.GetSettings();

            url = setts.GetOrDefault(consts.KEY_URL_PAY).EnsureTrailingSlash() + url;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsJsonAsync(url, criteria);
                response.EnsureSuccessStatusCode();
                //var s = await response.Content.ReadAsStringAsync();
                return await response.Content.ReadAsAsync<R>();
            }
        }
    }

}