using System.Runtime.InteropServices;
using System.Text;
using kruGL.Native;

namespace kruGL.OpenGL;

/// <summary>
/// Главная низкоуровневая точка входа в OpenGL внутри библиотеки.
///
/// Этот класс не создаёт окно и не поднимает context сам по себе.
/// Он предполагает, что снаружи уже есть активный GL context и механизм,
/// который умеет отдавать адреса native GL-функций.
///
/// Задача <see cref="GlApi"/>, это собрать эти адреса в типизированный C# API:
/// buffers, shaders, programs, vertex arrays, viewport, clear и draw calls.
/// </summary>
public sealed class GlApi
{
    private readonly GlGenBuffersDelegate _genBuffers;
    private readonly GlDeleteBuffersDelegate _deleteBuffers;
    private readonly GlGenVertexArraysDelegate _genVertexArrays;
    private readonly GlDeleteVertexArraysDelegate _deleteVertexArrays;
    private readonly GlBindVertexArrayDelegate _bindVertexArray;
    private readonly GlBindBufferDelegate _bindBuffer;
    private readonly GlBufferDataDelegate _bufferData;
    private readonly GlClearColorDelegate _clearColor;
    private readonly GlClearDelegate _clear;
    private readonly GlViewportDelegate _viewport;
    private readonly GlCreateShaderDelegate _createShader;
    private readonly GlDeleteShaderDelegate _deleteShader;
    private readonly GlShaderSourceDelegate _shaderSource;
    private readonly GlCompileShaderDelegate _compileShader;
    private readonly GlGetShaderIvDelegate _getShaderIv;
    private readonly GlGetShaderInfoLogDelegate _getShaderInfoLog;
    private readonly GlCreateProgramDelegate _createProgram;
    private readonly GlDeleteProgramDelegate _deleteProgram;
    private readonly GlAttachShaderDelegate _attachShader;
    private readonly GlLinkProgramDelegate _linkProgram;
    private readonly GlGetProgramIvDelegate _getProgramIv;
    private readonly GlGetProgramInfoLogDelegate _getProgramInfoLog;
    private readonly GlUseProgramDelegate _useProgram;
    private readonly GlEnableVertexAttribArrayDelegate _enableVertexAttribArray;
    private readonly GlVertexAttribPointerDelegate _vertexAttribPointer;
    private readonly GlDrawArraysDelegate _drawArrays;

    public GlApi(INativeFunctionLoader loader)
    {
        _genBuffers = loader.LoadDelegate<GlGenBuffersDelegate>("glGenBuffers");
        _deleteBuffers = loader.LoadDelegate<GlDeleteBuffersDelegate>("glDeleteBuffers");
        _genVertexArrays = loader.LoadDelegate<GlGenVertexArraysDelegate>("glGenVertexArrays");
        _deleteVertexArrays = loader.LoadDelegate<GlDeleteVertexArraysDelegate>("glDeleteVertexArrays");
        _bindVertexArray = loader.LoadDelegate<GlBindVertexArrayDelegate>("glBindVertexArray");
        _bindBuffer = loader.LoadDelegate<GlBindBufferDelegate>("glBindBuffer");
        _bufferData = loader.LoadDelegate<GlBufferDataDelegate>("glBufferData");
        _clearColor = loader.LoadDelegate<GlClearColorDelegate>("glClearColor");
        _clear = loader.LoadDelegate<GlClearDelegate>("glClear");
        _viewport = loader.LoadDelegate<GlViewportDelegate>("glViewport");
        _createShader = loader.LoadDelegate<GlCreateShaderDelegate>("glCreateShader");
        _deleteShader = loader.LoadDelegate<GlDeleteShaderDelegate>("glDeleteShader");
        _shaderSource = loader.LoadDelegate<GlShaderSourceDelegate>("glShaderSource");
        _compileShader = loader.LoadDelegate<GlCompileShaderDelegate>("glCompileShader");
        _getShaderIv = loader.LoadDelegate<GlGetShaderIvDelegate>("glGetShaderiv");
        _getShaderInfoLog = loader.LoadDelegate<GlGetShaderInfoLogDelegate>("glGetShaderInfoLog");
        _createProgram = loader.LoadDelegate<GlCreateProgramDelegate>("glCreateProgram");
        _deleteProgram = loader.LoadDelegate<GlDeleteProgramDelegate>("glDeleteProgram");
        _attachShader = loader.LoadDelegate<GlAttachShaderDelegate>("glAttachShader");
        _linkProgram = loader.LoadDelegate<GlLinkProgramDelegate>("glLinkProgram");
        _getProgramIv = loader.LoadDelegate<GlGetProgramIvDelegate>("glGetProgramiv");
        _getProgramInfoLog = loader.LoadDelegate<GlGetProgramInfoLogDelegate>("glGetProgramInfoLog");
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

    public void DeleteBuffer(GlBufferHandle handle)
    {
        var value = handle.Value;
        _deleteBuffers(1, ref value);
    }

    public GlVertexArrayHandle CreateVertexArray()
    {
        uint handle = 0;
        _genVertexArrays(1, ref handle);
        return new GlVertexArrayHandle(handle);
    }

    public void DeleteVertexArray(GlVertexArrayHandle handle)
    {
        var value = handle.Value;
        _deleteVertexArrays(1, ref value);
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

    public void DeleteShader(GlShaderHandle shader) => _deleteShader(shader.Value);

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

    public bool GetShaderCompileStatus(GlShaderHandle shader)
    {
        _getShaderIv(shader.Value, (uint)GlShaderParameterName.CompileStatus, out var value);
        return value != 0;
    }

    public string GetShaderInfoLog(GlShaderHandle shader)
    {
        _getShaderIv(shader.Value, (uint)GlShaderParameterName.InfoLogLength, out var length);
        if (length <= 1)
        {
            return string.Empty;
        }

        var buffer = new byte[length];
        _getShaderInfoLog(shader.Value, length, out var written, buffer);
        return Encoding.UTF8.GetString(buffer, 0, Math.Max(0, written)).TrimEnd('\0');
    }

    public void CompileShaderChecked(GlShaderHandle shader)
    {
        CompileShader(shader);
        if (!GetShaderCompileStatus(shader))
        {
            throw new GlCompileException(GetShaderInfoLog(shader));
        }
    }

    public GlProgramHandle CreateProgram() => new(_createProgram());

    public void DeleteProgram(GlProgramHandle program) => _deleteProgram(program.Value);

    public void AttachShader(GlProgramHandle program, GlShaderHandle shader) => _attachShader(program.Value, shader.Value);

    public void LinkProgram(GlProgramHandle program) => _linkProgram(program.Value);

    public bool GetProgramLinkStatus(GlProgramHandle program)
    {
        _getProgramIv(program.Value, (uint)GlProgramParameterName.LinkStatus, out var value);
        return value != 0;
    }

    public string GetProgramInfoLog(GlProgramHandle program)
    {
        _getProgramIv(program.Value, (uint)GlProgramParameterName.InfoLogLength, out var length);
        if (length <= 1)
        {
            return string.Empty;
        }

        var buffer = new byte[length];
        _getProgramInfoLog(program.Value, length, out var written, buffer);
        return Encoding.UTF8.GetString(buffer, 0, Math.Max(0, written)).TrimEnd('\0');
    }

    public void LinkProgramChecked(GlProgramHandle program)
    {
        LinkProgram(program);
        if (!GetProgramLinkStatus(program))
        {
            throw new GlLinkException(GetProgramInfoLog(program));
        }
    }

    public void UseProgram(GlProgramHandle program) => _useProgram(program.Value);

    public void EnableVertexAttribArray(uint index) => _enableVertexAttribArray(index);

    public void VertexAttribPointer(uint index, int size, GlDataType type, bool normalized, int stride, nint offset)
        => _vertexAttribPointer(index, size, (uint)type, normalized, stride, offset);

    public void DrawArrays(GlPrimitiveType primitiveType, int first, int count) => _drawArrays((uint)primitiveType, first, count);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGenBuffersDelegate(uint count, ref uint buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlDeleteBuffersDelegate(uint count, ref uint buffers);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGenVertexArraysDelegate(uint count, ref uint arrays);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlDeleteVertexArraysDelegate(uint count, ref uint arrays);

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
    private delegate void GlDeleteShaderDelegate(uint shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlShaderSourceDelegate(uint shader, int count, nint strings, ref int length);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlCompileShaderDelegate(uint shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGetShaderIvDelegate(uint shader, uint parameterName, out int value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGetShaderInfoLogDelegate(uint shader, int maxLength, out int length, byte[] infoLog);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate uint GlCreateProgramDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlDeleteProgramDelegate(uint program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlAttachShaderDelegate(uint program, uint shader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlLinkProgramDelegate(uint program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGetProgramIvDelegate(uint program, uint parameterName, out int value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlGetProgramInfoLogDelegate(uint program, int maxLength, out int length, byte[] infoLog);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlUseProgramDelegate(uint program);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlEnableVertexAttribArrayDelegate(uint index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlVertexAttribPointerDelegate(uint index, int size, uint type, bool normalized, int stride, nint pointer);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void GlDrawArraysDelegate(uint mode, int first, int count);
}
