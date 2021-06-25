using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.UI.Model
{
    public class Room
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(37, ErrorMessage = "maximum 36.")]
        public string Name { get; set; }

        public DateTime EndDate { get; set; }

        public JiraTask Task { get; set; }

        public List<User> Users { get; set; }

        public List<JiraTask> TaskList { get; set; }
    }
}
