using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{


    public class PayResearchLocation: PayIdValue
    {

        public PayIdValue Lpu { get; set; }
        /// <summary>
        ///  { Value: Адрес: 111539, Москва, ул.Вешняковская, 23 }
        /// </summary>
        public PayIdValue Address { get; set; }


        public List<PayRoom> Rooms { get; set; }


        public PayServiceItem Service { get; set; }


        /// <summary>
        /// описание - html
        /// </summary>
        public string About { get; set; }


        /// <summary>
        /// личина
        /// </summary>
        public string Avatar { get; set; }

    }



    public interface IPayResearch : IPay
    {
        /// <summary>
        /// список исследований
        /// </summary>        
        Task<List<PayServiceItem>> GetResearchList(PayCriteria criteria);

        /// <summary>
        /// Выбрать исследование
        /// </summary>
        Task<PayResearchLocation> GetResearchLocation(PayCriteria criteria);

        /// <summary>
        /// Список дней с визитами
        /// </summary>
        Task<List<PayIdValue>> GetResearchVisitDays(PayCriteria criteria);


        /// <summary>
        /// Список доступных времен
        /// </summary>
        Task<List<PayIdValue>> GetResearchVisitTimes(PayCriteria criteria);

    }


}
