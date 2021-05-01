using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MikuMikuMoving.Plugin;
using MikuMikuPlugin;
using DxMath;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using System.Reflection;

namespace MmmGraphEditorX
{
    public class GraphEditorX : IResidentPlugin, IHaveUserControl, ICanSavePlugin
    {
        internal static int frameWidth = 12;
        internal static int keySize = 8;
        protected RootControl rootControl;
        protected Configuration configuration;
        private System.Diagnostics.Stopwatch stopWatch = null;
        private Control prevParent = null;


        public Guid GUID => new Guid("492E342E-2A3E-4D91-956C-DC4086DBE794");

        public string Description => "GraphEditor(Readonly)";

        public IWin32Window ApplicationForm { get ; set ; }

        public string Text => "GraphEditor(Readonly)";

        public string EnglishText => "GraphEditor(Readonly)";

        public Image Image => Properties.Resource1.mmm_graph_icon36;

        public Image SmallImage => Properties.Resource1.mmm_graph_icon24;

        public MikuMikuPlugin.Scene Scene { get ; set ; }

        public UserControl CreateControl()
        {
            rootControl = new RootControl();

            // FrameBar
            rootControl.frameBar = new FrameBar();
            rootControl.frameBar.Name = rootControl._frameBarPlaceHolder.Name;
            rootControl.frameBar.Location = rootControl._frameBarPlaceHolder.Location;
            rootControl.frameBar.Size = rootControl._frameBarPlaceHolder.Size;
            rootControl.frameBar.Height = rootControl.frameBar.Font.Height * 2 + 10;
            rootControl.frameBar.TabIndex = rootControl._frameBarPlaceHolder.TabIndex;
            rootControl.Controls.Remove(rootControl._frameBarPlaceHolder);
            rootControl.Controls.Add(rootControl.frameBar);
            this.rootControl._frameBarPlaceHolder = null;

            // GraphView
            rootControl.graphView = new GraphView();
            rootControl.graphView.Name = rootControl._graphViewPlaceHolder.Name;
            rootControl.graphView.Location = rootControl._graphViewPlaceHolder.Location;
            rootControl.graphView.Size = rootControl._graphViewPlaceHolder.Size;
            rootControl.graphView.TabIndex = rootControl._graphViewPlaceHolder.TabIndex;
            rootControl.Controls.Remove(rootControl._graphViewPlaceHolder);
            rootControl.Controls.Add(rootControl.graphView);
            this.rootControl._graphViewPlaceHolder = null;


            //this.rootControl.SizeChanged += rootControl_SizeChanged;
            //this.rootControl.AutoSize = true;
            //this.rootControl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //this.rootControl.AutoSizeChanged += RootControl_AutoSizeChanged;
            this.rootControl.ParentChanged += RootControl_ParentChanged;
            this.rootControl.SizeChanged += RootControl_SizeChanged;


            //!@todo need implements.
            //this.control.config = this.config;
            //this.control.UpdateUIStateByConfig();
            fitControls();
            rootControl.Configuration = configuration;

            return rootControl;
        }

        private void RootControl_SizeChanged(object sender, EventArgs e)
        {
            fitControls();
        }

        private void RootControl_ParentChanged(object sender, EventArgs e)
        {
            if (prevParent != null) prevParent.SizeChanged -= Parent_SizeChanged;
            prevParent = rootControl.Parent;
            rootControl.Parent.SizeChanged += Parent_SizeChanged;

            this.fitToParent();
        }

        private void Parent_SizeChanged(object sender, EventArgs e)
        {
            fitToParent();
        }

        private void RootControl_AutoSizeChanged(object sender, EventArgs e)
        {
            var w = this.rootControl.Width;
            var h = this.rootControl.Height;

        }

        private void rootControl_SizeChanged(object sender, EventArgs e)
        {
            var w = this.rootControl.Width;
            var h = this.rootControl.Height;
        }

        public void Disabled()
        {
            //!@todo need implements.
            //this.m_rootControl.Scene = null;
        }

        public void Dispose()
        {
            //!@todo need implements.
            //this.m_rootControl.Scene = null;
        }

        public void Enabled()
        {
            // NOP.
        }

        public void Initialize()
        {
            this.configuration = new Configuration();
        }

        public void OnLoadProject(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                Configuration loadconf = (Configuration)serializer.Deserialize(stream);
                Type t = typeof(Configuration);
                foreach (MemberInfo m in t.GetMembers())
                {
                    if (m.MemberType != MemberTypes.Field) continue;
                    FieldInfo f = t.GetField(m.Name, BindingFlags.Public | BindingFlags.Instance);
                    f.SetValue(this.configuration, f.GetValue(loadconf));
                }
                rootControl.Configuration = configuration;
            }
            catch (Exception)
            {
                // ignore
            }
        }

        public Stream OnSaveProject()
        {
            MemoryStream stream = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
            serializer.Serialize(stream, this.configuration);
            return stream;
        }

        /// <summary>
        /// プラグインが有効化されたときに毎フレーム呼び出されるメソッド
        /// </summary>
        /// <param name="Frame"></param>
        /// <param name="ElapsedTime"></param>
        public void Update(float Frame, float ElapsedTime)
        {
            //this.fitToParent();

            if (this.Scene == null) return;
            if (!this.Scene.State.Equals(SceneState.Editing)) return;

            // パフォーマンス改善
            if (stopWatch == null)
            {
                stopWatch = new System.Diagnostics.Stopwatch();
            }
            else if (stopWatch.ElapsedMilliseconds < 100)
            {
                return;
            }
            stopWatch.Reset();
            stopWatch.Start();

            this.rootControl.Scene = this.Scene;
            this.rootControl.Refresh();
        }

        private void fitControls()
        {
            Size rootSize = rootControl.Size;

            int rightWidth = rootSize.Width - (this.rootControl.leftPanel.Location.X + this.rootControl.leftPanel.Width);
            int rightHeight = rootSize.Height;
            int xOrigin = this.rootControl.leftPanel.Location.X + this.rootControl.leftPanel.Width;
            int yOrigin = this.rootControl.leftPanel.Location.Y;

            this.rootControl.frameBar.Width = rightWidth - this.rootControl.vScrollBar1.Width;
            this.rootControl.graphView.Width = this.rootControl.frameBar.Width;
            this.rootControl.graphView.Height = rightHeight - (this.rootControl.frameBar.Height + this.rootControl.hScrollBar1.Height);

            this.rootControl.frameBar.Location = new Point(xOrigin, yOrigin);
            this.rootControl.graphView.Location = new Point(xOrigin, yOrigin + this.rootControl.frameBar.Height);
            this.rootControl.vScrollBar1.Location = new Point(this.rootControl.graphView.Location.X + this.rootControl.graphView.Width, this.rootControl.graphView.Location.Y);
            this.rootControl.hScrollBar1.Location = new Point(this.rootControl.graphView.Location.X, this.rootControl.graphView.Location.Y + this.rootControl.graphView.Height);

            this.rootControl.vScrollBar1.Height = this.rootControl.graphView.Height;
            this.rootControl.hScrollBar1.Width = this.rootControl.graphView.Width;
        }

        private void fitToParent()
        {
            var form = this.rootControl.Parent;

            if (form == null) return;
            if (this.rootControl.Size == form.Size) return;
            if (form.Size.IsEmpty) return;


            this.rootControl.Size = form.Size;

            fitControls();
        }
    }
}
