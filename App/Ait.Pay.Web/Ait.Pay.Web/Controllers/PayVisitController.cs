﻿using Ait.Pay.IContract;
using Ait.Pay.Web.Models;
using Maybe2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ait.Pay.Web.Controllers
{
    public class PayVisitController : ApiController
    {
        readonly IPayVisit payVisit;
        readonly IPayVisitReport report;

        public PayVisitController(IPayVisit payVisit, IPayVisitReport report)
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


            var rep = await report.GetInviteVisitReport(visit);
            ////HARD CODE!
            //rep.view = rep.baseUrl.ReplaceLoopbackToProxyHost(Request.RequestUri.Host).EnsureTrailingSlash() + rep.view;

            return new
            {
                visit = visit,
                report = rep
            };
        }
    }
}