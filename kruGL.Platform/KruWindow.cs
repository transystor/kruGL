using Silk.NET.Maths;
using Silk.NET.Windowing;
using kruGL.Native;
using kruGL.OpenGL;

namespace kruGL.Platform;

public sealed class KruWindow : IDisposable
{
    private readonly IWindow _window;

    public KruWindow(WindowSettings settings)
    {
        var options = WindowOptions.Default;
        options.Title = settings.Title;
        options.Size = new Vector2D<int>(settings.Width, settings.Height);

        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Resize += OnResize;
        _window.Render += OnRender;
    }

    public GlApi? Gl { get; private set; }

    public event Action? Load;
    public event Action<int, int>? Resize;
    public event Action<double>? Render;

    public void Run() => _window.Run();

    public void Dispose() => _window.Dispose();

    private void OnLoad()
    {
        Gl = new GlApi(new SilkWindowFunctionLoader(_window));

        var size = _window.Size;
        Gl.Viewport(0, 0, size.X, size.Y);
        Load?.Invoke();
    }

    private void OnResize(Vector2D<int> size)
    {
        Gl?.Viewport(0, 0, size.X, size.Y);
        Resize?.Invoke(size.X, size.Y);
    }

    private void OnRender(double deltaTime)
    {
        Render?.Invoke(deltaTime);
    }

    private sealed class SilkWindowFunctionLoader : INativeFunctionLoader
    {
        private readonly IWindow _window;

        public SilkWindowFunctionLoader(IWindow window)
        {
            _window = window;
        }

        public nint LoadFunctionPointer(string functionName)
        {
            if (_window.GLContext is null || !_window.GLContext.TryGetProcAddress(functionName, out var address) || address == nint.Zero)
            {
                throw new NativeFunctionNotFoundException(functionName);
            }

            return address;
        }
    }
}
