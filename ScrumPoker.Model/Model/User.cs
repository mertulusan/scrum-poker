using ScrumPoker.Model.Enums;
using System;
using System.ComponentModel;

namespace ScrumPoker.Model.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public int Point { get; set; }
        public bool IsActive { get; set; }
    }
}
