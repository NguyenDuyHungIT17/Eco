using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class Otp
    {
        public enum OtpPurpose
        {
            Register = 0,
            Login = 1,
            ForgotPassword = 2,
            VerifyEmail = 3,
            ChangePassword = 4
        } 

        public enum OtpStatus
        {
            Pending = 0,
            Verified = 1,
            Expired = 2
        }
    }
}
