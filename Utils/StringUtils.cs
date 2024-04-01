using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NodeBlock.Engine.Utils
{
    public class StringUtils
    {
        public static List<string> ExtractTextWithinDoubleCurlyBraces(string input)
        {
            List<string> extractedText = new List<string>();

            // Regular expression pattern to match "{{any_text}}"
            string pattern = @"\{\{(.+?)\}\}";

            // Use Regex to find matches in the input string
            MatchCollection matches = Regex.Matches(input, pattern);

            // Iterate through the matches and add them to the list
            foreach (Match match in matches)
            {
                extractedText.Add(match.Groups[1].Value);
            }

            return extractedText;
        }
    }
}
