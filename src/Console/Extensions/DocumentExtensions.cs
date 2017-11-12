using AngleSharp.Dom;

namespace Console.Extensions
{
    public static class DocumentExtensions
    {
        public static string GetContent(this IDocument jobPage, string selector)
        {
            var element = jobPage.QuerySelector(selector);

            return element == null ? string.Empty : element.TextContent;
        }
    }
}
