using System;

namespace Framework
{
    public class Enums
    {
        public enum UserType
        {
            User=1,
            Admin=2
        }

        public enum OrderStatus
        {
            PendingPayment=1,
            Paid=2,
            ErrorOccurred=3,
            Canceled=4,
            Returned=5,
        }
    }
}
