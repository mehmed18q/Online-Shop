using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("UserLogin")]
    public class UserLogin
    {
        public UserLogin() { }
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Ip { get; set; }
        public string? Browser { get; set; }
        public string? Token { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public virtual UserType? UserType { get; set; }
        public bool? HasPermission { get; set; }
    }
}