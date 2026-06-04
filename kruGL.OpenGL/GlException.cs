namespace kruGL.OpenGL;

/// <summary>
/// Базовое исключение OpenGL-слоя библиотеки.
/// От него наследуются более конкретные ошибки, например compile/link failures.
/// </summary>
public class GlException : Exception
{
    public GlException(string message) : base(message)
    {
    }
}
