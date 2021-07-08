using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Model.Model
{
    public class Login
    {
        [Required]
        [StringLength(36, ErrorMessage = "maximum 36.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter valid room name")]
        public string RoomName { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "maximum 15.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Please enter valid user name")]
        public string UserName { get; set; }
    }
}
