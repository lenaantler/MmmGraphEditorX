using DxMath;
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
    public partial class GraphView : UserControl
    {
        private Scene _scene;
        internal MikuMikuPlugin.Scene Scene
        {
            get { return _scene; }
            set
            {
                _scene = value;
                updateTarget();
            }
        }

        internal Configuration Configuration { get; set; }

        internal MikuMikuPlugin.MotionLayer TargetMotionLayer { get; set; }
        private MikuMikuPlugin.Bone _targetBone;
        internal MikuMikuPlugin.Bone TargetBone
        {
            get { return _targetBone; }
            set
            {
                _targetBone = value;
                if (_targetBone != null)
                {
                    qWorldToLocal = calcWorldToLocalRotation(_targetBone);
                }
                else
                {
                    qWorldToLocal = Quaternion.Identity;
                }
            }
        }

        private Quaternion qWorldToLocal = Quaternion.Identity;
        private float minGraphValue = -3.2f;  // Math.PI + margin;
        private float maxGraphValue = 3.2f;  // Math.PI + margin;
        private float[] scales = {1.0f, 1.25f, 1.5f, 1.75f, 2.0f, 3.0f, 4.0f, 6.0f, 8.0f, 16.0f, 32.0f };
        private const int kDefaultScaleIndex = 0; // = 1.0f
        private int scaleIndex = kDefaultScaleIndex;

        internal float ZoomScale
        {
            get
            {
                return scales[scaleIndex];
            }
        }
        internal int ViewOffset { get; set; }



        public GraphView()
        {
            InitializeComponent();

            DoubleBuffered = true;
        }

        private Quaternion calcWorldToLocalRotation(MikuMikuPlugin.Bone bone)
        {
            try
            {
                Vector3 axisWorld, axisLocal;
                if (bone.LocalAxisX.X != 1.0f)
                {
                    axisWorld = new Vector3(1, 0, 0);
                    axisLocal = bone.LocalAxisX;
                }
                else if (bone.LocalAxisY.Y != 1.0f)
                {
                    axisWorld = new Vector3(0, 1, 0);
                    axisLocal = bone.LocalAxisY;
                }
                else if (bone.LocalAxisZ.Z != 1.0f)
                {
                    axisWorld = new Vector3(0, 0, 1);
                    axisLocal = bone.LocalAxisZ;
                }
                else
                {
                    return Quaternion.Identity;
                }
                return Quaternion.Conjugate(Quaternion.Normalize(Quaternion.RotationAxis(Vector3.Cross(axisWorld, axisLocal), (float)Math.Acos(Vector3.Dot(axisWorld, axisLocal)))));
            }
            catch (Exception)
            {
                return Quaternion.Identity;
            }
        }

        private void updateTarget()
        {
            if (Scene == null || Scene.ActiveModel == null)
            {
                TargetMotionLayer = null;
                TargetBone = null;
                minGraphValue = -3.2f;
                maxGraphValue = 3.2f;
                return;
            }

            MikuMikuPlugin.MotionLayer activeMotionLayer = null;
            MikuMikuPlugin.Bone activeBone = null;

            foreach (MikuMikuPlugin.Bone bone in Scene.ActiveModel.Bones)
            {
                foreach (MikuMikuPlugin.MotionLayer layer in bone.SelectedLayers)
                {
                    activeMotionLayer = layer;
                    activeBone = bone;
                    break;
                }
                if (activeMotionLayer != null && activeBone != null) break;
            }

            if (activeMotionLayer == null || activeBone == null)
            {
                TargetMotionLayer = null;
                TargetBone = null;
                minGraphValue = -3.2f;
                maxGraphValue = 3.2f;
                return;
            }

            TargetMotionLayer = activeMotionLayer;
            TargetBone = activeBone;

            updateValueRange();
        }

        private Vector3 eulerZXYFromQuaternion(Quaternion q)
        {
            double threshold = Math.PI - 1.0e-7;
            Matrix mat = Matrix.RotationQuaternion(q);
            double rx = Math.Asin(-mat.M32);
            double cosx = Math.Cos(rx);

            // ジンバルロック回避
            if (threshold < Math.Abs(rx))
            {
                rx = (0 < rx) ? -threshold : threshold;
                cosx = Math.Cos(rx);
            }

            double siny = (mat.M31 / cosx);
            double cosy = (mat.M33 / cosx);
            double ry = Math.Atan2(siny, cosy);

            double sinz = (mat.M12 / cosx);
            double cosz = (mat.M22 / cosx);
            double rz = Math.Atan2(sinz, cosz);

            return new Vector3((float)rx, (float)ry, (float)rz);
        }

        private Quaternion worldToLocalRotation(Quaternion q)
        {
            Vector4 axis = Vector3.Transform(q.Axis, qWorldToLocal);
            return Quaternion.Normalize(Quaternion.RotationAxis(new Vector3(axis.X, axis.Y, axis.Z), q.Angle));
        }

        internal void updateValueRange()
        {
            if (TargetMotionLayer == null || TargetBone == null) return;

            bool isHitMarker = false;
            float max = 3.2f;  // Math.PI + margin;
            float min = -3.2f; // Math.PI + margin;

            bool[] enabled = { Configuration.txEnabled, Configuration.tyEnabled, Configuration.tzEnabled };

            foreach (IMotionFrameData frameData in TargetMotionLayer.Frames)
            {
                if (!isHitMarker && frameData.FrameNumber == Scene.MarkerPosition)
                {
                    for (int n = 0; n < 3; ++n)
                    {
                        if (!enabled[n]) continue;

                        float value = TargetMotionLayer.CurrentLocalMotion.Move[n];
                        if (max < value) max = value;
                        else if (value < min) min = value;
                    }
                    isHitMarker = true;
                }

                for (int n = 0; n < 3; ++n)
                {
                    if (!enabled[n]) continue;

                    float value = frameData.Position[n];
                    if (max < value) max = value;
                    else if (value < min) min = value;
                }
            }

            minGraphValue = min;
            maxGraphValue = max;
        }

        /// <summary>
        /// 背景グリッドライン(?)の描画
        /// </summary>
        /// <param name="g"></param>
        private void drawGrid(Graphics g)
        {
            g.FillRectangle(Brushes.LightGray, 0, 0, Width, Height);

            if (Scene == null) return;


            long markerPosition = Scene.MarkerPosition;


            int xCenter = Width / 2;
            int numOfHalfFrames = Width / GraphEditorX.frameWidth;
            int xOffsetFromCenter = numOfHalfFrames * GraphEditorX.frameWidth;

            // draw tick line.
            for (int n = 0; n < (numOfHalfFrames * 2); ++n)
            {
                long frameNumber = markerPosition - numOfHalfFrames + n;
                if (frameNumber < 0) continue;

                bool hasLabel = frameNumber % 10 == 0;
                bool isLargeTick = frameNumber % 5 == 0;
                Pen pen = (frameNumber == markerPosition) ? Pens.YellowGreen : hasLabel ? Pens.White : isLargeTick ? Pens.WhiteSmoke : Pens.Gainsboro;

                int x = xCenter - (numOfHalfFrames * GraphEditorX.frameWidth) + (n * GraphEditorX.frameWidth);
                int y1 = 0;
                int y2 = Height;
                g.DrawLine(pen, x, y1, x, y2);
            }


            //
            // draw horizontal lines.
            //

            float maxRange = Math.Max(Math.Abs(maxGraphValue), Math.Abs(minGraphValue));
            float valueScale = ((Height / 2) / maxRange) * ZoomScale;
            int yCenter = (Height / 2) - ViewOffset;


            // draw rotation range line.
            g.DrawLine(Pens.Gray, 0, yCenter + (float)Math.PI * valueScale, Width, yCenter + (float)Math.PI * valueScale);
            g.DrawLine(Pens.Gray, 0, yCenter + (float)-Math.PI * valueScale, Width, yCenter + (float)-Math.PI * valueScale);
        }

        // draw values graph.
        private void drawGraph(Graphics g)
        {
            if (Scene == null) return;
            if (TargetMotionLayer == null) return;

            int markerPosition = (int)Scene.MarkerPosition;
            int drawFrameCount = (int)Math.Floor((float)Width / (float)GraphEditorX.frameWidth) + 2;
            int firstFrameNumber = markerPosition - drawFrameCount / 2;
            int lastFrameNumber = firstFrameNumber + drawFrameCount;

            {
                int prevFrameNumber = 0;
                int postFrameNumber = int.MaxValue;

                foreach (IMotionFrameData frameData in TargetMotionLayer.Frames)
                {
                    if (frameData.FrameNumber < firstFrameNumber) prevFrameNumber = (int)frameData.FrameNumber;
                    if (lastFrameNumber < frameData.FrameNumber && frameData.FrameNumber < postFrameNumber)
                    {
                        postFrameNumber = (int)frameData.FrameNumber;

                        //System.Diagnostics.Trace.WriteLine("post frame number:" + postFrameNumber.ToString());
                    }
                }

                if (postFrameNumber == int.MaxValue) postFrameNumber = lastFrameNumber;

                firstFrameNumber = prevFrameNumber;
                lastFrameNumber = postFrameNumber;
            }

            IMotionFrameData prevFrameData = null;


            foreach (IMotionFrameData frameData in TargetMotionLayer.Frames)
            {
                if (firstFrameNumber <= frameData.FrameNumber && frameData.FrameNumber <= lastFrameNumber)
                {
                    plotKeyFrame(g, prevFrameData, frameData);
                }

                prevFrameData = frameData;
            }
        }

        private void plotKeyFrame(Graphics g, IMotionFrameData prevFrameData, IMotionFrameData frameData)
        {
            int markerPosition = (int)Scene.MarkerPosition;
            float maxRange = Math.Max(Math.Abs(maxGraphValue), Math.Abs(minGraphValue));
            float valueScale = ((Height / 2) / maxRange) * ZoomScale;
            int xCenter = Width / 2;
            int yCenter = (Height / 2) - ViewOffset;

            Brush[] brushs = { Brushes.Red, Brushes.Green, Brushes.Blue };
            Brush[] brushs2 = { Brushes.DeepPink, Brushes.YellowGreen, Brushes.DodgerBlue };
            Pen[] pens = { Pens.Red, Pens.Green, Pens.Blue };
            Vector3 pos = (markerPosition == frameData.FrameNumber) ? TargetMotionLayer.CurrentLocalMotion.Move : frameData.Position;
            Quaternion rot = worldToLocalRotation(((markerPosition == frameData.FrameNumber) ? TargetMotionLayer.CurrentLocalMotion.Rotation : frameData.Quaternion));
            Vector3 euler = eulerZXYFromQuaternion(rot);


            int frameOffset = (int)frameData.FrameNumber - markerPosition;

            int size = GraphEditorX.keySize;
            int x = xCenter + GraphEditorX.frameWidth * frameOffset;


            if (!isValid(pos, euler))
            {
                g.DrawLine(Pens.Red, x, 0, x, Height);
                return;
            }

            // draw key value rect.

            if (TargetBone.BoneFlags.HasFlag(MikuMikuPlugin.BoneType.XYZ))
            {
                bool[] enabled = { Configuration.txEnabled, Configuration.tyEnabled, Configuration.tzEnabled };

                for (int n = 0; n < 3; ++n)
                {
                    if (!enabled[n]) continue;
                    float y = yCenter - pos[n] * valueScale;
                    g.FillRectangle(brushs[n], x - (size / 2), y - (size / 2), size, size);
                }
                drawTranslationLine(g, prevFrameData, frameData);
            }

            if (TargetBone.BoneFlags.HasFlag(MikuMikuPlugin.BoneType.Rotate))
            {
                bool[] enabled = { Configuration.rxEnabled, Configuration.ryEnabled, Configuration.rzEnabled };

                for (int n = 0; n < 3; ++n)
                {
                    if (!enabled[n]) continue;
                    float y = yCenter - euler[n] * valueScale;
                    g.FillRectangle(brushs2[n], x - (size / 2), y - (size / 2), size, size);
                }
                drawRotationLine(g, prevFrameData, frameData);
            }

        }

        private bool isValid(Vector3 pos, Vector3 euler)
        {
            if (float.IsNaN(pos.X) || float.IsNaN(pos.Y) || float.IsNaN(pos.Z) || float.IsNaN(euler.X) || float.IsNaN(euler.Y) || float.IsNaN(euler.Z)) return false;
            return true;
        }

        private void drawTranslationLine(Graphics g, IMotionFrameData from, IMotionFrameData to)
        {
            if (from == null) return;

            int markerPosition = (int)Scene.MarkerPosition;
            float maxRange = Math.Max(Math.Abs(maxGraphValue), Math.Abs(minGraphValue));
            float valueScale = ((Height / 2) / maxRange) * ZoomScale;
            int xCenter = Width / 2;
            int yCenter = (Height / 2) - ViewOffset;

            int frameSpan = (int)(to.FrameNumber - from.FrameNumber);
            int xSpan = frameSpan * GraphEditorX.frameWidth;
            int xFrom = xCenter - (int)(Scene.MarkerPosition - from.FrameNumber) * GraphEditorX.frameWidth;
            int xTo = xCenter - (int)(Scene.MarkerPosition - to.FrameNumber) * GraphEditorX.frameWidth;

            Vector3 fromPos = (Scene.MarkerPosition == from.FrameNumber) ? TargetMotionLayer.CurrentLocalMotion.Move : from.Position;
            Vector3 toPos = (Scene.MarkerPosition == to.FrameNumber) ? TargetMotionLayer.CurrentLocalMotion.Move : to.Position;

            MikuMikuPlugin.InterpolatePoint[] interpolations = { to.InterpolXA, to.InterpolXB, to.InterpolYA, to.InterpolYB, to.InterpolZA, to.InterpolZB };
            Pen[] pens = { Pens.Red, Pens.Green, Pens.Blue };
            bool[] enabled = { Configuration.txEnabled, Configuration.tyEnabled, Configuration.tzEnabled };

            for (int n = 0; n < 3; ++n)
            {
                if (!enabled[n]) continue;
                float ySpan = toPos[n] - fromPos[n];
                float yFrom = yCenter - fromPos[n] * valueScale;
                float yTo = yCenter - toPos[n] * valueScale;
                float ax = xFrom + (interpolations[n * 2].X / 128.0f * xSpan);
                float ay = yFrom - (interpolations[n * 2].Y / 128.0f * ySpan) * valueScale;
                float bx = xFrom + (interpolations[n * 2 + 1].X / 128.0f * xSpan);
                float by = yFrom - (interpolations[n * 2 + 1].Y / 128.0f * ySpan) * valueScale;

                g.DrawBezier(pens[n], xFrom, yFrom, ax, ay, bx, by, xTo, yTo);
            }
        }

        private void drawRotationLine(Graphics g, IMotionFrameData from, IMotionFrameData to)
        {
            if (from == null) return;

            int markerPosition = (int)Scene.MarkerPosition;
            float maxRange = Math.Max(Math.Abs(maxGraphValue), Math.Abs(minGraphValue));
            float valueScale = ((Height / 2) / maxRange) * ZoomScale;
            int xCenter = Width / 2;
            int yCenter = (Height / 2) - ViewOffset;

            int frameSpan = (int)(to.FrameNumber - from.FrameNumber);
            int xSpan = frameSpan * GraphEditorX.frameWidth;
            int xFrom = xCenter - (int)(Scene.MarkerPosition - from.FrameNumber) * GraphEditorX.frameWidth;
            int xTo = xCenter - (int)(Scene.MarkerPosition - to.FrameNumber) * GraphEditorX.frameWidth;

            Quaternion qFrom = worldToLocalRotation((Scene.MarkerPosition == from.FrameNumber) ? TargetMotionLayer.CurrentLocalMotion.Rotation : from.Quaternion);
            Quaternion qTo = worldToLocalRotation((Scene.MarkerPosition == to.FrameNumber) ? TargetMotionLayer.CurrentLocalMotion.Rotation : to.Quaternion);
            Vector3 fromEuler = eulerZXYFromQuaternion(qFrom);
            Vector3 toEuler = eulerZXYFromQuaternion(qTo);


            MikuMikuPlugin.InterpolatePoint[] interpolations = { to.InterpolXA, to.InterpolXB, to.InterpolYA, to.InterpolYB, to.InterpolZA, to.InterpolZB };
            Brush[] brushes = { Brushes.DeepPink, Brushes.YellowGreen, Brushes.DodgerBlue };

            double t = 0;
            double tx = 0;
            double amount = 0;
            for (int n = 0; n < xSpan; ++n)
            {
                if ((xFrom + n) < 0) continue;
                if (Width < (xFrom + n)) break;

                while (tx < n && t < 1)
                {
                    t += 0.01f;
                    tx = (t * t * t + 3 * t * t * (1 - t) * to.InterpolRB.X / 128 + 3 * t * (1 - t) * (1 - t) * to.InterpolRA.X / 128) * xSpan;
                }
                amount = (t * t * t + 3 * t * t * (1 - t) * to.InterpolRB.Y / 128 + 3 * t * (1 - t) * (1 - t) * to.InterpolRA.Y / 128);

                Quaternion qMiddle = Quaternion.Slerp(qFrom, qTo, (float)amount);
                Vector3 euler = eulerZXYFromQuaternion(qMiddle);

                bool[] enabled = { Configuration.rxEnabled, Configuration.ryEnabled, Configuration.rzEnabled };
                for (int m = 0; m < 3; ++m)
                {
                    if (!enabled[m]) continue;

                    g.FillRectangle(brushes[m], xFrom + n, yCenter - (euler[m] * valueScale), 1, 2);
                }
            }
        }

        internal void zoomIn()
        {
            if (scaleIndex < (scales.Length - 1)) ++scaleIndex;
        }

        internal void zoomOut()
        {
            if (0 < scaleIndex) --scaleIndex;
            if (ZoomScale <= 1.0f) ViewOffset = 0;
        }



        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
            // NOP.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            Graphics g = e.Graphics;

            try
            {
                drawGrid(g);
                drawGraph(g);
            }
            catch (Exception)
            {
                // ignore.
            }
        }
    }
}
