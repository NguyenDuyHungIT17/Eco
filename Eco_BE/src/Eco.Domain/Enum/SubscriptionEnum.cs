using System;
using System.Collections.Generic;
using System.Text;

namespace Eco.Domain.Enum
{
    public class SubscriptionEnum
    {
        public enum SubscriptionPlan
        {
            Free = 0,
            Premium = 1,
            Enterprise = 2
        }
        public enum SubscriptionStatus
        {
            Active = 0,
            Inactive = 1,
            Expired = 2
        }
    }
}
