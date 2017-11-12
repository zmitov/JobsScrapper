using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Extensions;
using Console.Extensions;

namespace Console.JobsBgProvider
{
    public class LinkProvider
    {
        private readonly IBrowsingContext _context;
        public const int JobsPerPage = 15;

        public LinkProvider()
            : this(BrowsingContext.New(Configuration.Default.WithDefaultLoader()))
        {

        }

        public LinkProvider(IBrowsingContext context)
        {
            this._context = context;
        }

        public List<string> GetJobUrls()
        {
            return Enumerable.Range(0, GetPages(_context))
                .AsParallel()
                .WithDegreeOfParallelism(Concurrency.DegreeOfNetworkParallelism)
                .Select(GetPageUrl)
                .Select(p => GetJobLinksInPage(_context, p))
                .SelectMany(e => e)
                .ToList();
        }

        private static int GetPages(IBrowsingContext context)
        {
            const string jobCountSelector = "#search_results_div > table > tbody > tr > td > table > tbody > tr:nth-child(3) > td:nth-child(1)";

            var document = context.OpenAsync(GetPageUrl(0)).Result;
            var jobsOnPage = document.QuerySelector(jobCountSelector).TextContent;
            var jobs = int.Parse(Regex.Matches(jobsOnPage, @"\d+").Cast<Match>().Last().Value);
            var pages = (int) Math.Ceiling(jobs / (double) JobsPerPage);

            return pages;
        }

        public static string GetPageUrl(int page)
        {
            const string pageableListUrlFormat = "http://www.jobs.bg/front_job_search.php?frompage={0}&all_cities=0&categories%5B%5D=15&all_type=0&all_position_level=1&all_company_type=1&keyword=#paging";
            var url = string.Format(pageableListUrlFormat, page * JobsPerPage);
            return url;
        }

        public static List<string> GetJobLinksInPage(IBrowsingContext context, string pageUrl)
        {
            var document = context.OpenAsync(pageUrl).Result;

            var links = document.QuerySelectorAll<IHtmlAnchorElement>(".joblink")
                .Select(x => x.Href)
                .ToList();

            return links;
        }
    }
}
