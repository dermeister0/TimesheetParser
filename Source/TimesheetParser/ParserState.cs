namespace TimesheetParser
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