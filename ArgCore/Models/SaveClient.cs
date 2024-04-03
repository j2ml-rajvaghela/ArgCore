using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class SaveClient
    {
        public Arg.DataModels.Client Client { get; set; }

        public SelectList Countries { get; set; }
    }
}
