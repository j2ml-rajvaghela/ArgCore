using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class LoadClientData
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SelectList Companies { get; set; }

        public int CompanyId { get; set; }

        public SelectList DataFiles { get; set; }

        public string SelectedFile { get; set; }

        public string FileName { get; set; }

        public SelectList TruncateTables { get; set; }
    }
}
