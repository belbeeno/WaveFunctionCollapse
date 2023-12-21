using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WFCVisualizer;
internal class DirectBitmap : IDisposable
{
    public readonly Bitmap bitmap;

    private int[] _bits;
    private GCHandle _bitHandle;

    public int Width { get; private set; }
    public int Height { get; private set; }

    public DirectBitmap(int w, int h)
    {
        Width = w;
        Height = h;
        _bits = new int[w * h];
        _bitHandle = GCHandle.Alloc(_bits, GCHandleType.Pinned);
        bitmap = new Bitmap(w, h, w * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, _bitHandle.AddrOfPinnedObject());
    }

    public void Clear()
    {
        Array.Clear(_bits, 0, _bits.Length);
    }

    public void Set(int x, int y, WaveFunctionCollapse.Color color)
    {
        if (y * Width + x > _bits.Length)
        {
            //throw new IndexOutOfRangeException($"Attempting to access DirectBitmap index ({y * Width + x}) which is larger than the stored bits ({_bits.Length})");
            return;
        }
        _bits[y * Width + x] = (int)color.Data;
    }
    public WaveFunctionCollapse.Color Get(int x, int y)
    {
        if (y * Width + x > _bits.Length)
        {
            //throw new IndexOutOfRangeException($"Attempting to access DirectBitmap index ({y * Width + x}) which is larger than the stored bits ({_bits.Length})");
            return new();
        }
        return new WaveFunctionCollapse.Color((uint)_bits[y * Width + x]);
    }

    private bool _disposed = false;
    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        bitmap.Dispose();
        _bitHandle.Free();
    }
}
