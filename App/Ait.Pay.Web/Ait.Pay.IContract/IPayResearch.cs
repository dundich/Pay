using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{


    public class PayResearchLocation : PayIdValue
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

    /// <summary>
    /// список услуг
    /// </summary>
    public class PayGetResearchList : PayCriteria
    {
        /// <summary>
        /// код исследования
        /// </summary>
        public string ResearchId { get; set; }
    }


    /// <summary>
    /// место оказания услуги (исследования)
    /// </summary>
    public class PayGetResearchLocation : PayGetResearchList
    {
    }


    /// <summary>
    /// получить доступные дни
    /// </summary>
    public class PayGetResearchVisitDays : PayGetResearchLocation
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string DateBeg { get; set; }
        /// <summary>
        /// yyyy-MM-dd (включительно)
        /// </summary>
        public string DateEnd { get; set; }
    }


    public class PayGetResearchVisitSlots : PayGetResearchLocation
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// NULL -> all
        /// </summary>
        public string SlotId { get; set; }
    }




    public interface IPayResearch : IPay
    {
        /// <summary>
        /// список исследований
        /// </summary>        
        Task<List<PayServiceItem>> GetResearchList(PayGetResearchList criteria);

        /// <summary>
        /// Выбрать исследование
        /// </summary>
        Task<PayResearchLocation> GetResearchLocation(PayGetResearchLocation criteria);

        /// <summary>
        /// Список дней с визитами
        /// </summary>
        Task<List<PayVisitDay>> GetResearchVisitDays(PayGetResearchVisitDays criteria);

        /// <summary>
        /// Список доступных времен
        /// </summary>
        Task<List<PaySlot>> GetResearchVisitSlots(PayGetResearchVisitSlots criteria);
    }


}
