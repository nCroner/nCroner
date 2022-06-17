namespace Application.Models.CronJobs
{
    public class CronJobActionModel
    {
        public Guid Id { get; set; }
        public Dictionary<string, object>? Input { get; set; }
        public string? Condition { get; set; }
    }
}