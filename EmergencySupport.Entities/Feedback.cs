using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmergencySupport.Entities
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }

        [Display(Name = "Emergency Request")]
        public int RequestId { get; set; }

        [Display(Name = "User Name")]
        public int UserId { get; set; }

        [Required]
        public int Rating { get; set; }

        [Required]
        public string Comments { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }

        [ForeignKey("RequestId")]
        public virtual EmergencyRequests Request { get; set; }
    }
}
