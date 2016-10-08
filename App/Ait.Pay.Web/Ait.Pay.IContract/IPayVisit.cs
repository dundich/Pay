using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.IContract
{
    /// <summary>
    /// Визит (приём | исследование)
    /// </summary>
    public class PayVisit : PayIdValue
    {
        /// <summary>
        /// Учреждение - Value=наименование
        /// </summary>
        public PayIdValue Lpu { get; set; }
        /// <summary>
        /// Пациент Value=FIO
        /// </summary>
        public PayIdValue Patient { get; set; }
        /// <summary>
        /// Услуга (исследование или прием врача)
        /// </summary>
        public PayServiceItem Service { get; set; }
        /// <summary>
        /// Врач если прием
        /// </summary>
        public PayIdValue Doctor { get; set; }
        /// <summary>
        /// Специальность
        /// </summary>
        public PayIdValue Speciality { get; set; }
        /// <summary>
        /// Дата визита
        /// </summary>
        public string VisitDate { get; set; }
        /// <summary>
        /// Время визита
        /// </summary>
        public string VisitTime { get; set; }
        /// <summary>
        /// Мед. карта
        /// </summary>
        public string PatientCard { get; set; }
        /// <summary>
        /// День рождения
        /// </summary>
        public string PatientBirthdate { get; set; }
        /// <summary>
        /// Место приема- кабинет
        /// </summary>
        public string Room { get; set; }
        /// <summary>
        /// адрес учреждения
        /// </summary>
        public string LpuAddress { get; set; }
        /// <summary>
        /// Контакты ЛПУ!
        /// </summary>
        public string Contacts { get; set; }
        /// <summary>
        /// помятка пациенту
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Визит создан
        /// </summary>
        public string CreatedAt { get; set; }
        /// <summary>
        /// 0 - doctor 
        /// 1 - research
        /// </summary>
        public int Kind { get; set; }
    }


    /// <summary>
    /// Получить визиты
    /// </summary>
    public class PayGetVisits : PayCriteria
    {
        /// <summary>
        /// Пациент
        /// </summary>
        public string PatientId { get; set; }
        /// <summary>
        /// Id визита (Null-все визиты)
        /// </summary>
        public string VisitId { get; set; }
        /// <summary>
        /// начало периода yyyy-MM-dd (NULL->TODAY)
        /// </summary>
        public string DateBeg { get; set; }
        /// <summary>
        /// конец периода yyyy-MM-dd (включительно) NULL->maxDate
        /// </summary>
        public string DateEnd { get; set; }
        /// <summary>
        /// Вид визита
        /// </summary>
        public int? Kind { get; set; }
    }



    public class PayVisitResult : PayIdValue
    {
        /// <summary>
        /// -2 (место занято)
        /// -13 (доступ закрыт)
        /// -10101 (Мин. срок между назначениями по одной специальности - {0} дн!)
        /// -10102 (Мин. срок между двумя назначениями пациента - {0} мин.!)
        /// -666 CRASH
        /// </summary>
        public int? ErrorCode { get; set; }
    }

    /// <summary>
    /// Запрос создать визит к врачу
    /// </summary>
    public class PayCreateDoctorVisit : PayIdValue
    {
        /// <summary>
        /// Пациент
        /// </summary>
        public string PatientId { get; set; }
        /// <summary>
        /// место в расписании
        /// </summary>
        public string SlotId { get; set; }
        /// <summary>
        /// Доктор
        /// </summary>
        public string DoctorId { get; set; }
        /// <summary>
        /// Специальность
        /// </summary>
        public string SpecialityId { get; set; }
    }

    /// <summary>
    /// Запрос создать визит на исследование
    /// </summary>
    public class PayCreateResearchVisit : PayIdValue
    {
        /// <summary>
        /// Пациент
        /// </summary>
        public string PatientId { get; set; }
        /// <summary>
        /// место в расписании
        /// </summary>
        public string SlotId { get; set; }
        /// <summary>
        /// id исследования
        /// </summary>
        public string ResearchId { get; set; }
    }

    /// <summary>
    /// Работа с визитами
    /// </summary>
    public interface IPayVisit
    {

        /// <summary>
        /// Создать визит к врачу
        /// </summary>
        Task<PayVisitResult> CreateDoctorVisit(PayCreateDoctorVisit criteria);

        /// <summary>
        /// Создать визит на исследование
        /// </summary>
        Task<PayVisitResult> CreateResearchVisit(PayCreateResearchVisit criteria);

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
