using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class AirCraftSettingRecords : SQLRecords<AirCraftSetting>
    {
        public static AirCraftSettingRecords Instance = new AirCraftSettingRecords();

        internal object Add(AirCraft nowSelectAirCraft)
        {
            return null;
        }
        internal object Save()
        {
            return null;
        }
    }
}
