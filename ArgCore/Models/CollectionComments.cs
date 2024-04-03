using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class CollectionComments
    {
        public List<Arg.DataModels.CollectionComment> CollectionCommentsList { get; set; }

        public int BalanceId { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string CollectionComment { get; set; }
    }
}
