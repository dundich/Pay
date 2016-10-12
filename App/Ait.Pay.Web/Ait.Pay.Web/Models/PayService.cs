using Ait.Pay.IContract;
using Maybe2;
using Maybe2.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.Web.Models
{
    /// <summary>
    /// Proxy to remote service
    /// </summary>
    public class PayService : IPayContract
    {
        readonly IShellSettings cfg;

        public PayService(IShellSettings cfg)
        {
            this.cfg = cfg;
        }

        /// <summary>
        /// IDOCTOR
        /// </summary>
        public async Task<List<PayIdValue>> GetSpecialityList(PayCriteria criteria)
        {
            return await GetPay<List<PayIdValue>, PayCriteria>("GetSpecialityList", criteria);
        }

        public async Task<List<PayDoctor>> GetDoctorList(PayGetDoctorList criteria)
        {
            return await GetPay<List<PayDoctor>, PayGetDoctorList>("GetDoctorList", criteria);
        }

        public async Task<List<PayVisitDay>> GetDoctorVisitDays(PayGetDoctorVisitDays criteria)
        {
            return await GetPay<List<PayVisitDay>, PayGetDoctorList>("GetDoctorVisitDays", criteria);
        }

        public async Task<List<PaySlot>> GetDoctorVisitSlots(PayGetDoctorVisitSlots criteria)
        {
            return await GetPay<List<PaySlot>, PayGetDoctorVisitSlots>("GetDoctorVisitSlots", criteria);
        }


        //************
        //  IDENT
        //***********
        public async Task<PayIdValue> Register(PayPatientReg patient)
        {
            return await GetPay<PayPatientReg, PayIdValue>("Register", patient);
        }


        //****************
        //  IDENT
        //***********
        public async Task<List<PayServiceItem>> GetResearchList(PayGetResearchList criteria)
        {
            return await GetPay<List<PayServiceItem>, PayCriteria>("GetResearchList", criteria);
        }

        public async Task<PayResearchLocation> GetResearchLocation(PayGetResearchLocation criteria)
        {
            return await GetPay<PayResearchLocation, PayGetResearchLocation>("GetResearchLocation", criteria);
        }

        public async Task<List<PayVisitDay>> GetResearchVisitDays(PayGetResearchVisitDays criteria)
        {
            return await GetPay<List<PayVisitDay>, PayGetResearchVisitDays>("GetResearchVisitDays", criteria);
        }

        public async Task<List<PaySlot>> GetResearchVisitSlots(PayGetResearchVisitSlots criteria)
        {
            return await GetPay<List<PaySlot>, PayGetResearchVisitSlots>("GetResearchVisitSlots", criteria);
        }


        //****************
        //  IVISIT
        //***********

        public async Task<PayVisitResult> CreateDoctorVisit(PayCreateDoctorVisit criteria)
        {
            return await GetPay<PayVisitResult, PayCreateDoctorVisit>("CreateDoctorVisit", criteria);
        }

        public async Task<PayVisitResult> CreateResearchVisit(PayCreateResearchVisit criteria)
        {
            return await GetPay<PayVisitResult, PayCreateResearchVisit>("CreateResearchVisit", criteria);
        }

        public async Task<PayIdValue> DeleteVisit(PayCriteria criteria)
        {
            return await GetPay<PayIdValue, PayCriteria>("DeleteVisit", criteria);
        }

        public async Task<List<PayVisit>> GetVisits(PayGetVisits criteria)
        {
            return await GetPay<List<PayVisit>, PayCriteria>("GetPayVisit", criteria);
        }



        //---------------
        private async Task<R> GetPay<R, T>(string url, T criteria)
        {
            var setts = cfg.GetSettings();
            url = setts.GetOrDefault(consts.KEY_URL_PAY).EnsureTrailingSlash() + url;
            return await url.PostAsJson<R, T>(criteria);
        }
    }
}