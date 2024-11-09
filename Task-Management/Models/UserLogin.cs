using System.ComponentModel.DataAnnotations;

namespace Task_Management.Models
{
    public class UserLogin
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin = 1,
        Editor = 2,
        Viewer = 3
    }
}
