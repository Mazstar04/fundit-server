using System.Runtime.Serialization;

namespace fundit_server.Enums
{
    public enum WithdrawalStatus
    {
        [EnumMember(Value = "Pending")]
        Pending = 0,
        [EnumMember(Value = "Successful")]
        Successful = 1,
        [EnumMember(Value = "Failed")]
        Failed = 3,
    }
}