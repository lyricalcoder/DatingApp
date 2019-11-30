using System.ComponentModel.DataAnnotations;
namespace DatingAppAPI.Dtos
{
    public class UserForRegisterDTO
    {
        [Required]
        public string UserName { get; set; }
       
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Passowrds must be between 8 and 20 characters.")]
         public string Password { get; set; }
    }
}