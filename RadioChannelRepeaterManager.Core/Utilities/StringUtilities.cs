namespace RadioChannelRepeaterManager.Core.Utilities;

public static class StringUtilities
{
    /// <summary>
    /// Determines whether a string contains only printable characters.
    /// </summary>
    /// <param name="value">The string to evaluate.</param>
    /// <returns><see langword="true"/> if the string contains no control characters; otherwise, <see langword="false"/>.</returns>
    public static bool IsPrintableString(string value)
    {
        foreach (char c in value)
        {
            if (char.IsControl(c))
            {
                return false;
            }
        }
        return true;
    }
}
