using System.Diagnostics;

namespace WaveFunctionCollapse
{
    public struct Kernel
    {
        public static Color EDGE_COLOR = new Color() { Data = 0x00FFFF00 };

        public readonly Color[,] data;
        
        public readonly int HalfWidth;
        public readonly int HalfHeight;

        public Kernel(int w, int h)
        {
            data = new Color[w, h];
            for (int y = 0; y < h; ++y)
            {
                for (int x = 0; x < w; ++x)
                {
                    data[x, y] = EDGE_COLOR;
                }
            }
        }
        
        public Kernel(ref Color[,] bitmap, int bmpOriginX, int bmpOriginY, int kernHalfWidth, int kernHalfHeight, int rotation, bool flipX, bool flipY)
        {
            rotation = (rotation % 4);

            HalfWidth = kernHalfWidth;
            HalfHeight = kernHalfHeight;
            int bmpWidth = bitmap.GetLength(0);
            int bmpHeight = bitmap.GetLength(1);
            int kernWidth = kernHalfWidth * 2 + 1;
            int kernHeight = kernHalfHeight * 2 + 1;

            data = new Color[kernWidth, kernHeight];
            for (int y = 0; y < kernHeight; ++y)
            {
                for (int x = 0; x < kernWidth; ++x)
                {
                    int tX = flipX ? kernWidth - x - 1 : x;
                    int tY = flipY ? kernHeight - y - 1 : y;
                    int bmpX = (((bmpOriginX - kernHalfWidth + tX) + bmpWidth) % bmpWidth);
                    int bmpY = (((bmpOriginY - kernHalfHeight + tY) + bmpHeight) % bmpHeight);
                    switch (rotation)
                    {
                        case 0:
                            data[tX, tY] = bitmap[bmpX, bmpY];
                            break;
                        case 1:
                            data[kernHeight - tY - 1, tX] = bitmap[bmpX, bmpY];
                            break;
                        case 2:
                            data[kernWidth - tX - 1, kernHeight - tY - 1] = bitmap[bmpX, bmpY];
                            break;
                        case 3:
                            data[tY, kernWidth - tX - 1] = bitmap[bmpX, bmpY];
                            break;
                        default:
                            throw new IndexOutOfRangeException($"Unable to handle {rotation} rotations on kernel");
                    }
                }
            }
        }

        public Color Origin
        {
            get => data[HalfWidth, HalfHeight];
        }

        public bool MatchesAt(Color color, int xOffset, int yOffset)
        {
            int x = HalfWidth + xOffset;
            int y = HalfHeight + yOffset;
            if (x < 0 || x >= data.GetLength(0) || y < 0 || y >= data.GetLength(1))
            {
                throw new IndexOutOfRangeException($"Position ({x},{y}) outside the range of this kernel ({data.GetLength(0)}, {data.GetLength(1)})");
            }
            return data[HalfWidth + xOffset, HalfHeight + yOffset].Matches(color);
        }
    }
}