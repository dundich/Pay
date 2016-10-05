using Ait.Pay.IContract;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ait.Pay.Web.Controllers
{
    public class PayDoctorController : ApiController
    {

        readonly IPayDoctor payService;

        public PayDoctorController(IPayDoctor payService)
        {
            this.payService = payService;
        }


        [HttpPost]
        public async Task<object> CreateVisit(PayCreateDoctorVisit criteria)
        {
            return (await payService.CreateDoctorVisit(criteria));
        }


        [HttpPost]
        public async Task<object> GetSpecialityList()
        {
            return (await payService.GetSpecialityList(new PayCriteria { }))
                .OrderBy(c => c.Value);
        }


        [HttpPost]
        public async Task<object> GetDoctorList(PayGetDoctorList criteria)
        {
            return (await payService.GetDoctorList(criteria)).OrderBy(c => c.Value);
        }



        [HttpPost]
        public async Task<object> GetDoctor(PayGetDoctor criteria)
        {
            return await payService.GetDoctor(criteria);
        }


        [HttpPost]
        public async Task<object> GetVisitDays(PayGetDoctorVisitDays criteria)
        {
            return await payService.GetDoctorVisitDays(criteria);
        }


        [HttpPost]
        public async Task<object> GetVisitSlots(PayGetDoctorVisitSlots criteria)
        {
            return (await payService.GetDoctorVisitSlots(criteria)).OrderBy(c => c.Value);
        }
    }
}