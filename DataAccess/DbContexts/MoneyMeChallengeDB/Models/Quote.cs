using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContexts.MoneyMeChallengeDB.Models
{
    [Table("Quote")]
    public partial class Quote
    {
        [Key]
        public int QuoteId { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
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
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string MobileNumber { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
