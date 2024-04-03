namespace Arg.DataModels
{

    public class emailrecipient
    {
        //public properties
        public int PK { get; set; }
        public int EmailId { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class Email
    {
        public int EmailId { get; set; }
        public string BatchId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public bool IsBodyHtml { get; set; }
        public DateTime DateSent { get; set; }
        public bool Sent { get; set; }
        public bool Error { get; set; }
        public string ErrorText { get; set; }
        public int Attempts { get; set; }
        public int Priority { get; set; }
        public int Sequence { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        //recipients
        public string SingleRecipient { get; set; }

        public List<emailrecipient> Recipients { get; set; }

        public string ReplyTo
        {
            get
            {
                string Value = string.Empty;

                foreach (emailrecipient r in Recipients.Where(x => IsEqual(x.Type, "ReplyTo")))
                {
                    if (!IsBlank(Value))
                    {
                        Value += "; ";
                    }
                    Value += string.Format("{0}", r.Email);
                }

                return Value;
            }
        }

        public string To
        {
            get
            {
                string Value = string.Empty;

                foreach (emailrecipient r in Recipients.Where(x => IsEqual(x.Type, "To")))
                {
                    if (!IsBlank(Value))
                    {
                        Value += "; ";
                    }

                    Value += string.Format("{0}", r.Email);
                }

                return Value;
            }
        }

        public string CC
        {
            get
            {
                string Value = string.Empty;

                foreach (emailrecipient r in Recipients.Where(x => IsEqual(x.Type, "CC")))
                {
                    if (!IsBlank(Value))
                    {
                        Value += "; ";
                    }

                    Value += string.Format("{0}", r.EmailId);
                }

                return Value;
            }
        }

        public string BCC
        {
            get
            {
                string Value = string.Empty;

                foreach (emailrecipient r in Recipients.Where(x => IsEqual(x.Type, "BCC")))
                {
                    if (!IsBlank(Value))
                    {
                        Value += "; ";
                    }

                    Value += string.Format("{0}", r.Email);
                }

                return Value;
            }
        }

        private bool IsEqual(string Value, string CompareValue)
        {
            bool ReturnValue = false;

            if (Value != null && CompareValue != null)
            {
                ReturnValue = string.Compare(Value.Trim(), CompareValue.Trim(), StringComparison.OrdinalIgnoreCase) == 0;
            }

            return ReturnValue;
        }

        private bool IsBlank(string Value)
        {
            bool ReturnValue = true;

            if (Value != null)
            {
                ReturnValue = Value.Trim().Length == 0;
            }

            return ReturnValue;
        }
    }
}

