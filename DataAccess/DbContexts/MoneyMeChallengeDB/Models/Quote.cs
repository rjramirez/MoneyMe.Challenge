using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.MoneyMeChallengeDB.Models
{
    [Keyless]
    [Table("Quote")]
    public partial class Quote
    {
        [Column("QuoteID")]
        public int QuoteId { get; set; }
        [Column(TypeName = "money")]
        public decimal AmountRequired { get; set; }
        public short Term { get; set; }
        [Required]
        [StringLength(4)]
        [Unicode(false)]
        public string Title { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [Column(TypeName = "numeric(18, 0)")]
        public decimal Mobile { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; }
    }
}
