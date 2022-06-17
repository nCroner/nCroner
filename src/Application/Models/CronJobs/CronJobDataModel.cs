namespace Application.Models.CronJobs
{
    public class CronJobDataModel
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> Input { get; set; }
    }
}