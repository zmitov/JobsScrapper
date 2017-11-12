using Console.Data;
using Console.Extensions;
using Console.JobsBgProvider;
using Console.Models;

namespace Console
{
    public class Startup
    {
        public static void Main()
        {
            // scrape data
            var jobBgService = new JobsBgService();
            var jobs = jobBgService.GetJobs();

            // save data to db
            var context = new Context();

            jobs.ForEach(job => { context.Set<Job>().AddIfNotExists(job, x => x.Id == job.Id); });

            //context.Jobs.AddRange(jobs);
            context.SaveChanges();
        }
    }
}