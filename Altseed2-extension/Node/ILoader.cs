namespace Altseed2Extension.Node
{
    public interface ILoader
    {
        (int taskCount, int progress) ProgressInfo { get; set; }
    }
}