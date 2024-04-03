namespace Arg.DataModels
{
    public class billoflading
    {
        public int billofladingid { get; set; }

        public int clientid { get; set; }

        public int customerid { get; set; }

        public string bolnumber { get; set; }

        public string origin { get; set; }

        public string destination { get; set; }

        public int? bookingid { get; set; }

        public string datebol { get; set; }

        public string datecreated { get; set; }

        public string dateupdated { get; set; }

        public bool isactive { get; set; }

        public int? statusid { get; set; }

        public int stat { get; set; }

        public string customername { get; set; }

        public int cntrcount { get; set; }

        public int tcnt { get; set; }

        public int rpos { get; set; }
    }
}
