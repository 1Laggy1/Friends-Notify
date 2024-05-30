using System.ComponentModel.DataAnnotations;

namespace Friends_Notify.Models
{
    public class User
    {
        [Key]
        public ulong Id { get; set; }
    }
}
