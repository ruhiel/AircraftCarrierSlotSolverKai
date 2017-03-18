using CsvHelper.Configuration;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class FleetMap : CsvClassMap<Fleet>
    {
        public FleetMap()
        {
            Map(m => m.ID).Index(0).Name("ID");
            Map(m => m.Name).Index(1).Name("艦隊名");
            Map(m => m.AirSuperiorityPotential).Index(2).Name("制空値");
            Map(m => m.Ship1Name).Index(3).Name("艦名1");
            Map(m => m.Ship1Slot1).Index(4).Name("艦名1スロット1");
            Map(m => m.Ship1Slot1Improvement).Index(5).Name("艦名1スロット1改修");
            Map(m => m.Ship1Slot2).Index(6).Name("艦名1スロット2");
            Map(m => m.Ship1Slot2Improvement).Index(7).Name("艦名1スロット2改修");
            Map(m => m.Ship1Slot3).Index(8).Name("艦名1スロット3");
            Map(m => m.Ship1Slot3Improvement).Index(9).Name("艦名1スロット3改修");
            Map(m => m.Ship1Slot4).Index(10).Name("艦名1スロット4");
            Map(m => m.Ship1Slot4Improvement).Index(11).Name("艦名1スロット4改修");
            Map(m => m.Ship2Name).Index(12).Name("艦名2");
            Map(m => m.Ship2Slot1).Index(13).Name("艦名2スロット1");
            Map(m => m.Ship2Slot1Improvement).Index(14).Name("艦名2スロット1改修");
            Map(m => m.Ship2Slot2).Index(15).Name("艦名2スロット2");
            Map(m => m.Ship2Slot2Improvement).Index(16).Name("艦名2スロット2改修");
            Map(m => m.Ship2Slot3).Index(17).Name("艦名2スロット3");
            Map(m => m.Ship2Slot3Improvement).Index(18).Name("艦名2スロット3改修");
            Map(m => m.Ship2Slot4).Index(19).Name("艦名2スロット4");
            Map(m => m.Ship2Slot4Improvement).Index(20).Name("艦名2スロット4改修");
            Map(m => m.Ship3Name).Index(21).Name("艦名3");
            Map(m => m.Ship3Slot1).Index(22).Name("艦名3スロット1");
            Map(m => m.Ship3Slot1Improvement).Index(23).Name("艦名3スロット1改修");
            Map(m => m.Ship3Slot2).Index(24).Name("艦名3スロット2");
            Map(m => m.Ship3Slot2Improvement).Index(25).Name("艦名3スロット2改修");
            Map(m => m.Ship3Slot3).Index(26).Name("艦名3スロット3");
            Map(m => m.Ship3Slot3Improvement).Index(27).Name("艦名3スロット3改修");
            Map(m => m.Ship3Slot4).Index(28).Name("艦名3スロット4");
            Map(m => m.Ship3Slot4Improvement).Index(29).Name("艦名3スロット4改修");
            Map(m => m.Ship4Name).Index(30).Name("艦名4");
            Map(m => m.Ship4Slot1).Index(31).Name("艦名4スロット1");
            Map(m => m.Ship4Slot1Improvement).Index(32).Name("艦名4スロット1改修");
            Map(m => m.Ship4Slot2).Index(33).Name("艦名4スロット2");
            Map(m => m.Ship4Slot2Improvement).Index(34).Name("艦名4スロット2改修");
            Map(m => m.Ship4Slot3).Index(35).Name("艦名4スロット3");
            Map(m => m.Ship4Slot3Improvement).Index(36).Name("艦名4スロット3改修");
            Map(m => m.Ship4Slot4).Index(37).Name("艦名4スロット4");
            Map(m => m.Ship4Slot4Improvement).Index(38).Name("艦名4スロット4改修");
            Map(m => m.Ship5Name).Index(39).Name("艦名5");
            Map(m => m.Ship5Slot1).Index(40).Name("艦名5スロット1");
            Map(m => m.Ship5Slot1Improvement).Index(41).Name("艦名5スロット1改修");
            Map(m => m.Ship5Slot2).Index(42).Name("艦名5スロット2");
            Map(m => m.Ship5Slot2Improvement).Index(43).Name("艦名5スロット2改修");
            Map(m => m.Ship5Slot3).Index(44).Name("艦名5スロット3");
            Map(m => m.Ship5Slot3Improvement).Index(45).Name("艦名5スロット3改修");
            Map(m => m.Ship5Slot4).Index(46).Name("艦名5スロット4");
            Map(m => m.Ship5Slot4Improvement).Index(47).Name("艦名5スロット4改修");
            Map(m => m.Ship6Name).Index(48).Name("艦名6");
            Map(m => m.Ship6Slot1).Index(49).Name("艦名6スロット1");
            Map(m => m.Ship6Slot1Improvement).Index(50).Name("艦名6スロット1改修");
            Map(m => m.Ship6Slot2).Index(51).Name("艦名6スロット2");
            Map(m => m.Ship6Slot2Improvement).Index(52).Name("艦名6スロット2改修");
            Map(m => m.Ship6Slot3).Index(53).Name("艦名6スロット3");
            Map(m => m.Ship6Slot3Improvement).Index(54).Name("艦名6スロット3改修");
            Map(m => m.Ship6Slot4).Index(55).Name("艦名6スロット4");
            Map(m => m.Ship6Slot4Improvement).Index(56).Name("艦名6スロット4改修");
        }
    }
}
