namespace kruGL.OpenGL;

/// <summary>
/// Типобезопасная оболочка над OpenGL buffer handle.
/// Нужна, чтобы в коде не таскать голые <c>uint</c> без контекста.
/// </summary>
public readonly record struct GlBufferHandle(uint Value);
