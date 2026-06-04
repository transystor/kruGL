namespace kruGL.OpenGL;

/// <summary>
/// Ошибка компиляции shader-а.
/// Обычно в сообщении лежит info log, который вернул драйвер OpenGL.
/// </summary>
public sealed class GlCompileException : GlException
{
    public GlCompileException(string message) : base(message)
    {
    }
}
