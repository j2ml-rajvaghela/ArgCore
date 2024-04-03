using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Clients
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }
        public List<Arg.DataModels.IPAddressRestriction> IPAddress { get; set; }

        public List<ArgClient> ClientsList { get; set; }

        public ArgClient ClientDetail { get; set; }

        public SelectList ClientNames { get; set; }
    }
}
