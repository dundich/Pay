using Ait.Pay.IContract;
using Maybe2.Classes;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ait.Pay.Web.Controllers
{
    public class PayResearchController : ApiController
    {

        readonly IPayResearch payService;

        public PayResearchController(IPayResearch payService)
        {
            this.payService = payService;
        }


        [HttpPost]
        public async Task<object> GetResearchList(PayGetResearchList criteria)
        {
            var list = await payService.GetResearchList(criteria);


            var nodes = list.Select(c => new ForestLink<PayServiceItem> { Id = c.Id, Data = c, ParentId = c.ParentId ?? "" });

            var forest = nodes.ConvertToForest();


            return forest.RootNodes.Select(c => new
            {
                Data = c.Data,
                Level = c.Level,
                Children = c.Children.OrderBy(c1 => c1.Data.Value).Select(ch => new
                {
                    Data = ch.Data,
                    Level = ch.Level,
                    Children = ch.Children.Where(l => !l.Data.IsFolder).ConvertToFlatArray(l => l.OrderBy(li => li.Data.Value)).Select(cch => new
                    {
                        Data = cch.Data,
                        Level = cch.Level
                    })
                })
            });
        }


        [HttpPost]
        public async Task<object> GetResearchLocation(PayGetResearchLocation criteria)
        {
            return await payService.GetResearchLocation(criteria);
        }


        [HttpPost]
        public async Task<object> GetVisitDays(PayGetResearchVisitDays criteria)
        {
            return await payService.GetResearchVisitDays(criteria);
        }


        [HttpPost]
        public async Task<object> GetVisitSlots(PayGetDoctorVisitSlots criteria)
        {
            return (await payService.GetResearchVisitSlots(criteria)).OrderBy(c => c.Value);
        }

    }
}