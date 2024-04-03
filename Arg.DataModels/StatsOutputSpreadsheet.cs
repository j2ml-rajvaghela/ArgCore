using Arg.Agility.DataModels;

namespace Arg.DataModels
{
    public class StatsOutputSpreadsheet
    {
        public List<BOLHeader> OutputSpreadsheetStats { get; set; }
        public List<BOLHeaders> AgilityOutputSpreadsheetStats { get; set; }
        public List<ShipmentJournal> ShipmentOutputSpreadsheetStats { get; set; }
    }
}
