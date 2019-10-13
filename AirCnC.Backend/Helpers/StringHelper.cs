using System.Collections.Generic;

namespace AirCnC.Backend.Helpers
{
    public static class StringHelper
    {
        public static IEnumerable<string> GetWords(this string text, char separator = ',')
        {
            foreach (var tech in text.Split(separator))
            {
                yield return tech.Trim();
            }
        }
    }
}
