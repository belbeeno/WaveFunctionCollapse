using System.Timers;
using WaveFunctionCollapse;

namespace WFCVisualizer
{
    public partial class Form1 : Form
    {
        DirectBitmap? image = null;
        DirectBitmap lastKernel;
        WFCGenerator wfc;
        private System.Timers.Timer _timer = new System.Timers.Timer(100.0);

        readonly (int, int) outputDimensions = new(128, 128);

        public Form1()
        {
            InitializeComponent();
            int kernelHalfWidth = 2;
            int kernelHalfHeight = kernelHalfWidth;
            wfc = new WFCGenerator("LessRooms.png", kernelHalfWidth, kernelHalfHeight, true, false);
            wfc.Initialize(outputDimensions.Item1, outputDimensions.Item2);
            _timer.Elapsed += Update;
            lastKernel = new DirectBitmap(kernelHalfWidth * 2 + 1, kernelHalfHeight * 2 + 1);

            pictureBox = new PictureBoxWithFiltering();
            picLastKernel = new PictureBoxWithFiltering();
        }

        private void Update(object? sender, ElapsedEventArgs e)
        {
            if (!Step())
            {
                _timer.Stop();
            }
        }
        private bool Step()
        {
            if (wfc == null)
            {
                Toast("WFC not initialized, can't advance WFC.");
                return false;
            }

            int width = wfc.outputBmp.GetLength(0);
            int height = wfc.outputBmp.GetLength(1);
            if (image == null || image.Width != width || image.Height != height)
            {
                if (image != null) { image.Dispose(); }
                image = new(width, height);
            }

            bool retVal = wfc.Step();
            ExtractToImage(wfc.outputBmp, ref image);
            pictureBox.Image = image.bitmap;
            if (wfc.LastUsedKernel.HasValue)
            {
                ExtractToImage(wfc.LastUsedKernel.Value.data, ref lastKernel);
                picLastKernel.Image = lastKernel.bitmap;
            }

            return retVal;
        }

        private void Toast(string message)
        {
            lblStatus.Text = message;
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            Step();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (wfc == null)
            {
                Toast("WFC not initialized, can't advance WFC.");
                return;
            }

            int width = wfc.outputBmp.GetLength(0);
            int height = wfc.outputBmp.GetLength(1);
            if (image == null || image.Width != width || image.Height != height)
            {
                if (image != null) { image.Dispose(); }
                image = new(width, height);
            }

            bool success = wfc.FindNextKernel();
            ExtractToImage(wfc.outputBmp, ref image);
            pictureBox.Image = image.bitmap;
            if (wfc.NextKernel.HasValue)
            {
                ExtractToImage(wfc.NextKernel.Value.data, ref lastKernel);
                picLastKernel.Image = lastKernel.bitmap;
            }
        }
        private async void btnComplete_Click(object sender, EventArgs e)
        {
            int width = wfc.outputBmp.GetLength(0);
            int height = wfc.outputBmp.GetLength(1);
            if (image == null || image.Width != width || image.Height != height)
            {
                if (image != null) { image.Dispose(); }
                image = new(width, height);
            }

            Toast("Starting");
            IProgress<int> progress = new Progress<int>((i) =>
            {
                progressBar.Value = i;
            });
            Task task = Task.Factory.StartNew(() =>
            {
                while (wfc.Step())
                {
                    if (progress != null)
                    {
                        progress.Report((int)(wfc.GetPercent() * 100f));
                    }
                }
            });
            await task;

            ExtractToImage(wfc.outputBmp, ref image);
            pictureBox.Image = image.bitmap;
            progressBar.Value = 0;
            Toast("Complete");
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Stop();
                Toast("Stopped");
            }
            else
            {
                _timer.Start();
                Toast("Playing...");
            }
        }
        private void btnDisplayKernels_Click(object sender, EventArgs e)
        {
            if (wfc == null)
            {
                Toast("WFC not initialized, can't display kernels.");
                return;
            }

            int kWidth = (int)(wfc.kernelBmp.GetLength(0) * 1.1f) + 1;
            int kHeight = (int)(wfc.kernelBmp.GetLength(1) * 1.1f) + 1;
            int kCount = wfc.kernels.Count;

            int picWidth = 500;
            int xMax = picWidth / kWidth;
            if (image != null) image.Dispose();
            image = new(picWidth, picWidth);

            for (int i = 0; i < kCount; ++i)
            {
                int x = i % xMax;
                int y = i / xMax;
                ExtractToImage(wfc.kernels[i].data, ref image, x * kWidth + 1, y * kHeight + 1);
            }
            pictureBox.Image = image.bitmap;
            Toast($"Displaying {kCount} kernels ({wfc.kernelBmp.GetLength(0) * wfc.kernelBmp.GetLength(1) * 16})");
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            wfc.Initialize(outputDimensions.Item1, outputDimensions.Item2);
        }

        private PictureBoxWithFiltering pictureBox;
        private PictureBoxWithFiltering picLastKernel;

        private void Form1_Load(object sender, EventArgs e)
        {
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            pictureBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            pictureBox.BackColor = SystemColors.ControlDarkDark;
            pictureBox.Location = new Point(12, 12);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(749, 460);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Mode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            Controls.Add(pictureBox);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();

            ((System.ComponentModel.ISupportInitialize)picLastKernel).BeginInit();
            picLastKernel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            picLastKernel.BackColor = SystemColors.ActiveCaption;
            picLastKernel.Dock = DockStyle.Left;
            picLastKernel.Name = "picLastKernel";
            picLastKernel.Size = new Size(60, 60);
            picLastKernel.SizeMode = PictureBoxSizeMode.Zoom;
            picLastKernel.TabIndex = 5;
            picLastKernel.TabStop = false;
            picLastKernel.Mode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            controlsGroup.Controls.Add(picLastKernel);
            ((System.ComponentModel.ISupportInitialize)picLastKernel).BeginInit();
        }

        private void ExtractToImage(WaveFunctionCollapse.Color[,] bitmap, ref DirectBitmap target, int xOutputOffset = 0, int yOutputOffset = 0)
        {
            int width = bitmap.GetLength(0);
            int height = bitmap.GetLength(1);
            if (target == null)
            {
                target = new(width, height);
            }

            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    var color = bitmap[x, y];
                    target.Set(x + xOutputOffset, y + yOutputOffset, color);
                }
            }
        }
    }
}
