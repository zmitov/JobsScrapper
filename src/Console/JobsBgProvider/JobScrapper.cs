using System;
using AngleSharp;
using Console.Extensions;
using Console.Models;

namespace Console.JobsBgProvider
{
    public class JobScrapper
    {
        private readonly IBrowsingContext _context;

        public JobScrapper()
            : this(BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
        {

        }

        public JobScrapper(IBrowsingContext context)
        {
            this._context = context;
        }

        public Job ScrapeJob(string url)
        {
            var jobPage = _context.OpenAsync(url).Result;
            var id = long.Parse(url.Substring(url.LastIndexOf("/", StringComparison.Ordinal) + 1));

            var salary = jobPage.GetContent(
                    "body > table:nth-child(3) > tbody > tr > td > table > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(3) > td > span");

            var title = jobPage.GetContent(
                "body > table:nth-child(3) > tbody > tr > td > table > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(2) > td > b");

            var company = jobPage.GetContent(
                "body > table:nth-child(3) > tbody > tr > td > table > tbody > tr:nth-child(2) > td > table > tbody > tr:nth-child(2) > td > a");

            var description = jobPage.GetContent(
                                  "body > table:nth-child(3) > tbody > tr > td > table > tbody > tr:nth-child(5) > td") +
                              jobPage.GetContent(
                                  "body > table:nth-child(3) > tbody > tr > td > table:nth-child(2) > tbody > tr:nth-child(4)");

            return new Job
            {
                Company = company,
                Description = description,
                Salary = salary,
                Title = title,
                Id = id,
                Created = DateTime.Now
            };
        }

       


    }
}
