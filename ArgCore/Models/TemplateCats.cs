using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class TemplateCats
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public IEnumerable<Arg.DataModels.TemplateCats> TemplateCatsList { get; set; }

        public Arg.DataModels.TemplateCats TemplateCatDetail { get; set; }

        public string ErrorMessage { get; set; }
    }
}
