namespace Cool.Messages.Proxy.Legacy
{
    partial class ProxyForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing )
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.rtb_out = new System.Windows.Forms.RichTextBox();
            this.tb_xpub = new System.Windows.Forms.TextBox();
            this.label_xpub = new System.Windows.Forms.Label();
            this.label_xsub = new System.Windows.Forms.Label();
            this.tb_xsub = new System.Windows.Forms.TextBox();
            this.label_addr = new System.Windows.Forms.Label();
            this.tb_addr = new System.Windows.Forms.TextBox();
            this.btn_onoff = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtb_out
            // 
            this.rtb_out.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtb_out.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.rtb_out.Location = new System.Drawing.Point(44, 72);
            this.rtb_out.Name = "rtb_out";
            this.rtb_out.ReadOnly = true;
            this.rtb_out.Size = new System.Drawing.Size(322, 61);
            this.rtb_out.TabIndex = 1;
            this.rtb_out.Text = "";
            // 
            // tb_xpub
            // 
            this.tb_xpub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_xpub.Location = new System.Drawing.Point(101, 37);
            this.tb_xpub.Name = "tb_xpub";
            this.tb_xpub.Size = new System.Drawing.Size(100, 21);
            this.tb_xpub.TabIndex = 2;
            this.tb_xpub.Text = "8111";
            // 
            // label_xpub
            // 
            this.label_xpub.AutoSize = true;
            this.label_xpub.Location = new System.Drawing.Point(42, 40);
            this.label_xpub.Name = "label_xpub";
            this.label_xpub.Size = new System.Drawing.Size(53, 12);
            this.label_xpub.TabIndex = 3;
            this.label_xpub.Text = "XPubPort";
            // 
            // label_xsub
            // 
            this.label_xsub.AutoSize = true;
            this.label_xsub.Location = new System.Drawing.Point(207, 40);
            this.label_xsub.Name = "label_xsub";
            this.label_xsub.Size = new System.Drawing.Size(53, 12);
            this.label_xsub.TabIndex = 5;
            this.label_xsub.Text = "XSubPort";
            // 
            // tb_xsub
            // 
            this.tb_xsub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tb_xsub.Location = new System.Drawing.Point(266, 37);
            this.tb_xsub.Name = "tb_xsub";
            this.tb_xsub.Size = new System.Drawing.Size(100, 21);
            this.tb_xsub.TabIndex = 4;
            this.tb_xsub.Text = "8222";
            // 
            // label_addr
            // 
            this.label_addr.AutoSize = true;
            this.label_addr.Location = new System.Drawing.Point(42, 9);
            this.label_addr.Name = "label_addr";
            this.label_addr.Size = new System.Drawing.Size(47, 12);
            this.label_addr.TabIndex = 7;
            this.label_addr.Text = "Address";
            // 
            // tb_addr
            // 
            this.tb_addr.Location = new System.Drawing.Point(101, 6);
            this.tb_addr.Name = "tb_addr";
            this.tb_addr.Size = new System.Drawing.Size(265, 21);
            this.tb_addr.TabIndex = 6;
            this.tb_addr.Text = "0.0.0.0";
            // 
            // btn_onoff
            // 
            this.btn_onoff.Location = new System.Drawing.Point(387, 12);
            this.btn_onoff.Name = "btn_onoff";
            this.btn_onoff.Size = new System.Drawing.Size(62, 27);
            this.btn_onoff.TabIndex = 8;
            this.btn_onoff.Text = "On/Off";
            this.btn_onoff.UseVisualStyleBackColor = true;
            this.btn_onoff.Click += new System.EventHandler(this.btn_onoff_Click);
            // 
            // ProxyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 145);
            this.Controls.Add(this.btn_onoff);
            this.Controls.Add(this.label_addr);
            this.Controls.Add(this.tb_addr);
            this.Controls.Add(this.label_xsub);
            this.Controls.Add(this.tb_xsub);
            this.Controls.Add(this.label_xpub);
            this.Controls.Add(this.tb_xpub);
            this.Controls.Add(this.rtb_out);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProxyForm";
            this.Text = "MessageProxy";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProxyForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtb_out;
        private System.Windows.Forms.TextBox tb_xpub;
        private System.Windows.Forms.Label label_xpub;
        private System.Windows.Forms.Label label_xsub;
        private System.Windows.Forms.TextBox tb_xsub;
        private System.Windows.Forms.Label label_addr;
        private System.Windows.Forms.TextBox tb_addr;
        private System.Windows.Forms.Button btn_onoff;
    }
}

