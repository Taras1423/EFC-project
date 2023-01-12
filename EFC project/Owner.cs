using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFC_project
{
    public class Owner : Worker
    {
        public int StoragesOwned { get; set; }
        public int AnnualIncome { get; set; } // річний дохід
    }
}
