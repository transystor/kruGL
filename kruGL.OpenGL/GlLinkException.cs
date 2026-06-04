namespace kruGL.OpenGL;

/// <summary>
/// Ошибка линковки shader program.
/// Обычно означает, что vertex/fragment shader не состыковались,
/// или драйвер не смог собрать корректную программу.
/// </summary>
public sealed class GlLinkException : GlException
{
    public GlLinkException(string message) : base(message)
    {
    }
}
