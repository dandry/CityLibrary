using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CityLibrary.DAL.Models;

namespace CityLibrary.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamiętać tę przeglądarkę?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętać?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Podane hasła różnią się od siebie.")]
        public string ConfirmPassword { get; set; }

        [RequiredAttribute, Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required, Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Adres e-mail musi być w formacie nazwa@domena.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{11}$", ErrorMessage = "Number PESEL musi składać się z 11 cyfr.")]
        //[Remote("IsPeselAvailable", "Users", AdditionalFields = "UserId", ErrorMessage = "Użytkownik z podanym numerem PESEL znajduje się już w bazie.")]
        public long PESEL { get; set; }

        [Required, Display(Name = "Data urodzin")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [Required, Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Nr lokalu")]
        public int? ApartmentNumber { get; set; }

        [Required, Display(Name = "Kod pocztowy")]
        [RegularExpression(@"^[0-9]{2}-[0-9]{3}$", ErrorMessage = "Kod pocztowy musi być w formacie 00-000")]
        public string PostalCode { get; set; }

        [Required, Display(Name = "Miasto")]
        public string City { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Podane hasła różnią się od siebie.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
