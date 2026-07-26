using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class Status
    {
        public enum VerificationStatus
        {
            Pending = 0,
            Verified = 1,
            Expired = 2
        }

        public enum ResetPasswordStatus
        {
            Pending = 0,
            Reset = 1,
            Expired = 2
        }
    }
}
