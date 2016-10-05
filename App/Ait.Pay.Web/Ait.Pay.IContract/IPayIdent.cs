using System.Threading.Tasks;

namespace Ait.Pay.IContract
{

    public class PayPatientReg : PayIdValue
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        /// <summary>
        /// yyy-MM-dd
        /// </summary>
        public string Birthdate { get; set; }

        public string Sex { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Kladr { get; set; }

        public string Address { get; set; }

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
    }


    public interface IPayIdent : IPay
    {
        Task<PayIdValue> Register(PayPatientReg patient);
    }
}