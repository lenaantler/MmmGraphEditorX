
namespace MmmGraphEditorX
{
    partial class RootControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.leftPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RzButton = new System.Windows.Forms.CheckBox();
            this.RyButton = new System.Windows.Forms.CheckBox();
            this.RxButton = new System.Windows.Forms.CheckBox();
            this.TzButton = new System.Windows.Forms.CheckBox();
            this.TyButton = new System.Windows.Forms.CheckBox();
            this.TxButton = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._frameBarPlaceHolder = new System.Windows.Forms.Panel();
            this._graphViewPlaceHolder = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.leftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // leftPanel
            // 
            this.leftPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.leftPanel.Controls.Add(this.panel1);
            this.leftPanel.Controls.Add(this.RzButton);
            this.leftPanel.Controls.Add(this.RyButton);
            this.leftPanel.Controls.Add(this.RxButton);
            this.leftPanel.Controls.Add(this.TzButton);
            this.leftPanel.Controls.Add(this.TyButton);
            this.leftPanel.Controls.Add(this.TxButton);
            this.leftPanel.Controls.Add(this.label2);
            this.leftPanel.Controls.Add(this.label1);
            this.leftPanel.Location = new System.Drawing.Point(3, 3);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(91, 263);
            this.leftPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(130, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(275, 51);
            this.panel1.TabIndex = 1;
            // 
            // RzButton
            // 
            this.RzButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.RzButton.Checked = true;
            this.RzButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RzButton.Location = new System.Drawing.Point(58, 81);
            this.RzButton.Name = "RzButton";
            this.RzButton.Size = new System.Drawing.Size(24, 23);
            this.RzButton.TabIndex = 7;
            this.RzButton.Text = "Z";
            this.RzButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RzButton.UseVisualStyleBackColor = true;
            this.RzButton.CheckedChanged += new System.EventHandler(this.RzButton_CheckedChanged);
            // 
            // RyButton
            // 
            this.RyButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.RyButton.Checked = true;
            this.RyButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RyButton.Location = new System.Drawing.Point(31, 81);
            this.RyButton.Name = "RyButton";
            this.RyButton.Size = new System.Drawing.Size(25, 23);
            this.RyButton.TabIndex = 6;
            this.RyButton.Text = "Y";
            this.RyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RyButton.UseVisualStyleBackColor = true;
            this.RyButton.CheckedChanged += new System.EventHandler(this.RyButton_CheckedChanged);
            // 
            // RxButton
            // 
            this.RxButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.RxButton.Checked = true;
            this.RxButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RxButton.Location = new System.Drawing.Point(6, 81);
            this.RxButton.Name = "RxButton";
            this.RxButton.Size = new System.Drawing.Size(24, 23);
            this.RxButton.TabIndex = 5;
            this.RxButton.Text = "X";
            this.RxButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.RxButton.UseVisualStyleBackColor = true;
            this.RxButton.CheckedChanged += new System.EventHandler(this.RxButton_CheckedChanged);
            // 
            // TzButton
            // 
            this.TzButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.TzButton.Checked = true;
            this.TzButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TzButton.Location = new System.Drawing.Point(59, 28);
            this.TzButton.Name = "TzButton";
            this.TzButton.Size = new System.Drawing.Size(24, 23);
            this.TzButton.TabIndex = 4;
            this.TzButton.Text = "Z";
            this.TzButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TzButton.UseVisualStyleBackColor = true;
            this.TzButton.CheckedChanged += new System.EventHandler(this.TzButton_CheckedChanged);
            // 
            // TyButton
            // 
            this.TyButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.TyButton.Checked = true;
            this.TyButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TyButton.Location = new System.Drawing.Point(32, 28);
            this.TyButton.Name = "TyButton";
            this.TyButton.Size = new System.Drawing.Size(25, 23);
            this.TyButton.TabIndex = 3;
            this.TyButton.Text = "Y";
            this.TyButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TyButton.UseVisualStyleBackColor = true;
            this.TyButton.CheckedChanged += new System.EventHandler(this.TyButton_CheckedChanged);
            // 
            // TxButton
            // 
            this.TxButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.TxButton.Checked = true;
            this.TxButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TxButton.Location = new System.Drawing.Point(7, 28);
            this.TxButton.Name = "TxButton";
            this.TxButton.Size = new System.Drawing.Size(24, 23);
            this.TxButton.TabIndex = 2;
            this.TxButton.Text = "X";
            this.TxButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TxButton.UseVisualStyleBackColor = true;
            this.TxButton.CheckedChanged += new System.EventHandler(this.TxButton_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "回転";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "移動";
            // 
            // _frameBarPlaceHolder
            // 
            this._frameBarPlaceHolder.BackColor = System.Drawing.SystemColors.Control;
            this._frameBarPlaceHolder.Location = new System.Drawing.Point(100, 6);
            this._frameBarPlaceHolder.Name = "_frameBarPlaceHolder";
            this._frameBarPlaceHolder.Size = new System.Drawing.Size(292, 72);
            this._frameBarPlaceHolder.TabIndex = 1;
            // 
            // _graphViewPlaceHolder
            // 
            this._graphViewPlaceHolder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._graphViewPlaceHolder.BackColor = System.Drawing.SystemColors.Control;
            this._graphViewPlaceHolder.Location = new System.Drawing.Point(100, 84);
            this._graphViewPlaceHolder.Name = "_graphViewPlaceHolder";
            this._graphViewPlaceHolder.Size = new System.Drawing.Size(292, 161);
            this._graphViewPlaceHolder.TabIndex = 2;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(395, 84);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 161);
            this.vScrollBar1.TabIndex = 3;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(100, 248);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(292, 17);
            this.hScrollBar1.TabIndex = 4;
            // 
            // RootControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this._graphViewPlaceHolder);
            this.Controls.Add(this._frameBarPlaceHolder);
            this.Controls.Add(this.leftPanel);
            this.Name = "RootControl";
            this.Size = new System.Drawing.Size(411, 269);
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox RzButton;
        private System.Windows.Forms.CheckBox RyButton;
        private System.Windows.Forms.CheckBox RxButton;
        private System.Windows.Forms.CheckBox TzButton;
        private System.Windows.Forms.CheckBox TyButton;
        private System.Windows.Forms.CheckBox TxButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Panel _frameBarPlaceHolder;
        internal System.Windows.Forms.Panel _graphViewPlaceHolder;
        internal System.Windows.Forms.VScrollBar vScrollBar1;
        internal System.Windows.Forms.HScrollBar hScrollBar1;
        internal System.Windows.Forms.Panel leftPanel;
    }
}
