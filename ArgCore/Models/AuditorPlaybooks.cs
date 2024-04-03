using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class AuditorPlaybooks
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();
        public AuditorPlaybook AuditorPlaybookDetail { get; set; }
        public List<AuditorPlaybook> AuditorPlaybookList { get; set; }
        public int? QueryId { get; internal set; }
        public SearchOptions SearchOptions { get; set; }
        public SelectList StatusList { get; set; }
        public string FilterResult { get; set; }
        public string Comment { get; set; }
    }
}
