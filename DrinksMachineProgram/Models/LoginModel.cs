using System.ComponentModel.DataAnnotations;

namespace DrinksMachineProgram.Models
{

    public class LoginModel
    {

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "The user name is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; }

    }

}
