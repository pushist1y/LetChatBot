using System.Net;
using System.Text.RegularExpressions;

namespace LetChatBot
{
    public static class LetChatBotStringExtensions
    {
        public static string ExtractLinksFromHtml(this string input)
        {
            return Regex.Replace(input, "<a.*?href=\\\"(.*?)\\\".*?>(.*?)<\\/a>", "$1");
        }

        public static string ExtractForumSmilesText(this string input)
        {
            return Regex.Replace(input, "<!--[^>]*>[^<]*<img[^>]*alt=\\\"(.*?)\\\"[^>]*>[^>]*-->", "$1");
        }

        public static string StripHtmlTags(this string input)
        {
            return Regex.Replace(input, "<[^>]*(>|$)", string.Empty);
        }

        public static string HtmlDecode(this string input)
        {
            return WebUtility.HtmlDecode(input);
        }

        public static string HtmlEncode(this string input)
        {
            return WebUtility.HtmlEncode(input);
        }

        public static string ConvertToTelegram(this string input)
        {
            return input
                    .ExtractLinksFromHtml()
                    .ExtractForumSmilesText()
                    .StripHtmlTags()
                    .HtmlDecode();
        }


        public static string ConvertToForum(this string input)
        {
            return input
                    .HtmlEncode();
        }

    }
}