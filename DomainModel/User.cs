    using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace DomainModel
{
    [Table("User")]
    public class User : ObjectModel
    {
        public User()
        {
            //FullName = string.Empty;
            //Password = string.Empty;
            //MobileNumber = string.Empty;
            //Email = string.Empty;
            //Address = string.Empty;
            //UserType = new UserType();
            //Orders = new List<Order>();
        }

        public int UserTypeId { get; set; }
        public virtual UserType UserType { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public List<Order> Orders { get; set; }
    }
}