using System.Text;

public static class GlobalTypeIdentitation
{
    public static string Load(string str, int key)
    {
        StringBuilder builder = new StringBuilder();
        foreach (char c in str.ToCharArray())
            builder.Append((char)(c + key));
        return builder.ToString();
    }
}
