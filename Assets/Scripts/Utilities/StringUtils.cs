namespace Utilities
{
    public static class StringUtils
    {
        public static string Ellipsize(string str, int length = 6)
        {
            if (str.Length <= length * 2 + 3) return str;
            return str[..length] + "..." + str[^length..];
        }
    }
}