namespace Heavysoft.TimesheetParser.PluginInterfaces
{
    public class JobDefinition
    {
        public int TaskId { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Duration in minutes.
        /// </summary>
        public int Duration { get; set; }
    }
}
