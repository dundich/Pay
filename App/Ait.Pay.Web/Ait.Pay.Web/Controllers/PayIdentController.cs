using Ait.Pay.IContract;
using Maybe2;
using Maybe2.Reflection;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ait.Pay.Web.Controllers
{

    public class PayIdentController : ApiController
    {
        #region ident
        public class ident
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string MiddleName { get; set; }

            public string Birthday { get; set; }

            public string Phone { get; set; }

            public string Email { get; set; }

            public string KladrId { get; set; }

            public string Street { get; set; }

            public string House { get; set; }

            public string Build { get; set; }

            public string Room { get; set; }


            public string DocSer { get; set; }
            public string DocNum { get; set; }
            /// <summary>
            /// yyyy-MM-dd
            /// </summary>
            public string DocDate { get; set; }
            public string DocIssuer { get; set; }



            public PayIdValue Sex { get; set; }


            public PayPatientReg Convert()
            {
                var p = this.CopyProperties(new PayPatientReg { }, true);

                p.LastName = toName(p.LastName);
                p.FirstName = toName(p.FirstName);
                p.MiddleName = toName(p.MiddleName);

                p.Birthday = toYMD(Birthday);
                p.DocDate = toYMD(DocDate);

                p.Sex = Sex.NoNull(c => c.Id);

                return p;
            }

            static string toYMD(string p)
            {
                return p.StrToDateNullable().Value.NoNull(c => c.ToString("yyyy-MM-dd"));
            }

            static string toName(string p)
            {
                return p.PackToNull(255).NoNull(c => c.ToUpper());
            }

        }
        #endregion

        readonly IPayIdent service;

        public PayIdentController(IPayIdent service)
        {
            this.service = service;
        }


        [HttpPost]
        public async Task<object> Register(ident patient)
        {

            if (patient == null) throw new ArgumentNullException("Пациент не задан!");

            return await service.Register(patient.Convert());
        }

    }
}