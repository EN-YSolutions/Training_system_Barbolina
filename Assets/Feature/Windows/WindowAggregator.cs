using System.Collections.Generic;

public static class WindowAggregator
{
    private static Stack<BaseWindow> _windows = new();

    public static void Open(BaseWindow window)
    {
        if(_windows.Count > 0)
        {
            _windows.Peek().Close();
        }

        window.Open();
        _windows.Push(window);
    }

    public static void Close()
    {
        _windows.Pop().Close();
        if(_windows.Count > 0)
        {
            _windows.Peek().Open();
        }
    }

    public static void Clear()
    {
        _windows = new();
    }
}
