using System;

namespace DBCommon
{
    public interface ICommandAble
    {
        string Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime UpdatedDateTime { get; set; }
        string UpdatedUserName { get; set; }
        DateTime BasedOnUpdatedDateTime { get; set; }
    }
}
