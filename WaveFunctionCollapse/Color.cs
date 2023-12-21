namespace WaveFunctionCollapse;

public struct Color
{
    // Imported as BRGA32
    public uint Data
    {
        get => _data;
        set
        {
            _data = value;
        }
    }
    private uint _data;

    public Color()
    {
        _data = 0x00FFFF00;
    }
    public Color(uint data)
    {
        _data = data;
    }
    public Color(byte _a, byte _r, byte _g, byte _b)
    {
        _data = (uint)((_b << 0) | (_g << 8) | (_r << 16) | (_a << 24));
    }

    public byte B => (byte)((_data & 0x000000FF) >> 0);
    public byte G => (byte)((_data & 0x0000FF00) >> 8);
    public byte R => (byte)((_data & 0x00FF0000) >> 16);
    public byte A => (byte)((_data & 0xFF000000) >> 24);
    public uint ARGB => (uint)((A << 24) | (R << 16) | (G << 8) | (B << 0));

    public bool Matches(Color rhs)
    {
        //if (isWildcard || rhs.isWildcard) return true;
        return (_data - rhs._data) == 0;
    }
}