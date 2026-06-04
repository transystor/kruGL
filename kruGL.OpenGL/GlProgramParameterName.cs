namespace kruGL.OpenGL;

/// <summary>
/// Имена параметров shader program, которые можно запросить у драйвера OpenGL.
/// Нужны, например, чтобы проверить успешность линковки или узнать длину info log.
/// </summary>
public enum GlProgramParameterName : uint
{
    LinkStatus = 0x8B82,
    InfoLogLength = 0x8B84
}
