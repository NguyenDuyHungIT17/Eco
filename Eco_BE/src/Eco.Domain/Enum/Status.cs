using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class Status
    {
        public enum VerificationStatus
        {
            /// <summary>
            /// Token vừa được sinh ra, đang chờ người dùng xác thực (click link email).
            /// Đây là trạng thái duy nhất được phép verify thành công.
            /// </summary>
            Pending = 0,

            /// <summary>
            /// Token đã được sử dụng thành công để xác thực (email đã verify).
            /// Nếu verify lại lần nữa với token này -> coi là idempotent,
            /// trả về "đã xác thực trước đó", không báo lỗi.
            /// </summary>
            Verified = 1,

            /// <summary>
            /// Token đã quá thời hạn sử dụng (ExpiredAt &lt; Now) nhưng chưa từng
            /// được verify thành công. Được set "lazy" tại thời điểm user click link
            /// hết hạn, không cần job quét nền.
            /// </summary>
            Expired = 2,

            /// <summary>
            /// Token bị chủ động vô hiệu hóa (không phải do hết hạn tự nhiên).
            /// Ví dụ: user bấm "gửi lại email xác thực" -> token cũ bị revoke
            /// để tránh tồn tại nhiều token Pending cùng lúc cho 1 user.
            /// </summary>
            Revoked = 3
        }

        public enum ResetPasswordStatus
        {
            Pending = 0,
            Reset = 1,
            Expired = 2
        }
    }
}
