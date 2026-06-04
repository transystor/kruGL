namespace kruGL.OpenGL;

/// <summary>
/// Типы данных, которые OpenGL ожидает в buffer-ах и vertex attributes.
/// Используются, например, при описании layout-а вершины.
/// </summary>
public enum GlDataType : uint
{
    Float = 0x1406,
    UnsignedInt = 0x1405,
    UnsignedByte = 0x1401
}
