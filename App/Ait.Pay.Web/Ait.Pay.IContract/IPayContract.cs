using System.Collections.Generic;

namespace Ait.Pay.IContract
{

    public interface IPay { }

    /// <summary>
    /// [HttpPost]
    /// </summary>
    public interface IPayContract : IPayDoctor, IPayIdent, IPayResearch, IPayVisit, IPay
    {


    }


    ///// <summary>
    ///// На всяк случай...
    ///// </summary>
    //public class ExtProps : Dictionary<string, string> { }


    public class PayIdValue
    {
        public string Id { get; set; }

        public string Value { get; set; }


        //public ExtProps _extProps { get; set; }


        public PayIdValue()
        {
            //_extProps = null;
        }
    }


    public class PayTreeItem : PayIdValue
    {
        public string ParentId { get; set; }
        public bool IsFolder { get; set; }
    }


    /// <summary>
    /// {Id:2, Value:'Художественная реставрация зубов', 'Price': 'от 2 500 - 3 000'}
    /// </summary>
    public class PayServiceItem : PayTreeItem
    {
        /// <summary>
        /// Цена услуги
        /// </summary>
        public string Price { get; set; }
    }

    /// <summary>
    /// место оказания услуги
    ///  {Id: 22, Value='каб.505 эт.2'}
    ///  
    /// График работы центра:
    /// пн-пт: 08:00-21:00
    /// сб: 08:00-21:00
    /// вс: 08:00-21:00
    /// </summary>
    public class PayRoom : PayIdValue
    {
        /// <summary>
        /// {Id = "Пн, Вт, Ср", Value="08:00-12:48"}
        /// </summary>
        public List<PayIdValue> Times { get; set; }
    }

    /// <summary>
    /// Value="2012-12-03"
    /// </summary>
    public class PayVisitDay : PayIdValue
    {
        /// <summary>
        /// "08:00-12:48"
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// свободных мест
        /// </summary>
        public int? TicketCount { get; set; }
    }



    public class PayCriteria
    {
        /// <summary>
        /// Ключ ЛПУ
        /// </summary>
        public string LpuId { get; set; }
    }

    /// <summary>
    /// Место в расписании
    /// Value="12:30"
    /// </summary>
    public class PaySlot : PayIdValue
    {
        /// <summary>
        /// где приём
        /// </summary>
        public string Room { get; set; }

        /// <summary>
        /// Дата визита (yyyy-MM-dd)
        /// </summary>
        public string DateTime { get; set; }


        /// <summary>
        /// кол-во свободных мест
        /// </summary>
        public int? TicketCount { get; set; }        
    }
}