namespace kruGL.OpenGL;

/// <summary>
/// Целевой binding slot для OpenGL buffer-ов.
/// Определяет, в какой именно тип конвейера будет привязан buffer.
/// </summary>
public enum GlBufferTarget : uint
{
    ArrayBuffer = 0x8892,
    ElementArrayBuffer = 0x8893
}
