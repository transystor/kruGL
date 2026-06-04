namespace kruGL.OpenGL;

/// <summary>
/// Имена параметров shader-объекта, которые можно спросить у OpenGL.
/// Используются для проверки compile status и получения длины shader info log.
/// </summary>
public enum GlShaderParameterName : uint
{
    CompileStatus = 0x8B81,
    InfoLogLength = 0x8B84
}
