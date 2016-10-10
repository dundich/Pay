using System.ComponentModel.DataAnnotations;

namespace Ait.Infrastructure.Web.Models
{
    public class TodoItemViewModel
    {
        [Required(ErrorMessage = "The Task Field is Required.")]
        public string task { get; set; }
        public bool completed { get; set; }
    }
}