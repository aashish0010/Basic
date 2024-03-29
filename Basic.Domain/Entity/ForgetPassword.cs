﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Domain.Entity
{
    public class ForgetPassword
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Userid { get; set; }
        public string? Email { get; set; }
        public string? Processid { get; set; }
        public string? Status { get; set; }
        public DateTime? Createdate { get; set; }
        public DateTime? Approvedate { get; set; }
    }
}
