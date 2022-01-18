using System.ComponentModel.DataAnnotations;

namespace PwaCore.Models
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "لطفا نام را وارد کنید")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفا ایمیل را وارد کنید")]
        [DataType(DataType.EmailAddress, ErrorMessage = "ایمیل وارد شده اشتباه است")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا شماره تلفن را وارد کنید")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "شماره وارد شده اشتباه است")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا پسورد را وارد کنید")]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا پسورد را وارد کنید")]
        [Compare("Password", ErrorMessage = "پسورد وارد شده با تکرار آن مغایرت دارد")]
        public string RePassword { get; set; }
    }



    
}
