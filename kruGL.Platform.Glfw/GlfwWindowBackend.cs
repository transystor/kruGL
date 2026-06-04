using GLFW;
using kruGL.Native;
using kruGL.OpenGL;
using kruGL.Platform;

namespace kruGL.Platform.Glfw;

public sealed class GlfwWindowBackend : IWindowBackend
{
    private readonly Window _window;
    private readonly SizeCallback _framebufferSizeCallback;

    public GlfwWindowBackend(WindowSettings settings)
    {
        if (!global::GLFW.Glfw.Init())
        {
            throw new InvalidOperationException("GLFW initialization failed.");
        }

        global::GLFW.Glfw.DefaultWindowHints();
        global::GLFW.Glfw.WindowHint(Hint.ClientApi, ClientApi.OpenGL);
        global::GLFW.Glfw.WindowHint(Hint.ContextVersionMajor, 3);
        global::GLFW.Glfw.WindowHint(Hint.ContextVersionMinor, 3);
        global::GLFW.Glfw.WindowHint(Hint.OpenglProfile, Profile.Core);
        global::GLFW.Glfw.WindowHint(Hint.Visible, true);
        global::GLFW.Glfw.WindowHint(Hint.Resizable, true);
        global::GLFW.Glfw.WindowHint(Hint.Doublebuffer, true);

        _window = global::GLFW.Glfw.CreateWindow(settings.Width, settings.Height, settings.Title, global::GLFW.Monitor.None, Window.None);
        if (_window == Window.None)
        {
            global::GLFW.Glfw.Terminate();
            throw new InvalidOperationException("GLFW window creation failed.");
        }

        global::GLFW.Glfw.MakeContextCurrent(_window);
        global::GLFW.Glfw.SwapInterval(1);

        _framebufferSizeCallback = (_, width, height) => Resize?.Invoke(width, height);
        global::GLFW.Glfw.SetFramebufferSizeCallback(_window, _framebufferSizeCallback);
    }

    public int Width
    {
        get
        {
            global::GLFW.Glfw.GetFramebufferSize(_window, out var width, out _);
            return width;
        }
    }

    public int Height
    {
        get
        {
            global::GLFW.Glfw.GetFramebufferSize(_window, out _, out var height);
            return height;
        }
    }

    public bool ShouldClose => global::GLFW.Glfw.WindowShouldClose(_window);

    public event Action? Load;
    public event Action<int, int>? Resize;
    public event Action<double>? Render;

    public void Run()
    {
        Load?.Invoke();

        var lastTime = global::GLFW.Glfw.Time;
        while (!ShouldClose)
        {
            var now = global::GLFW.Glfw.Time;
            var delta = now - lastTime;
            lastTime = now;

            Render?.Invoke(delta);
            global::GLFW.Glfw.SwapBuffers(_window);
            global::GLFW.Glfw.PollEvents();
        }
    }

    public GlApi CreateGlApi() => new(this);

    public nint LoadFunctionPointer(string functionName)
    {
        var address = global::GLFW.Glfw.GetProcAddress(functionName);
        if (address == nint.Zero)
        {
            throw new NativeFunctionNotFoundException(functionName);
        }

        return address;
    }

    public void Dispose()
    {
        global::GLFW.Glfw.DestroyWindow(_window);
        global::GLFW.Glfw.Terminate();
    }
}
