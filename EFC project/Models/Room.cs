using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EFC_project.Models
{
    public class Room
    {
        public int Number { get; set; }
        public int NumberOfStorage { get; set; }
        public int StorageNumber { get; set; }
        public int NumberOfRows { get; set; }
        public string TemperatureRange { get; set; }
        public virtual MainStorage Storage { get; set; } = null!;

    }
}