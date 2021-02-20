using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Programmesana_Sanija_Airita.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
    }
    public class UserMetadata
    {
        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username required")]
        [RegularExpression("^[a-zA-Z0-9 ]{1,50}$", ErrorMessage = "Name must be between 1 and 50 characters and does not contain any special symbols")]
        public string Username { get; set; }

        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name required")]
        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Name does not contain any special symbols")]
        public string Name { get; set; }

        [Display(Name = "Surname")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Surname required")]
        [RegularExpression("^[a-zA-Z]{1,50}$", ErrorMessage = "Surname does not contain any special symbols")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
          public string Password { get; set; }

        
    }
    }