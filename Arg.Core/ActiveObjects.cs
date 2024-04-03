namespace Arg.Core
{
    public class ActiveObjects
    {
        public class EditInfo
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string BolNo { get; set; }
        }

        public static List<EditInfo> ItemsBeingEdited = new List<EditInfo>();

        public static string BolIsBeingEditedInfo(string bolNo)
        {
            var info = BolIsBeingEdited(bolNo);
            if (info.BolNo != null && info.BolNo.Length > 0)
            {
                return "This BOL is being Edited by " + (!string.IsNullOrWhiteSpace(info.Email) ? info.Email + ", " : "") + info.Name;
            }
            return "";
        }

        public static EditInfo BolIsBeingEdited(string bolNo)
        {
            return ItemsBeingEdited.FirstOrDefault(x => x.BolNo == bolNo) ?? new EditInfo();
        }

        public static void SetBolBeingEdited(string bolNo, string email, string name)
        {
            ItemsBeingEdited.Add(new EditInfo { BolNo = bolNo, Email = email, Name = name });
        }
    }
}
