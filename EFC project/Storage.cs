using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EFC_project
{
    public class Storage
    {
        public int Number { get; set; }
        public int NumberOfRooms { get; set; }
        public int IdOfWorker { get; set; }
        public string WorkerPassportNumber { get; set; }
        public string WorkerPassportSeria { get; set; }
        public int Size { get; set; }
        public string? NameOfOwner { get; set; }
        public List<Visiting> Visitings { get; set; }
        public List<Worker> Workers { get; set; }
        public List<Room> Rooms { get; set; }
    }
}