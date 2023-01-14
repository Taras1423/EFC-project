using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EFC_project.Models
{
    public class Visiting
    {
        public int VisitingId { get; set; }
        public int IdOfClient { get; set; }
        public int NumberOfStorage { get; set; }
        public string DateOfEntry { get; set; }
        public string DateOfExit { get; set; }
        public string ClientPassportSeria { get; set; }
        public string ClientPassportNumber { get; set; }
        public string ClientPhoneNumber { get; set; }
        public virtual MainStorage Storage { get; set; } = null!;
        public virtual Client Client { get; set; } = null!;

    }
}