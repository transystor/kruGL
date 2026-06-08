# kruGL

Тонкая C#-библиотека для practical OpenGL-wrapper под нужды игры и связанных тулов.


## Текущая структура

- `kruGL.Abstractions` — базовые интерфейсы и типы
- `kruGL.Core` — общие utility/helper pieces
- `kruGL.Native` — загрузка native function pointers
- `kruGL.OpenGL` — low-level OpenGL API surface
- `kruGL.Platform` — platform abstractions
- `kruGL.Platform.Glfw` — текущая platform-реализация окна и context bootstrap через прямой GLFW .NET binding

Для Windows native GLFW DLL теперь ожидается в репозитории по пути:
- `native/windows/x64/glfw.dll`

Дальше project сам копирует этот файл в output рядом с исполняемыми бинарниками.

## V0 surface

Первая версия целится в минимальный набор, достаточный для:

- `ClearColor`
- `Clear`
- `Viewport`
- buffers + delete
- vertex arrays + delete
- shaders / programs
- compile/link status
- info logs
- checked compile/link helpers
- `DrawArrays`

## Smoke test
- создать окно
- поднять GL context
- достать адреса GL-функций из context

При этом GLFW-специфика теперь вынесена из общего platform API, чтобы позже можно было допилить и подставить альтернативную platform-реализацию без ломки верхнего слоя.
