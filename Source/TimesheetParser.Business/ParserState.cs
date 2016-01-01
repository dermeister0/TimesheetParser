namespace TimesheetParser.Business
{
    enum ParserState
    {
        Begin,
        StartTimeFound,
        TaskFound,
        DescriptionFound,
        EndTimeFound,
    }
}