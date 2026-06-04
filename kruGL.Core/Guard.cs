namespace kruGL.Core;

/// <summary>
/// Небольшой utility-класс для защитных проверок входных аргументов.
/// Это не ядро библиотеки, а просто общее место для базовых precondition checks.
/// </summary>
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
