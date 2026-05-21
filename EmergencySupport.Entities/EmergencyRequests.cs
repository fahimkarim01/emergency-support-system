using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmergencySupport.Entities
{
    [Table("EmergencyRequests")]
    public class EmergencyRequests
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

        [Display(Name = "User Name")]
        public int UserId { get; set; }

        [Required]
        public string EmergencyType { get; set; }

        [Required]
        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string PriorityLevel { get; set; } = "Pending";

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}