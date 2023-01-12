﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EFC_project
{
    public class Visiting
    {
        public int VisitingId { get; set; }
        public int IdOfClient { get; set; }
        public int NumberOfStorage { get; set; }
        public string DateOfEntry { get; set; }
        public string DateOfExit { get; set; }
        public Storage Storage { get; set; }
        public Client Client { get; set; }

    }
}