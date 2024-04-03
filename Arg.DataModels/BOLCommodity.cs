using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("BOLCommodity")]
    public class BOLCommodity
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }
        public string ContainerID { get; set; }
        public int? Item { get; set; }
        public decimal? WeightKilos { get; set; }
        public decimal? CBM { get; set; }
        public decimal? WeightPounds { get; set; }
        public decimal? CBF { get; set; }
        public string Tariff { get; set; }
        public string CommodityCode { get; set; }
        public string CommodityDescription { get; set; }
        public string PackageType { get; set; }
        public string CargoType { get; set; }
        public decimal? MinTemperature { get; set; }
        public decimal? MaxTemperature { get; set; }
        public decimal? SetTemperature { get; set; }
        public string CelsiusOrFahrenheit { get; set; }
        public int? QuantityBooked { get; set; }
        public string SITFlag { get; set; }
        public DateTime? Uploaded { get; set; }

        [Computed]
        public string Container { get; set; }

        [Computed]
        public string Commodity { get; set; }

        //[ResultColumn]
        //public string ContainerId { get; set; }
        [Computed]
        public string UNHazmatCode { get; set; }

        [Computed]
        public string PlacardNumber { get; set; }

        [Computed]
        public string Size { get; set; }

        [Computed]
        public string Type { get; set; }

        [Computed]
        public string Weight { get; set; }

        [Computed]
        public string ContainerDetails { get; set; }

        [Computed]
        public string CommodityDescriptions { get; set; }
    }
}
