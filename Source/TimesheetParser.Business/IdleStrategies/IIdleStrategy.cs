namespace TimesheetParser.Business.IdleStrategies
{
    public interface IIdleStrategy
    {
        void DistributeIdle(ParseResult result);
    }
}
