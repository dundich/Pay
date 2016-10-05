using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    /// <summary>
    /// Доктор {Id: 22, Value='Иванов Иван Иванович'}
    /// </summary>
    public class PayDoctor : PayIdValue
    {
        /// <summary>
        /// Список специальностей
        /// </summary>

        public List<PayIdValue> Specialities { get; set; }
        /// <summary>
        /// Стаж работы - 12 лет
        /// </summary>
        public int? JobAge { get; set; }
        /// <summary>
        /// Ранг, Степень (Высшая категория, Кандидат медицинских наук, Заместитель глав.врача)
        /// </summary>
        public List<string> Ranges { get; set; }
        /// <summary>
        /// описание - html
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// Список Клиник-организаций
        /// </summary>
        public List<PayLpuLocation> LpuList { get; set; }
        /// <summary>
        /// Рейтинг
        /// </summary>
        public int? Rating { get; set; }

        /// <summary>
        /// личина
        /// </summary>
        public string Avatar { get; set; }
    }



    /// <summary>
    /// описание пакета услуг
    /// </summary>
    public class PayLpuLocation : PayIdValue
    {
        /// <summary>
        /// {Id: 1, Value: 'Городская клиническая больница №'}
        /// </summary>
        public PayIdValue Lpu { get; set; }

        /// <summary>
        ///  { Value: Адрес: 111539, Москва, ул.Вешняковская, 23 }
        /// </summary>
        public PayIdValue Address { get; set; }

        /// <summary>
        /// уточнение; детализация 
        /// { Id:34, Value:'Проктолог', } (напр. специальность...)
        /// { Id:34, Value:'Детское отд', Id:35, Value:'Взросл. отд',} (напр. отд...)
        /// </summary>
        public PayIdValue Specification { get; set; }

        /// <summary>
        /// Список услуг для спецификации
        /// </summary>
        public List<PayServiceItem> Services { get; set; }

        /// <summary>
        /// Место оказания услуги
        /// {Id: 22, Value='каб.505 эт.2'}
        /// </summary>
        public List<PayRoom> Rooms { get; set; }

    }


    public class PayGetDoctorList : PayCriteria
    {
        public string SpecialityId { get; set; }
    }

    public class PayGetDoctor : PayGetDoctorList
    {
        public string DoctorId { get; set; }
    }



    public class PayCreateDoctorVisit : PayGetDoctor
    {
        public string PatientId { get; set; }

        public string SlotId { get; set; }
    }


    public class PayGetDoctorVisitDays : PayGetDoctor
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



    public class PayGetDoctorVisitSlots : PayGetDoctor
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string Date { get; set; }
    }


    public class PayGetDoctorVisits : PayGetDoctor
    {
        public string PatientId { get; set; }

        public string VisitId { get; set; }
    }


    public interface IPayDoctor : IPay
    {
        /// <summary>
        /// список специальностей
        /// </summary>
        Task<List<PayIdValue>> GetSpecialityList(PayCriteria criteria);

        /// <summary>
        /// список докторишек
        /// </summary>
        Task<List<PayDoctor>> GetDoctorList(PayGetDoctorList criteria);

        /// <summary>
        /// Выбрать доктора
        /// </summary>
        Task<PayDoctor> GetDoctor(PayGetDoctor criteria);

        /// <summary>
        /// Список дней с визитами
        /// </summary>
        Task<List<PayVisitDay>> GetDoctorVisitDays(PayGetDoctorVisitDays criteria);


        /// <summary>
        /// Список доступных времен
        /// </summary>
        Task<List<PaySlot>> GetDoctorVisitSlots(PayGetDoctorVisitSlots criteria);


        /// <summary>
        /// Создать визит
        /// </summary>
        Task<PayVisitResult> CreateDoctorVisit(PayCreateDoctorVisit criteria);


        Task<PayIdValue> DeleteDoctorVisit(PayCriteria criteria);


        Task<List<PayVisitInfo>> GetDoctorVisits(PayGetDoctorVisits criteria);
    }
}
