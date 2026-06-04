namespace kruGL.OpenGL;

/// <summary>
/// Тип shader-а, который создаётся в OpenGL.
/// На текущем этапе библиотека использует vertex и fragment shader-ы как основу самого базового render path.
/// </summary>
public enum GlShaderType : uint
{
    VertexShader = 0x8B31,
    FragmentShader = 0x8B30
}
