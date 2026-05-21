using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmergencySupport.Entities
{
    [Table("Responders")]
    public class Responders
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponderId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string ServiceType { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string AvailabilityStatus { get; set; } = "Available";

        public double CurrentLatitude { get; set; }

        public double CurrentLongitude { get; set; }

        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
    }
}
