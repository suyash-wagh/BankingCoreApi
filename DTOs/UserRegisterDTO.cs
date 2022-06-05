using System.ComponentModel.DataAnnotations;

namespace BankingCoreApi.DTOs
{
    public class UserRegisterDTO
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [Range(500,1000000, ErrorMessage ="Minimum balance should be 500 while registering.")]
        public int Balance { get; set; }
        
        [Required]
        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = " Enter valid email address.")]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
