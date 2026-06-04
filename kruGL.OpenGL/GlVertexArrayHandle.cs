namespace kruGL.OpenGL;

/// <summary>
/// Типобезопасная оболочка над Vertex Array Object.
/// Через VAO OpenGL хранит описание vertex input layout и связанные buffer bindings.
/// </summary>
public readonly record struct GlVertexArrayHandle(uint Value);
