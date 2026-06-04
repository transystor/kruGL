namespace kruGL.OpenGL;

/// <summary>
/// Типобезопасная оболочка над OpenGL program handle.
/// Программа получается после успешной линковки shader-ов и потом используется в draw path.
/// </summary>
public readonly record struct GlProgramHandle(uint Value);
