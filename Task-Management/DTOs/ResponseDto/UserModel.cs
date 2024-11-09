using System.ComponentModel.DataAnnotations;
using Task_Management.Models;

namespace Task_Management.DTOs.ResponseDto
{
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required] 
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public Role Role { get; set; }
       
    }
}
