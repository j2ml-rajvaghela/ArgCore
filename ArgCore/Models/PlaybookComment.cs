using Arg.DataModels;
using ArgCore.Helpers;

namespace ArgCore.Models
{
    public class PlaybookComment
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public AuditorPlaybook AuditorPlaybookDetails { get; internal set; }
        public PlaybookComments PlaybookCommentsDetail { get; set; }
        public List<PlaybookComments> PlaybookCommentsList { get; set; }
        public int PlayId { get; set; }
    }
}
