using System.Drawing.Drawing2D;

namespace WFCVisualizer;

public class PictureBoxWithFiltering : PictureBox
{
    public InterpolationMode Mode { get; set; }
    protected override void OnPaint(PaintEventArgs pe)
    {
        pe.Graphics.InterpolationMode = Mode;
        base.OnPaint(pe);
    }
}