﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC_project.Models
{
    [Table("Client")]
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        [MaxLength(50)]
        [Required]
        public string Email { get; set; }
        public int IdOfDocument { get; set; }
        public string PassportNumber { get; set; }
        public string PassportSeria { get; set; }
        public List<Document> Documents { get; set; }
        public List<Visiting> Visitings { get; set; }
    }
}