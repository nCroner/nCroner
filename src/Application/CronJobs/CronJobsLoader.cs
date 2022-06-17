using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Application.Models.CronJobs;
using Application.Plugins;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.CronJobs
{
    public class CronJobsLoader
    {
        /*
        
        JobManager.AddJob(async () =>
        {
            await Task.CompletedTask;
        },schedule => schedule.ToRunEvery(10));
        
         */

        private readonly IPluginsLoader _loader;
        private readonly ILogger<CronJobsLoader> _logger;
        private readonly IValidator<CronJobModel> _validator;

        public CronJobsLoader(IPluginsLoader loader, ILogger<CronJobsLoader> logger,
            IValidator<CronJobModel> validator)
        {
            _loader = loader;
            _logger = logger;
            _validator = validator;
        }

        public void Load()
        {
            _logger.LogInformation("Loading plugins...");

            var cronJobFiles = GetCronJobs();

            foreach (var cron in cronJobFiles)
            {
                var cronFullPath = $"{Directory.GetCurrentDirectory()}\\{cron}";
                var cronJobData = LoadCronJob(cronFullPath);

                if (cronJobData is null)
                {
                    continue;
                }

                cronJobData.FileName = cron;

                _logger.LogInformation($"Load cron job {cron}");

                ProcessCronJob(cronJobData);
            }
        }

        #region Private Methods

        private static IEnumerable<string> GetCronJobs()
        {
            var dllFiles = Directory.GetFiles("cronJobs", "*.json",
                SearchOption.AllDirectories);

            return dllFiles;
        }

        private static CronJobModel? LoadCronJob(string cronJobPath)
        {
            var content = File.ReadAllText(cronJobPath);

            return string.IsNullOrWhiteSpace(content)
                ? default
                : JsonSerializer.Deserialize<CronJobModel>(content);
        }

        private void ProcessCronJob(CronJobModel cronJob)
        {
            var validateRes = _validator.Validate(cronJob);
            if (!validateRes.IsValid)
            {
                return;
            }

            if (!cronJob.Enabled)
            {
                return;
            }            
            
            var trigger = _loader.GetTrigger(cronJob.Event.Id);
            if (trigger is null)
            {
                return;
            }

            var triggersIsValid = ValidateCronJobActions(cronJob);
            if (!triggersIsValid)
            {
                return;
            }

            //_loader.GetTrigger(cronJob.Event.Id)
        }

        private bool ValidateCronJobActions(CronJobModel cronJob)
        {
            foreach (var action in cronJob.Actions)
            {
                var actionData = _loader.GetAction(action.Id);
                if (actionData is null)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}