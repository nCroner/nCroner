using FluentValidation;
using Newtonsoft.Json;

namespace Application.Models.CronJobs
{
    public class CronJobModel
    {
        [JsonIgnore]
        public string FileName { get; set; }
        
        public Guid Id { get; set; }
        public string Title { get; set; }
        //public Guid? SystemEventId { get; set; }
        public bool Enabled { get; set; }
        public Dictionary<string,object>? EventInput { get; set; }
        public CronJobDataModel Event { get; set; }
        public List<CronJobActionModel> Actions { get; set; }
        //public string EventRoute { get; set; }
    }

    public class CronJobModelValidator : AbstractValidator<CronJobModel>
    {
        public CronJobModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Event).NotNull();
            RuleFor(x => x.Actions).NotNull();
        }
    }
}