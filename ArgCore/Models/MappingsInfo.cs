using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class MappingsInfo
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SelectList HeadersList { get; set; }

        public SelectList TablesList { get; set; }

        public SelectList TableColumnsList { get; set; }

        public string Name { get; set; }

        [Required]
        public int SelectedColumnIndex { get; set; }

        [Required]
        public string SelectedTable { get; set; }

        [Required]
        public string SelectedColumn { get; set; }

        public List<Arg.DataModels.Mappings.Mapping> Mappings { get; set; }

        public string SelectedHeaderName { get; set; }

        public string File { get; set; }
    }
}
