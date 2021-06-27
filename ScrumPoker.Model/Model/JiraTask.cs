using ScrumPoker.Model.Enums;
using System;

namespace ScrumPoker.Model.Model
{
    public class JiraTask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Average { get; set; }
        public JiraTaskStatus Status { get; set; }
    }
}