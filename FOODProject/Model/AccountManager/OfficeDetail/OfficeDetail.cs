using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FOODProject.Model.AccountManager.OfficeDetail
{
    public class OfficeDetail
    {
        public string OfficeName { get; set; }
        public string Managername { get; set; }
        public ulong PhoneNumber { get; set; }
        public ulong AlternateNumber { get; set; }
        public string Image { get; set; }

        public Model.Common.IntegerNullString UserId { get; set; } = new Model.Common.IntegerNullString();
    }
}
