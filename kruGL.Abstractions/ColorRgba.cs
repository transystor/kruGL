namespace kruGL.Abstractions;

/// <summary>
/// Простейшее представление цвета в формате RGBA.
///
/// Это базовый value-type для мест, где нужен цвет без привязки
/// к конкретной graphics-реализации или формату текстуры.
/// </summary>
public readonly record struct ColorRgba(byte R, byte G, byte B, byte A = 255)
{
    public static readonly ColorRgba Black = new(0, 0, 0, 255);
    public static readonly ColorRgba White = new(255, 255, 255, 255);
}
