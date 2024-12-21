using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.MoneyMeChallengeDB.Models
{
    [Table("Blacklist")]
    public partial class Blacklist
    {
        [Key]
        public int BlacklistId { get; set; }
        [StringLength(20)]
        public string Mobile { get; set; }
        [StringLength(50)]
        public string Domain { get; set; }
        public bool Active { get; set; }
        [Required]
        [StringLength(200)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(200)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
