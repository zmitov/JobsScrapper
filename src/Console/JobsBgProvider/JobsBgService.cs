using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using Console.Extensions;
using Console.Models;

namespace Console.JobsBgProvider
{
    public class JobsBgService
    {
        private readonly JobScrapper _jobScrapper;
        private readonly LinkProvider _linkProvider;

        public JobsBgService()
        {
            var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            this._linkProvider = new LinkProvider(context);
            this._jobScrapper = new JobScrapper(context);
        }

        public List<Job> GetJobs()
        {
            var jobs = _linkProvider
                .GetJobUrls()
                .AsParallel()
                .WithDegreeOfParallelism(Concurrency.DegreeOfNetworkParallelism)
                .Select(_jobScrapper.ScrapeJob)
                .ToList();

            return jobs;
        }
    }
}
