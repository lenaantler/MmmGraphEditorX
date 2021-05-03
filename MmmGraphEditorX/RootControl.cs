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
    public partial class RootControl : UserControl
    {
        public RootControl()
        {
            InitializeComponent();

            vScrollBar1.Maximum = 0;
            vScrollBar1.Value = 0;

            hScrollBar1.ValueChanged += HScrollBar1_ValueChanged;
            vScrollBar1.ValueChanged += VScrollBar1_ValueChanged;
            vScrollBar1.Scroll += VScrollBar1_Scroll;
        }

        private Configuration _configuration;
        internal Configuration Configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
                graphView.Configuration = value;
            }
        }
        internal FrameBar frameBar;
        internal GraphView graphView;

        private Scene _scene;
        public Scene Scene
        {
            get
            {
                return _scene;
            }
            internal set
            {
                _scene = value;

                frameBar.Scene = value;
                graphView.Scene = value;

                updateFrameRange();
            }
        }

        private void updateFrameRange()
        {
            if (Scene == null || graphView.TargetBone == null || graphView.TargetMotionLayer == null)
            {
                hScrollBar1.Maximum = 0;
                hScrollBar1.Value = 0;
            }
            else
            {
                if (graphView.TargetMotionLayer.Frames.Count == 0)
                {
                    hScrollBar1.Maximum = 0;
                    hScrollBar1.Value = 0;
                }
                else
                {
                    int max = (int)Scene.MarkerPosition;

                    foreach (Bone bone in Scene.ActiveModel.Bones)
                    {
                        foreach (MotionLayer layer in bone.Layers)
                        {
                            int frameCount = layer.Frames.Count;
                            if (0 < frameCount)
                            {
                                max = (int)Math.Max(max, layer.Frames[frameCount - 1].FrameNumber);
                            }
                        }
                    }

                    // valueが最大フレーム数になるように調整。
                    max += hScrollBar1.LargeChange - 1;

                    hScrollBar1.Minimum = 0;
                    hScrollBar1.Maximum = max;
                    hScrollBar1.Value = (int)Scene.MarkerPosition;
                }
            }

        }

        private void HScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            if (graphView.TargetMotionLayer != null) Scene.MarkerPosition = hScrollBar1.Value;
        }

        private void VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //System.Diagnostics.Trace.WriteLine("Scroll Value:" + vScrollBar1.Value.ToString());
            //System.Diagnostics.Trace.WriteLine("Scroll Maximum:" + vScrollBar1.Maximum.ToString());
            //System.Diagnostics.Trace.WriteLine("Scroll LargeChange:" + vScrollBar1.LargeChange.ToString());
            int valueMax = (vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);
            //System.Diagnostics.Trace.WriteLine("valueMax:" + valueMax.ToString());
            int valueHalf = valueMax / 2;
            graphView.ViewOffset = vScrollBar1.Value - valueHalf;
            //System.Diagnostics.Trace.WriteLine("ViewOffset:" + graphView.ViewOffset.ToString());

            graphView.Refresh();
        }

        private void VScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Trace.WriteLine("Scroll Value:" + vScrollBar1.Value.ToString());
            //System.Diagnostics.Trace.WriteLine("Scroll Maximum:" + vScrollBar1.Maximum.ToString());
            //System.Diagnostics.Trace.WriteLine("Scroll LargeChange:" + vScrollBar1.LargeChange.ToString());
            int valueMax = (vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);
            //System.Diagnostics.Trace.WriteLine("valueMax:" + valueMax.ToString());
            int valueHalf = valueMax / 2;
            graphView.ViewOffset = vScrollBar1.Value - valueHalf;
            //System.Diagnostics.Trace.WriteLine("ViewOffset:" + graphView.ViewOffset.ToString());

            graphView.Refresh();
        }

        private void TxButton_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.txEnabled = TxButton.Checked;
        }

        private void TyButton_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.tyEnabled = TyButton.Checked;
        }

        private void TzButton_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.tzEnabled = TzButton.Checked;
        }

        private void RxButton_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.rxEnabled = RxButton.Checked;
        }

        private void RyButton_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.ryEnabled = RyButton.Checked;
        }

        private void RzButton_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.rzEnabled = RzButton.Checked;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            float prevZoomScale = graphView.ZoomScale;

            if (0 < e.Delta)
            {
                graphView.zoomIn();
            }
            else
            {
                graphView.zoomOut();
            }

            //System.Diagnostics.Trace.WriteLine("ViewScale:" + graphView.ZoomScale.ToString());

            int value = vScrollBar1.Value;
            int valueMax = (vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);

            vScrollBar1.LargeChange = graphView.Height + 1;
            vScrollBar1.Maximum = (int)(graphView.Height * graphView.ZoomScale);

            if (valueMax == 0)
            {
                //vScrollBar1.Value = 0;
                valueMax = (vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);
                vScrollBar1.Value = Math.Min(vScrollBar1.Maximum, Math.Max(vScrollBar1.Minimum, (int)(valueMax / 2)));
            }
            else
            {
                float pos = (float)value / (float)valueMax;

                if (prevZoomScale == 1.0f && graphView.ZoomScale != 1.0f)
                {
                    pos = 0.5f;
                }

                valueMax = (vScrollBar1.Maximum - vScrollBar1.LargeChange + 1);
                vScrollBar1.Value = Math.Min(vScrollBar1.Maximum, Math.Max(vScrollBar1.Minimum, (int)(valueMax * pos)));
            }
            //vScrollBar1.Value = 0;
            //    //e.Delta;
            //    //e.Y;
        }
    }
}
