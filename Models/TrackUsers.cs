using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Friends_Notify.Models
{
    [Keyless]
    public class TrackUsers
    {
        public ulong UserId { get; set; }

        public ulong TrackingUserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("TrackingUserId")]
        public User TrackingUser { get; set; }
    }
}
