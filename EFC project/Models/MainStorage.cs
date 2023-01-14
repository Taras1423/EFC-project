using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFC_project.Models
{
    public partial class MainStorage
    {
        public int Number { get; set; }
        public int NumberOfRooms { get; set; }
        public int IdOfWorker { get; set; }
        public string WorkerPassportNumber { get; set; }
        public string WorkerPassportSeria { get; set; }
        public int Size { get; set; }
        public string? NameOfOwner { get; set; }
        public virtual List<Visiting> Visitings { get; set; } = new List<Visiting>();
        public virtual List<Worker> Workers { get; set; } = new List<Worker>();
        public virtual List<Room> Rooms { get; set; } = new List<Room>();
    }
}