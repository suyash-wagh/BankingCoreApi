using System.ComponentModel.DataAnnotations;

namespace BankingCoreApi.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        [RegularExpression("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = " Enter valid email address.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
