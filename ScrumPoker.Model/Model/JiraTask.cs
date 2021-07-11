using ScrumPoker.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Model.Model
{
    public class JiraTask
    {
        [Display(Name="Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Average")]
        public decimal Average { get; set; }

        [Display(Name = "Comfirmed Point")]
        public CardPoints ComfirmedPoint { get; set; }

        public JiraTaskStatus Status { get; set; }
    }
}