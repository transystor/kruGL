using System.Runtime.InteropServices;
using System.Text;
using kruGL.Native;

namespace kruGL.OpenGL;

public sealed class GlApi
{
    private readonly GlGenBuffersDelegate _genBuffers;
    private readonly GlGenVertexArraysDelegate _genVertexArrays;
    private readonly GlBindVertexArrayDelegate _bindVertexArray;
    private readonly GlBindBufferDelegate _bindBuffer;
    private readonly GlBufferDataDelegate _bufferData;
    private readonly GlClearColorDelegate _clearColor;
    private readonly GlClearDelegate _clear;
    private readonly GlViewportDelegate _viewport;
    private readonly GlCreateShaderDelegate _createShader;
    private readonly GlShaderSourceDelegate _shaderSource;
    private readonly GlCompileShaderDelegate _compileShader;
    private readonly GlCreateProgramDelegate _createProgram;
    private readonly GlAttachShaderDelegate _attachShader;
    private readonly GlLinkProgramDelegate _linkProgram;
    private readonly GlUseProgramDelegate _useProgram;
    private readonly GlEnableVertexAttribArrayDelegate _enableVertexAttribArray;
    private readonly GlVertexAttribPointerDelegate _vertexAttribPointer;
    private readonly GlDrawArraysDelegate _drawArrays;

    public GlApi(INativeFunctionLoader loader)
    {
        _genBuffers = loader.LoadDelegate<GlGenBuffersDelegate>("glGenBuffers");
        _genVertexArrays = loader.LoadDelegate<GlGenVertexArraysDelegate>("glGenVertexArrays");
        _bindVertexArray = loader.LoadDelegate<GlBindVertexArrayDelegate>("glBindVertexArray");
        _bindBuffer = loader.LoadDelegate<GlBindBufferDelegate>("glBindBuffer");
        _bufferData = loader.LoadDelegate<GlBufferDataDelegate>("glBufferData");
        _clearColor = loader.LoadDelegate<GlClearColorDelegate>("glClearColor");
        _clear = loader.LoadDelegate<GlClearDelegate>("glClear");
        _viewport = loader.LoadDelegate<GlViewportDelegate>("glViewport");
        _createShader = loader.LoadDelegate<GlCreateShaderDelegate>("glCreateShader");
        _shaderSource = loader.LoadDelegate<GlShaderSourceDelegate>("glShaderSource");
        _compileShader = loader.LoadDelegate<GlCompileShaderDelegate>("glCompileShader");
        _createProgram = loader.LoadDelegate<GlCreateProgramDelegate>("glCreateProgram");
        _attachShader = loader.LoadDelegate<GlAttachShaderDelegate>("glAttachShader");
        _linkProgram = loader.LoadDelegate<GlLinkProgramDelegate>("glLinkProgram");
        _useProgram = loader.LoadDelegate<GlUseProgramDelegate>("glUseProgram");
        _enableVertexAttribArray = loader.LoadDelegate<GlEnableVertexAttribArrayDelegate>("glEnableVertexAttribArray");
        _vertexAttribPointer = loader.LoadDelegate<GlVertexAttribPointerDelegate>("glVertexAttribPointer");
        _drawArrays = loader.LoadDelegate<GlDrawArraysDelegate>("glDrawArrays");
    }

    public void ClearColor(float red, float green, float blue, float alpha) => _clearColor(red, green, blue, alpha);

    public void Clear(GlClearMask mask) => _clear((uint)mask);

    public void Viewport(int x, int y, int width, int height) => _viewport(x, y, width, height);

    public GlBufferHandle CreateBuffer()
    {
        uint handle = 0;
        _genBuffers(1, ref handle);
        return new GlBufferHandle(handle);
    }

    public GlVertexArrayHandle CreateVertexArray()
    {
        uint handle = 0;
        _genVertexArrays(1, ref handle);
        return new GlVertexArrayHandle(handle);
    }

    public void BindVertexArray(GlVertexArrayHandle handle) => _bindVertexArray(handle.Value);

    public void BindBuffer(GlBufferTarget target, GlBufferHandle handle) => _bindBuffer((uint)target, handle.Value);

    public void BufferData<T>(GlBufferTarget target, ReadOnlySpan<T> data, GlBufferUsage usage) where T : unmanaged
    {
        var dataSize = data.Length * Marshal.SizeOf<T>();
        var bytes = MemoryMarshal.AsBytes(data);
        var pinned = GCHandle.Alloc(bytes.ToArray(), GCHandleType.Pinned);

        try
        {
            _bufferData((uint)target, (nuint)dataSize, pinned.AddrOfPinnedObject(), (uint)usage);
        }
        finally
        {
            pinned.Free();
        }
    }

    public GlShaderHandle CreateShader(GlShaderType shaderType) => new(_createShader((uint)shaderType));

    public void ShaderSource(GlShaderHandle shader, string source)
    {
        var sourceBytes = Encoding.UTF8.GetBytes(source + "\0");
        var length = sourceBytes.Length - 1;
        var sourceHandle = GCHandle.Alloc(sourceBytes, GCHandleType.Pinned);
        var pointerArray = new[] { sourceHandle.AddrOfPinnedObject() };
        var pointerHandle = GCHandle.Alloc(pointerArray, GCHandleType.Pinned);

        try
        {
            _shaderSource(shader.Value, 1, pointerHandle.AddrOfPinnedObject(), ref length);
        }
        finally
        {
            pointerHandle.Free();
            sourceHandle.Free();
        }
    }

    public void CompileShader(GlShaderHandle shader) => _compileShader(shader.Value);

    public GlProgramHandle CreateProgram() => new(_createProgram());

    public void AttachShader(GlProgramHandle program, GlShaderHandle shader) => _attachShader(program.Value, shader.Value);

    public void LinkProgram(GlProgramHandle program) => _linkProgram(program.Value);

    public void UseProgram(GlProgramHandle program) => _useProgram(program.Value);

    public void EnableVertexAttribArray(uint index) => _enableVertexAttribArray(index);

    public void VertexAttribPointer(uint index, int size, GlDataType type, bool normalized, int stride, nint offset)
        => _vertexAttribPointer(index, size, (uint)type, normalized, stride, offset);

    public void DrawArrays(GlPrimitiveType primitiveType, int first, int count) => _drawArrays((uint)primitiveType, first, count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGenBuffersDelegate(uint count, ref uint buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGenVertexArraysDelegate(uint count, ref uint arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlBindVertexArrayDelegate(uint array);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlBindBufferDelegate(uint target, uint buffer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlBufferDataDelegate(uint target, nuint size, nint data, uint usage);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlClearColorDelegate(float red, float green, float blue, float alpha);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlClearDelegate(uint mask);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlViewportDelegate(int x, int y, int width, int height);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GlCreateShaderDelegate(uint shaderType);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlShaderSourceDelegate(uint shader, int count, nint strings, ref int length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlCompileShaderDelegate(uint shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GlCreateProgramDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlAttachShaderDelegate(uint program, uint shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlLinkProgramDelegate(uint program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlUseProgramDelegate(uint program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlEnableVertexAttribArrayDelegate(uint index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlVertexAttribPointerDelegate(uint index, int size, uint type, bool normalized, int stride, nint pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlDrawArraysDelegate(uint mode, int first, int count);
}
