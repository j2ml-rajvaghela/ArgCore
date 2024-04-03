using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("HouseHAWBAir")]
    public class HouseHAWBAir
    {
        [Key]
        public int ID { get; set; }
        public string airline_code { get; set; }
        public string forwarder_name { get; set; }
        public string agent_code { get; set; }
        public DateTime sold_date { get; set; }
        public DateTime flown_date { get; set; }
        public string flown_date_year { get; set; }
        public string flown_date_month { get; set; }
        public string flown_date_day { get; set; }
        public string awb_prefix { get; set; }
        public string awb_nr { get; set; }
        public string m_awb_nr { get; set; }
        public string origin_country { get; set; }
        public string origin { get; set; }
        public string routing { get; set; }
        public string flight_no { get; set; }
        public string final_destination_country { get; set; }
        public string final_destination { get; set; }
        public string weight_unit { get; set; }
        public decimal actual_weight { get; set; }
        public string volume_unit { get; set; }
        public decimal volume { get; set; }
        public decimal chargeable_weight { get; set; }
        public string special_handling_code { get; set; }
        public string commoditiy_code { get; set; }
        public string product_code { get; set; }
        public string nature_of_goods { get; set; }
        public decimal net_freight_charges { get; set; }
        public decimal fuel_surcharge { get; set; }
        public decimal security_surcharge { get; set; }
        public decimal total_cost { get; set; }
        public decimal other_charges_due_carrier { get; set; }
        public string base_currency { get; set; }
        public DateTime loaded { get; set; }
    }
}
