namespace kruGL.OpenGL;

/// <summary>
/// Типобезопасная оболочка над OpenGL shader handle.
/// Используется для явной работы с vertex/fragment shader-ами до линковки в программу.
/// </summary>
public readonly record struct GlShaderHandle(uint Value);
