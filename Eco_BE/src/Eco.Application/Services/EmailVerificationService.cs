using Eco.Application.Common.Interfaces.Identity;
using Eco.Application.Common.Interfaces.Persistence;
using Eco.Application.Common.Results;
using Eco.Application.DTOs.Auth;
using Eco.Domain.Enum;
using Microsoft.Extensions.Logging;
using System;   
using System.Threading.Tasks;

namespace Eco.Application.Services
{
    public class EmailVerificationService : IEmailVerificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmailVerificationService> _logger;

        public EmailVerificationService(
            IUnitOfWork unitOfWork,
            ILogger<EmailVerificationService> logger
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        //Kích hoạt tài khoản
        public async Task<Result<VerifyEmailResponseDto>> VerifyEmailAsync(string token)
        {
            // tìm bản ghi emailVerification theo token
            var verification = await _unitOfWork.EmailVerifications.FirstOrDefaultAsync(x => x.Token == token);

            if (verification == null)
            {
                _logger.LogWarning("Verify email thất bại: token không tồn tại - {Token}", token);
                return Result<VerifyEmailResponseDto>.Fail("InvalidToken", "Token is invalid or expired.");
            }

            //check đã verified
            if (verification.Status == Status.VerificationStatus.Verified)
            {
                return Result<VerifyEmailResponseDto>.Ok(new VerifyEmailResponseDto
                {
                    AlreadyVerified = true,
                    Message = "Email đã được xác thực trước đó"
                });
            }

            if (verification.ExpiredAt < DateTime.UtcNow || verification.Status == Status.VerificationStatus.Expired)
            {
                // Cập nhật trạng thái Expired nếu chưa cập nhật (tránh check lại nhiều lần)
                if (verification.Status != Status.VerificationStatus.Expired)
                {
                    verification.Status = Status.VerificationStatus.Expired;
                    await _unitOfWork.SaveChangesAsync();
                }

                return Result<VerifyEmailResponseDto>.Fail("TokenExpired",
                    "Token đã hết hạn, vui lòng yêu cầu gửi lại email xác thực");
            }

            if (verification.Status == Status.VerificationStatus.Revoked)
            {
                return Result<VerifyEmailResponseDto>.Fail("TokenRevoked",
                    "Token đã bị thu hồi, vui lòng yêu cầu gửi lại email xác thực");
            }

            var user = await _unitOfWork.Users.FirstOrDefaultAsync(x => x.Id == verification.UserId);

            if ( user == null || user.IsDeleted)
            {
                _logger.LogWarning("Verify email thất bại: user không tồn tại hoặc bị xóa- {UserId}", verification.UserId);
                return Result<VerifyEmailResponseDto>.Fail("UserNotFound", "User not found.");
            }

            user.EmailVerified = true;

            verification.Status = Status.VerificationStatus.Verified;
            verification.VerifiedAt = DateTime.UtcNow;
            
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Verify email thành công: userId - {UserId}, token - {Token}", user.Id, token);
            return Result<VerifyEmailResponseDto>.Ok(new VerifyEmailResponseDto
            {
                AlreadyVerified = false,
                Message = "Email đã được xác thực thành công"
            });
        }
    }
}
