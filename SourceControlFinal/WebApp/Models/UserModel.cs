using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    [Bind(Exclude = "Id")]
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "First Name Required")]
        [DataType(DataType.Text)]
        [Display(Name = "First Name  : ")]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name Required")]
        [DataType(DataType.Text)]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Last Name  : ")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]
        [Display(Name = "Email  : ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password  : ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Required")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password Not Matched")]
        [Display(Name = "Confirm Password  : ")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone Number Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{2})[-. ]?([0-9]{4})[-. ]?([0-9]{3})[-. ]?([0-9]{3})$", ErrorMessage = "Not a valid Phone number")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Age is Required")]
        [FormValidation.CustomAttribute.MinAge(18)]
        public string Age { get; set; }

        [Display(Name ="Upload Image")]
        public string ImagePath{get; set;}
        public HttpPostedFileBase ImageFile { get; set; }
        public DateTime CreatedOn { get; set; }


    }
}