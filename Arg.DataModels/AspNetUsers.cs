using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("AspNetUsers")]
    public class AspNetUsers
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Status { get; set; }
        public DateTime LoginValidUntil { get; set; }
        public DateTime LastPasswordChangeDate { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Computed]
        public string RoleName { get; set; }
    }
}
