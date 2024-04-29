using System.ComponentModel;

namespace ScrumPoker.Model.Enums
{
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
