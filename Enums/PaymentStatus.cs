using System.Runtime.Serialization;

namespace fundit_server.Enums
{
    public enum PaymentStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 0,
        [EnumMember(Value = "Successful")]
        Successful = 1,
        [EnumMember(Value = "Failed")]
        Failed = 2,
        [EnumMember(Value = "Cancelled")]
        Cancelled = 3,
    }
}