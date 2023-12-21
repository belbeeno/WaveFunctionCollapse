using System.Diagnostics;

namespace WaveFunctionCollapse
{
    public class WFCGenerator
    {
        private class WaveElement
        {
            public HashSet<int> data = new();
            public bool observed = false;
            public int finalKernelIdx = -1;
        }

public readonly Color[,] kernelBmp;
        public readonly List<Kernel> kernels;
        public Kernel? LastUsedKernel { get; private set; } = null;
        public Kernel? NextKernel { get; private set; } = null;

        private class Pair
        {
            public int x;
            public int y;
        }

        private WaveElement[,] _wave;
        public Color[,] outputBmp;

        private Random _random;

        public float GetPercent()
        {
            if (_wave.GetLength(0) == 0 || _wave.GetLength(1) == 0) return -1;

            float count = 0;
            for (int y = 0; y < _wave.GetLength(1); ++y)
            {
                for (int x = 0; x < _wave.GetLength(0); ++x)
                {
                    if (_wave[x, y].observed) count += 1f;
                }
            }
            return count / (_wave.GetLength(1) * _wave.GetLength(0));
        }

        public WFCGenerator(string path, int kHalfWidth, int kHalfHeight, bool doFlips, bool doRots)
        {
            (var data, var width, var height) = Helpers.LoadBitmap(path);
            if (width <= 0 || height <= 0)
            {
                throw new FileLoadException("Unable to load bitmap when attempting to construct WFCGenerator");
            }
            kernelBmp = new Color[width,height];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < height; ++x)
                {
                    kernelBmp[x, y].Data = (uint)data[y * width + x];
                }
            }
            kernels = new();
            int bmpWidth = kernelBmp.GetLength(0);
            int bmpHeight = kernelBmp.GetLength(1);
            for (int y = 0; y < bmpHeight; ++y)
            {
                for (int x = 0; x < bmpWidth; ++x)
                {
                    for (int flips = 0; flips < (doFlips ? 4 : 1); ++flips)
                    {
                        bool flipX = (flips % 2) == 1;
                        bool flipY = (flips >= 2);
                        for (int rotations = 0; rotations < (doRots ? 4 : 1); ++rotations)
                        {
                            Kernel newKernel = new(ref kernelBmp, x, y, kHalfWidth, kHalfHeight, rotations, flipX, flipY);
                            kernels.Add(newKernel);
                        }
                    }
                }
            }
            _random = new();
            _wave = new WaveElement[0,0];
            outputBmp = new Color[0,0];
        }

        public void Initialize(int outputWidth, int outputHeight, int? seed = null)
        {
            if (seed != null)
            {
                _random = new(seed.Value);
            }

            _wave = new WaveElement[outputWidth, outputHeight];
            int[] FULL_ENTROPY = new int[kernels.Count];
            for (int i = 0; i < FULL_ENTROPY.Length; ++i)
            {
                FULL_ENTROPY[i] = i;
            }

            outputBmp = new Color[outputWidth, outputHeight];
            for (int y = 0; y < outputWidth; ++y)
            {
                for (int x = 0; x < outputHeight; ++x)
                {
                    _wave[x, y] = new();
                    _wave[x, y].data = new(FULL_ENTROPY);
                    outputBmp[x, y] = new();
                }
            }
            Step(new() { x = outputWidth / 2, y = outputHeight / 2 });
        }

        private Pair? GetLowestEntroyPair()
        {
            int lowestEntropy = int.MaxValue;
            List<Pair> candidates = new();
            for (int y = 0; y < _wave.GetLength(1); ++y)
            {
                for (int x = 0; x < _wave.GetLength(0); ++x)
                {
                    if (!_wave[x, y].observed && _wave[x, y].data.Count <= lowestEntropy)
                    {
                        if (_wave[x, y].data.Count < lowestEntropy)
                        {
                            candidates.Clear();
                        }
                        candidates.Add(new() { x = x, y = y });
                        lowestEntropy = _wave[x, y].data.Count;
                    }
                }
            }
            return (candidates.Count == 0 ? null : candidates[_random.Next(candidates.Count)]);
        }
        public bool Step()
        {
            return Step(null);
        }
        private bool Step(Pair? pair)
        {
            if (pair == null)
            {
                pair = GetLowestEntroyPair();
                if (pair == null)
                {
                    return false;
                }
            }

            ref WaveElement targetWave = ref _wave[pair.x, pair.y];
            int kernelIdx = GetKernelIdxStepFast(ref targetWave);
            targetWave.observed = true;

            if (kernelIdx < 0)
            {
                //throw new Exception($"Unable to locate a kernel that works for position ({pair.x},{pair.y}) out of {targetWave.data.Count} kernels");
                Debug.WriteLine($"Unable to locate a kernel that works for position ({pair.x},{pair.y}) out of {targetWave.data.Count} kernels");
                return true;
            }

            targetWave.finalKernelIdx = kernelIdx;
            LastUsedKernel = kernels[kernelIdx];
            outputBmp[pair.x, pair.y] = LastUsedKernel.Value.Origin;

            UpdateWave(pair, LastUsedKernel.Value);

            return true;
        }

        public bool FindNextKernel()
        {
            Pair? pair = GetLowestEntroyPair();
            if (pair == null) return false;
            outputBmp[pair.x, pair.y] = new(255, 255, 0, 0);

            ref WaveElement targetWave = ref _wave[pair.x, pair.y];
            int kernelIdx = GetKernelIdxStepFast(ref targetWave);
            if (kernelIdx < 0)
            {
                //throw new Exception($"Unable to locate a kernel that works for position ({pair.x},{pair.y}) out of {targetWave.data.Count} kernels");
                Debug.WriteLine($"Unable to locate a kernel that works for position ({pair.x},{pair.y}) out of {targetWave.data.Count} kernels");
                return false;
            }

            NextKernel = kernels[kernelIdx];
            return true;
        }

        private int GetKernelIdxStepFast(ref WaveElement targetWave)
        {
            return (targetWave.data.Count > 0 ? targetWave.data.ElementAt(_random.Next(targetWave.data.Count)) : -1);
        }

        private void UpdateWave(Pair pair, Kernel kernel)
        {
            List<int> toKeepScratchpad = new();
            int width = _wave.GetLength(0);
            int height = _wave.GetLength(1);
            for (int kY = -kernel.HalfHeight; kY <= kernel.HalfHeight; ++kY)
            {
                for (int kX = -kernel.HalfWidth; kX <= kernel.HalfWidth; ++kX)
                {
                    int x = (pair.x + kX + width) % width;
                    int y = (pair.y + kY + height) % height;
                    if (kX == 0 && kY == 0)
                    {
                        continue;
                    }

                    toKeepScratchpad.Clear();
                    ref WaveElement wave = ref _wave[x, y];
                    // Just checking if its observed is probably good enough, but if you want to continue to update
                    // the wave superposition separately then consider testing against wave.data.Count
                    if (wave.observed) continue;
                    foreach (int kernelIdx in wave.data)
                    {
                        Kernel otherKernel = kernels[kernelIdx];
                        if (otherKernel.MatchesAt(kernel.Origin, -kX, -kY))
                        {
                            toKeepScratchpad.Add(kernelIdx);
                        }
                    }
                    if (toKeepScratchpad.Count > 0)
                    {
                        byte val = (byte)(200 * ((float)toKeepScratchpad.Count / kernels.Count) + 55);
                        outputBmp[x, y] = new(255, val, 0, val);
                    }
                    else
                    {
                        outputBmp[x, y] = new(255, 255, 255, 255);
                    }
                    wave.data.IntersectWith(toKeepScratchpad);
                }
            }
        }
    }
}
