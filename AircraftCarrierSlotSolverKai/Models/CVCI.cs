using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class CVCI
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public CVCI(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
