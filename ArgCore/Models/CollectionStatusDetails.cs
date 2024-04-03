using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class CollectionStatusDetails
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public Arg.DataModels.BalanceDue BalanceDueInfo { get; set; }

        public string CustomerName { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string CollectionComment { get; set; }

        public SelectList CollectionStatuses { get; set; }

        public List<Arg.DataModels.CollectionComment> CollectionComments { get; set; }
    }
}
