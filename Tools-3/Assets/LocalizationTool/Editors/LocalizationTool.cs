public static class LocalizationTool
{

    public static string SubstringField(this string text, int length)
    {
        if (text.Length > length)
        {
            return text.Substring(0, length) + "...";
        }
        else
        {
            return text;
        }
    }
}
