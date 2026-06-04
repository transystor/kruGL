using kruGL.Abstractions;

namespace kruGL.OpenGL;

public sealed class GlRenderDevice : IRenderDevice
{
    public void Clear(ColorRgba color)
    {
        // Реальная OpenGL реализация появится после выбора низкоуровневого binding/context layer.
    }
}
