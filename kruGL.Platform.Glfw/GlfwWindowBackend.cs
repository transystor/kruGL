using Silk.NET.Maths;
using Silk.NET.Windowing;
using kruGL.Native;
using kruGL.OpenGL;
using kruGL.Platform;

namespace kruGL.Platform.Glfw;

public sealed class GlfwWindowBackend : IWindowBackend
{
    private readonly IWindow _window;

    public GlfwWindowBackend(WindowSettings settings)
    {
        var options = WindowOptions.Default;
        options.Title = settings.Title;
        options.Size = new Vector2D<int>(settings.Width, settings.Height);

        _window = Window.Create(options);
        _window.Load += () => Load?.Invoke();
        _window.Resize += size => Resize?.Invoke(size.X, size.Y);
        _window.Render += delta => Render?.Invoke(delta);
    }

    public int Width => _window.Size.X;
    public int Height => _window.Size.Y;
    public bool ShouldClose => _window.IsClosing;

    public event Action? Load;
    public event Action<int, int>? Resize;
    public event Action<double>? Render;

    public void Run() => _window.Run();

    public GlApi CreateGlApi() => new(this);

    public nint LoadFunctionPointer(string functionName)
    {
        if (_window.GLContext is null || !_window.GLContext.TryGetProcAddress(functionName, out var address) || address == nint.Zero)
        {
            throw new NativeFunctionNotFoundException(functionName);
        }

        return address;
    }

    public void Dispose() => _window.Dispose();
}
