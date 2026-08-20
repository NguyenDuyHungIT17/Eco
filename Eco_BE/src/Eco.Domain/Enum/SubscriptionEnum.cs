using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class SubscriptionEnum
    {
        public enum SubscriptionPlan // các gói đăng kí
        {
            Free = 0, // miễn phí
            Premium = 1, // trả phí
            Enterprise = 2 // trả phí doanh nghiệp
        }
        public enum SubscriptionStatus
        {
            Active = 0, // hoạt động
            Inactive = 1, // không hoạt động
            Expired = 2 // hết hạn
        }
    }
}
