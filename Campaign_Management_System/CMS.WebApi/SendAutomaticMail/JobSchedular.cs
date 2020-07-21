using Quartz;
using Quartz.Impl;

namespace CMS.BL.Manager.SendAutomaticMail
{
    public class JobSchedular
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            IJobDetail job = JobBuilder.Create<SendEmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(2)
            .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
