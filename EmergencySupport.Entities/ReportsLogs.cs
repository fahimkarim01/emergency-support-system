using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmergencySupport.Entities
{
    [Table("ReportsLogs")]
    public class ReportsLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        public int UserId { get; set; }

        public string Action { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string Details { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}
