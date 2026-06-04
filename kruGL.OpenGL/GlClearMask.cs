namespace kruGL.OpenGL;

/// <summary>
/// Флаги для операции очистки framebuffer-а.
/// Позволяют указать, какие именно буферы нужно очищать: цвет, глубину, stencil.
/// </summary>
[Flags]
public enum GlClearMask : uint
{
    ColorBufferBit = 0x00004000,
    DepthBufferBit = 0x00000100,
    StencilBufferBit = 0x00000400
}
