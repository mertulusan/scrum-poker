using System;
using System.ComponentModel;

namespace ScrumPoker.UI.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public int Point { get; set; }
        public bool IsActive { get; set; }
    }

    public enum RoleType
    {
        [Description("Developer")]
        DEV = 1,

        [Description("Scrum Master")]
        SM = 2,

        [Description("Product Owner")]
        PO = 3,

        [Description("Guest")]
        GUEST = 4,
    }
}
