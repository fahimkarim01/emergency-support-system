using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmergencySupport.Entities
{
    [Table("Assignments")]
    public class Assignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssignmentId { get; set; }

        public int RequestId { get; set; }

        public int ResponderId { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.Now;

        public DateTime? ArrivalTime { get; set; }

        public DateTime? CompletionTime { get; set; }

        public string Status { get; set; } = "Pending";

        public int AssignedBy { get; set; }

        [ForeignKey("RequestId")]
        public virtual EmergencyRequests Request { get; set; }

        [ForeignKey("ResponderId")]
        public virtual Responders Responder { get; set; }
    }
}
