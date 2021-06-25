using System;

namespace ScrumPoker.UI.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int Point { get; set; }
        public bool IsActive { get; set; }
    }
}
