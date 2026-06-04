namespace kruGL.Native;

/// <summary>
/// Абстракция над источником native function pointers.
/// Нужна затем, чтобы <c>kruGL.OpenGL</c> не знал, откуда именно берутся адреса GL-функций:
/// из GLFW, из будущей собственной платформенной реализации или из любого другого context provider.
/// </summary>
public interface INativeFunctionLoader
{
    /// <summary>
    /// Возвращает адрес native-функции по её имени.
    /// Если функция недоступна в текущем context-е, реализация должна бросить исключение.
    /// </summary>
    nint LoadFunctionPointer(string functionName);
}
