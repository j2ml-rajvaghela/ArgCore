using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class Templates
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public IEnumerable<Arg.DataModels.Templates> TemplatesList { get; set; }

        public Arg.DataModels.Templates TemplateDetail { get; set; }

        public SelectList Categories { get; set; }

        public string ErrorMessage { get; set; }
    }
}
