using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Model.Model
{
    public class Room
    {
        [Required]
        [StringLength(37, ErrorMessage = "maximum 36.")]
        public string Name { get; set; }

        public DateTime EndDate { get; set; }

        public JiraTask VotingTask { get; set; }

        public List<User> Users { get; set; }

        public List<JiraTask> VotedTaskList { get; set; }

        public string Winner { get; set; }

        public bool SessionIsEnd { get; set; }
    }
}
