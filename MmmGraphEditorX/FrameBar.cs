using MikuMikuPlugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MmmGraphEditorX
{
    public partial class FrameBar : UserControl
    {
        public Scene Scene { get; set; }


        public FrameBar()
        {
            InitializeComponent();

            DoubleBuffered = true;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            // NOP.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //
            // fill background.
            //
            g.FillRectangle(Brushes.DarkGray, 0, 0, Width, Height);


            //
            // draw ticks & numbers.
            //
            if (Scene == null) return;


            long markerPosition = Scene.MarkerPosition;

            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip;


            //
            // draw ticks and labels.
            //
            int xCenter = Width / 2;
            int numOfHalfFrames = Width / GraphEditorX.frameWidth;
            int xOffsetFromCenter = numOfHalfFrames * GraphEditorX.frameWidth;

            for (int n = 0; n < (numOfHalfFrames * 2); ++n)
            {
                long frameNumber = markerPosition - numOfHalfFrames + n;
                if (frameNumber < 0) continue;

                bool hasLabel = frameNumber % 10 == 0;
                bool isLargeTick = frameNumber % 5 == 0;
                Pen pen = hasLabel ? Pens.White : isLargeTick ? Pens.WhiteSmoke : Pens.Gainsboro;

                int x = xCenter - (numOfHalfFrames * GraphEditorX.frameWidth) + (n * GraphEditorX.frameWidth);
                int y1 = Height - (isLargeTick ? 10 : 5);
                int y2 = Height;
                RectangleF rect = new RectangleF(x - (GraphEditorX.frameWidth * 2), 2, GraphEditorX.frameWidth * 4, Font.Height);
                g.DrawLine(pen, x, y1, x, y2 );
                if ( hasLabel ) g.DrawString(frameNumber.ToString(), Font, Brushes.White, rect, stringFormat);
            }


            //
            // draw marker number.
            //

            RectangleF markerRect = new RectangleF(xCenter - (GraphEditorX.frameWidth * 1.5f), Font.Height, GraphEditorX.frameWidth * 3, Font.Height);

            g.FillRectangle(Brushes.YellowGreen, markerRect);
            g.DrawString(markerPosition.ToString(), Font, Brushes.Black, markerRect, stringFormat);
        }
    }
}
