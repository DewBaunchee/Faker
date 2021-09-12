using System.Text;

namespace App.Example
{
    public static class LittleFormatter
    {
        public static string Format(string unformatted)
        {
            if (unformatted == null)
                return null;

            StringBuilder formatted = new StringBuilder();

            char[] chars = unformatted.ToCharArray();
            int level = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                // Pass string literal...
                if (chars[i] == '\'')
                {
                    formatted.Append(chars[i++]);
                    while (chars[i] != '\'')
                        formatted.Append(chars[i++]);
                    formatted.Append(chars[i++]);
                }

                // ...and then continue
                if (IsAppendingLevel(chars[i]))
                    level++;
                if (IsDependingLevel(chars[i]))
                    level--;

                if (NeedNewLineBefore(chars[i]))
                    formatted.Append("\n").Append(MultipleChars(' ', 4 * level));

                formatted.Append(chars[i]);

                if (NeedNewLineAfter(chars[i]))
                    formatted.Append("\n").Append(MultipleChars(' ', 4 * level));
            }

            return formatted.ToString();
        }

        private static bool IsAppendingLevel(char c)
        {
            return "(".Contains(c);
        }

        private static bool IsDependingLevel(char c)
        {
            return ")".Contains(c);
        }

        private static bool NeedNewLineBefore(char c)
        {
            return ")".Contains(c);
        }

        private static bool NeedNewLineAfter(char c)
        {
            return "(,".Contains(c);
        }

        private static string MultipleChars(char c, int count)
        {
            string result = "";
            while (count-- > 0)
                result += c;
            return result;
        }
    }
}