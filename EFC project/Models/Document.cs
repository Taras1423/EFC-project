using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC_project.Models
{
    [Table("Document")]
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [ForeignKey("FK_Document_IdOfClient")]
        public int IdOfClient { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfTakeRent { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfCancelRent { get; set; }

        public int NumberOfCell { get; set; }

        public Client Client { get; set; }

    }
}