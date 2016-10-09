using Ait.Pay.IContract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Pay.Web.Models
{
    /// <summary>
    /// get report viewer
    /// </summary>
    public interface IPayReport
    {
        Task<ReportResponse> report2(ReportRequest value);
    }



    /// <summary>
    /// get Invite Report
    /// </summary>
    public interface IPayVisitReport
    {
        Task<ReportResponse> GetInviteVisitReport(PayVisit value);
    }



    /// <summary>
    /// Построенный отчет
    /// </summary>
    public class ReportResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string baseUrl { get; set; }

        /// <summary>
        /// report cache - id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// ошибка 
        /// </summary>
        public string error { get; set; }

        /// <summary>
        /// uri default отчета Viewer
        /// </summary>
        public string view { get; set; }

        /// <summary>
        /// report size
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// viewers (default, Mozila)
        /// </summary>
        public Dictionary<string, string> viewers { get; set; }
    }

    public class ReportRequest
    {
        /// <summary>
        /// input Data (json, text, rtf, xml ...)
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// ЛПУ код (для специфики отчета)
        /// </summary>
        public string lpuId { get; set; }
        /// <summary>
        /// see ReportIds
        /// </summary>
        public string repId { get; set; }

        /// <summary>
        /// default format view: default, Mozilla, pdf, devexp_html5
        /// </summary>
        public string viewFmt { get; set; }
    }
}