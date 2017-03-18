using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.ViewModels
{
    public class IntAttribute : ValidationAttribute
    {
        public IntAttribute(string errorMessage) : base(errorMessage)
        {
        }

        public override bool IsValid(object value) => int.TryParse(value?.ToString(), out int temp);
    }
}
