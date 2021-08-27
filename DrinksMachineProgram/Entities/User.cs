using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrinksMachineProgram.Entities
{

    public class User
    {

        #region Entity Attributes

        [Key]
        public short Id { get; set; }

        [StringLength(100)]
        [Display(Name = "User Name")]
        [Required(ErrorMessage = "The user name is required.")]
        public string UserName { get; set; }

        [StringLength(150)]
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "The first name is required.")]
        public string FirstName { get; set; }

        [StringLength(150)]
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The last name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        public byte[] PasswordHash { get; set; }

        #endregion Entity Attributes

        #region Model Attributes

        [NotMapped]
        [Required(ErrorMessage = "The password is required.")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "The password confirmation is required.")]
        public string PasswordConfirmation { get; set; }

        public string CreationDateFormat => CreationDate.ToShortDateString();

        #endregion Model Attributes

    }

}
