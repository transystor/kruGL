namespace kruGL.Core;

public static class Guard
{
    public static void AgainstNull<T>(T? value, string paramName) where T : class
    {
        if (value is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }
}
