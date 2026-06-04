namespace kruGL.Abstractions;

public readonly record struct ColorRgba(byte R, byte G, byte B, byte A = 255)
{
    public static readonly ColorRgba Black = new(0, 0, 0, 255);
    public static readonly ColorRgba White = new(255, 255, 255, 255);
}
