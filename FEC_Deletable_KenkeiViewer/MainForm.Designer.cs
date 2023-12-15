namespace FEC_Deletable_KenkeiViewer
{
    partial class MainForm
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

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.bFolder = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.bDelete = new System.Windows.Forms.Button();
            this.bDeleteBack = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView = new FEC_Michiten_ClassLibrary.UserCtrl.ListView();
            this.webViewCustom = new FEC_Michiten_ClassLibrary.UserCtrl.WebViewCustom();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.imageView = new FEC_Michiten_ClassLibrary.UserCtrl.ImageView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(711, 510);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(15, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "フォルダ読込";
            // 
            // txtFolder
            // 
            this.txtFolder.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtFolder.Location = new System.Drawing.Point(107, 12);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(451, 23);
            this.txtFolder.TabIndex = 1;
            // 
            // bFolder
            // 
            this.bFolder.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bFolder.Location = new System.Drawing.Point(564, 8);
            this.bFolder.Name = "bFolder";
            this.bFolder.Size = new System.Drawing.Size(75, 35);
            this.bFolder.TabIndex = 2;
            this.bFolder.Text = "読込";
            this.bFolder.UseVisualStyleBackColor = true;
            this.bFolder.Click += new System.EventHandler(this.bFolder_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(304, 65);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 16);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // bDelete
            // 
            this.bDelete.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bDelete.Location = new System.Drawing.Point(46, 655);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 71);
            this.bDelete.TabIndex = 0;
            this.bDelete.Text = "削除";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bDeleteBack
            // 
            this.bDeleteBack.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bDeleteBack.Location = new System.Drawing.Point(163, 655);
            this.bDeleteBack.Name = "bDeleteBack";
            this.bDeleteBack.Size = new System.Drawing.Size(75, 71);
            this.bDeleteBack.TabIndex = 0;
            this.bDeleteBack.Text = "削除\r\nしない";
            this.bDeleteBack.UseVisualStyleBackColor = true;
            this.bDeleteBack.Click += new System.EventHandler(this.bDeleteBack_Click);
            // 
            // bSave
            // 
            this.bSave.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.bSave.Location = new System.Drawing.Point(277, 655);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(75, 71);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(712, 8);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webViewCustom);
            this.splitContainer1.Size = new System.Drawing.Size(708, 797);
            this.splitContainer1.SplitterDistance = 455;
            this.splitContainer1.TabIndex = 6;
            // 
            // listView
            // 
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(4);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(708, 455);
            this.listView.TabIndex = 0;
            this.listView.ListSelectChanged += new System.EventHandler(this.listView_ListSelectChanged);
            this.listView.ListcCellDClickEvent += new System.Windows.Forms.DataGridViewCellEventHandler(this.listView_ListcCellDClickEvent);
            this.listView.ListCellEndEditEvent += new FEC_Michiten_ClassLibrary.UserCtrl.ListView.ListCellEndEditHandler(this.listView_ListCellEndEditEvent);
            // 
            // webViewCustom
            // 
            this.webViewCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewCustom.Location = new System.Drawing.Point(0, 0);
            this.webViewCustom.Name = "webViewCustom";
            this.webViewCustom.Size = new System.Drawing.Size(708, 338);
            this.webViewCustom.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(251, 748);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(455, 23);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(160, 751);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "検索してね";
            // 
            // imageView
            // 
            this.imageView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageView.Location = new System.Drawing.Point(12, 49);
            this.imageView.Margin = new System.Windows.Forms.Padding(4);
            this.imageView.Name = "imageView";
            this.imageView.Size = new System.Drawing.Size(694, 586);
            this.imageView.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1422, 817);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bDeleteBack);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.imageView);
            this.Controls.Add(this.bFolder);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "すべてを消してくれるわ";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FEC_Michiten_ClassLibrary.UserCtrl.ListView listView;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button bFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private FEC_Michiten_ClassLibrary.UserCtrl.ImageView imageView;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bDeleteBack;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private FEC_Michiten_ClassLibrary.UserCtrl.WebViewCustom webViewCustom;
    }
}

