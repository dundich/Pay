using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{

    public class PayVisit : PayIdValue
    {
        public PayIdValue Lpu { get; set; }

        public PayIdValue Patient { get; set; }

        public PayServiceItem Service { get; set; }

        public PayIdValue Doctor { get; set; }

        public PayIdValue Speciality { get; set; }


        public string VisitDate { get; set; }

        public string VisitTime { get; set; }


        public string PatientCard { get; set; }

        public string PatientBirthdate { get; set; }

        public string Room { get; set; }

        public string LpuAddress { get; set; }

        public string Note { get; set; }

        public string CreatedAt { get; set; }
        /// <summary>
        /// 0 - doctor vidit
        /// 1 - research
        /// </summary>
        public int Kind { get; set; }

    }


    /// <summary>
    /// Получить визиты
    /// </summary>
    public class PayGetVisits : PayCriteria
    {
        public string PatientId { get; set; }

        public string VisitId { get; set; }

        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string DateBeg { get; set; }
        /// <summary>
        /// yyyy-MM-dd (включительно)
        /// </summary>
        public string DateEnd { get; set; }

        /// <summary>
        /// Вид визита
        /// </summary>
        public int? Kind { get; set; }
    }

    /// <summary>
    /// Работа с визитами
    /// </summary>
    public interface IPayVisit
    {

        /// <summary>
        /// Удалить
        /// </summary>
        Task<PayIdValue> DeleteVisit(PayCriteria criteria);

        /// <summary>
        /// Список визитов
        /// </summary>
        Task<List<PayVisit>> GetVisits(PayGetVisits criteria);

    }
}
