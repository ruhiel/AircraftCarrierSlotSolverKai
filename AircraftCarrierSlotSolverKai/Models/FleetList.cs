using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class FleetList : ObservableCollection<Fleet>
    {
        public static FleetList Instance = new FleetList();

        private FleetList()
        {
            FleetRecords.Instance.Load();

            foreach (var record in FleetRecords.Instance.Records.OrderBy(x => x.ID))
            {
                Add(record);
            }
        }

        public void Add(string fleetName, int airSuperiorityPotential, IEnumerable<ShipSlotInfo> shipSlotInfos)
        {
            var fleet = new Fleet();
            fleet.ID = FleetRecords.Instance.Records.Max(x => x.ID) + 1;
            fleet.Name = fleetName;
            fleet.AirSuperiorityPotential = airSuperiorityPotential;

            for (var i = 0; i < shipSlotInfos.Count(); i++)
            {
                var element = shipSlotInfos.ElementAt(i);
                switch (i)
                {
                    case 0:
                        fleet.Ship1Name = element.ShipName;
                        fleet.Ship1Slot1 = element.Slot1.Name;
                        fleet.Ship1Slot1Improvement = element.Slot1.Improvement;
                        fleet.Ship1Slot2 = element.Slot2.Name;
                        fleet.Ship1Slot2Improvement = element.Slot2.Improvement;
                        fleet.Ship1Slot3 = element.Slot3.Name;
                        fleet.Ship1Slot3Improvement = element.Slot3.Improvement;
                        fleet.Ship1Slot4 = element.Slot4.Name;
                        fleet.Ship1Slot4Improvement = element.Slot4.Improvement;
                        break;
                    case 1:
                        fleet.Ship2Name = element.ShipName;
                        fleet.Ship2Slot1 = element.Slot1.Name;
                        fleet.Ship2Slot1Improvement = element.Slot1.Improvement;
                        fleet.Ship2Slot2 = element.Slot2.Name;
                        fleet.Ship2Slot2Improvement = element.Slot2.Improvement;
                        fleet.Ship2Slot3 = element.Slot3.Name;
                        fleet.Ship2Slot3Improvement = element.Slot3.Improvement;
                        fleet.Ship2Slot4 = element.Slot4.Name;
                        fleet.Ship2Slot4Improvement = element.Slot4.Improvement;
                        break;
                    case 2:
                        fleet.Ship3Name = element.ShipName;
                        fleet.Ship3Slot1 = element.Slot1.Name;
                        fleet.Ship3Slot1Improvement = element.Slot1.Improvement;
                        fleet.Ship3Slot2 = element.Slot2.Name;
                        fleet.Ship3Slot2Improvement = element.Slot2.Improvement;
                        fleet.Ship3Slot3 = element.Slot3.Name;
                        fleet.Ship3Slot3Improvement = element.Slot3.Improvement;
                        fleet.Ship3Slot4 = element.Slot4.Name;
                        fleet.Ship3Slot4Improvement = element.Slot4.Improvement;
                        break;
                    case 3:
                        fleet.Ship4Name = element.ShipName;
                        fleet.Ship4Slot1 = element.Slot1.Name;
                        fleet.Ship4Slot1Improvement = element.Slot1.Improvement;
                        fleet.Ship4Slot2 = element.Slot2.Name;
                        fleet.Ship4Slot2Improvement = element.Slot2.Improvement;
                        fleet.Ship4Slot3 = element.Slot3.Name;
                        fleet.Ship4Slot3Improvement = element.Slot3.Improvement;
                        fleet.Ship4Slot4 = element.Slot4.Name;
                        fleet.Ship4Slot4Improvement = element.Slot4.Improvement;
                        break;
                    case 4:
                        fleet.Ship5Name = element.ShipName;
                        fleet.Ship5Slot1 = element.Slot1.Name;
                        fleet.Ship5Slot1Improvement = element.Slot1.Improvement;
                        fleet.Ship5Slot2 = element.Slot2.Name;
                        fleet.Ship5Slot2Improvement = element.Slot2.Improvement;
                        fleet.Ship5Slot3 = element.Slot3.Name;
                        fleet.Ship5Slot3Improvement = element.Slot3.Improvement;
                        fleet.Ship5Slot4 = element.Slot4.Name;
                        fleet.Ship5Slot4Improvement = element.Slot4.Improvement;
                        break;
                    default:
                        fleet.Ship6Name = element.ShipName;
                        fleet.Ship6Slot1 = element.Slot1.Name;
                        fleet.Ship6Slot1Improvement = element.Slot1.Improvement;
                        fleet.Ship6Slot2 = element.Slot2.Name;
                        fleet.Ship6Slot2Improvement = element.Slot2.Improvement;
                        fleet.Ship6Slot3 = element.Slot3.Name;
                        fleet.Ship6Slot3Improvement = element.Slot3.Improvement;
                        fleet.Ship6Slot4 = element.Slot4.Name;
                        fleet.Ship6Slot4Improvement = element.Slot4.Improvement;
                        break;
                }
            }

            Add(fleet);

            Save();
        }

        public void Save()
        {
            FleetRecords.Instance.Records.Clear();

            FleetRecords.Instance.Records.AddRange(this);

            FleetRecords.Instance.Save();
        }
    }
}
