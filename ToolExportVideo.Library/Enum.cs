namespace ToolExportVideo.Library
{
    public class Enum
    {

    }
    public enum EditMode
    {
        None = 0,
        Add = 1,
        Update = 2,
        Delete = 3,
    }

    public enum ErrorCode
    {
        NoneError = 0,
        InvalidInput = 1,
        Unknown = 2,
        NotFound = 3,
        Conflict = 4,
        DuplicateCode = 5,
        EmployeeNotFound = 6,
        InvalidParam = 7,
        PointNotFound = 8,
        InvalidActiveCode = 9,
        NotEnoughPoints = 10,
        ErrorMinPoints = 11,

    }
    public enum MemberStatus
    {
        None = 0,
        Active = 1,
        DeActivate = 2,
        WaitActive = 3

    }
    public enum StatusPayReward
    {
        None = 0,
        Success = 1,
        AreTrading = 2,
    }
}