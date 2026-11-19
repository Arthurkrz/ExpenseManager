using AngleSharp;
using AngleSharp.Dom;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public static class HtmlParser
    {
        public static async Task<IDocument> ParseAsync(HttpResponseMessage response)
        {
            var html = await response.Content.ReadAsStringAsync();

            var context = BrowsingContext.New(Configuration.Default);

            return await context.OpenAsync(
                request => request.Content(html));
        }
    }
}
