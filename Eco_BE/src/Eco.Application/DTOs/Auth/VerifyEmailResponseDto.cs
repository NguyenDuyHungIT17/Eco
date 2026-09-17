using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Application.DTOs.Auth
{
    public class VerifyEmailResponseDto
    {
        public bool AlreadyVerified { get; set; }
        public string Message { get; set; } = default!;

    }
}
