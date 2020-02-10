namespace DBCommon
{
    public interface ISerializer
    {
        ICommandAble DesSer(string cmmdStr);

        string Ser(ICommandAble cmmd);
    }
}
