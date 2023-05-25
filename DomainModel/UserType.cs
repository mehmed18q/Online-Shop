using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("UserType")]
    public class UserType
    {
        public UserType()
        {
            Id = 0;
            Name = string.Empty;
            Title = string.Empty;
            Users = new List<User>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public List<User> Users { get; set; }
    }
}