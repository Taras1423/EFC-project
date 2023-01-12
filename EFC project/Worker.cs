using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC_project
{
    [Table("Worker")]
    public class Worker
    {
        //[Key]
        public int WorkerId { get; set; }

        [ForeignKey("FK_Worker_NumberOfStorage")]
        public int NumberOfStorage { get; set; }

        [MaxLength(25)]
        public string? Name { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [MaxLength(25)]
        public string WorkShedule { get; set; }

        [MaxLength(25)]
        public string Position { get; set; }

        public string PassportNumber { get; set; }
        public string PassportSeria { get; set; }
        public Storage Storage { get; set; }

    }
}
