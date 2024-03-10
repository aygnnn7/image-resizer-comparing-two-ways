namespace ImageResizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pb = new PictureBox();
            ResizeBtn = new Button();
            InfoLbl = new Label();
            panel1 = new Panel();
            TypeLbl = new Label();
            TypeCB = new ComboBox();
            imgNameLbl = new Label();
            SelectImageBtn = new Button();
            label1 = new Label();
            percentageLbl = new Label();
            percentageTB = new TextBox();
            SelectImageDialog = new OpenFileDialog();
            InfoPnl = new Panel();
            NextTipBtn = new Button();
            InfoLbl2 = new Label();
            SaveBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)pb).BeginInit();
            panel1.SuspendLayout();
            InfoPnl.SuspendLayout();
            SuspendLayout();
            // 
            // pb
            // 
            pb.BackColor = SystemColors.Window;
            pb.BorderStyle = BorderStyle.Fixed3D;
            pb.Image = Properties.Resources._default;
            pb.Location = new Point(28, 36);
            pb.Name = "pb";
            pb.Size = new Size(476, 264);
            pb.SizeMode = PictureBoxSizeMode.CenterImage;
            pb.TabIndex = 0;
            pb.TabStop = false;
            // 
            // ResizeBtn
            // 
            ResizeBtn.Cursor = Cursors.Hand;
            ResizeBtn.Font = new Font("Constantia", 9F);
            ResizeBtn.Location = new Point(529, 226);
            ResizeBtn.Name = "ResizeBtn";
            ResizeBtn.Size = new Size(229, 34);
            ResizeBtn.TabIndex = 1;
            ResizeBtn.Text = "Resize";
            ResizeBtn.UseVisualStyleBackColor = true;
            ResizeBtn.Click += ResizeBtn_Click;
            // 
            // InfoLbl
            // 
            InfoLbl.AutoSize = true;
            InfoLbl.Font = new Font("Constantia", 9F, FontStyle.Bold);
            InfoLbl.Location = new Point(24, 95);
            InfoLbl.Name = "InfoLbl";
            InfoLbl.Size = new Size(112, 22);
            InfoLbl.TabIndex = 3;
            InfoLbl.Text = "Percentage:";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.GradientActiveCaption;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(TypeLbl);
            panel1.Controls.Add(TypeCB);
            panel1.Controls.Add(imgNameLbl);
            panel1.Controls.Add(SelectImageBtn);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(InfoLbl);
            panel1.Controls.Add(percentageLbl);
            panel1.Controls.Add(percentageTB);
            panel1.Location = new Point(529, 36);
            panel1.Name = "panel1";
            panel1.Size = new Size(229, 184);
            panel1.TabIndex = 4;
            // 
            // TypeLbl
            // 
            TypeLbl.AutoSize = true;
            TypeLbl.Font = new Font("Constantia", 9F, FontStyle.Bold);
            TypeLbl.Location = new Point(24, 134);
            TypeLbl.Name = "TypeLbl";
            TypeLbl.Size = new Size(55, 22);
            TypeLbl.TabIndex = 8;
            TypeLbl.Text = "Type:";
            // 
            // TypeCB
            // 
            TypeCB.FormattingEnabled = true;
            TypeCB.Items.AddRange(new object[] { "Sequential", "Parallel" });
            TypeCB.Location = new Point(85, 128);
            TypeCB.Name = "TypeCB";
            TypeCB.Size = new Size(121, 33);
            TypeCB.TabIndex = 7;
            TypeCB.Text = "Sequential";
            // 
            // imgNameLbl
            // 
            imgNameLbl.AutoSize = true;
            imgNameLbl.Font = new Font("Segoe UI", 8F);
            imgNameLbl.Location = new Point(99, 18);
            imgNameLbl.Name = "imgNameLbl";
            imgNameLbl.Size = new Size(103, 21);
            imgNameLbl.TabIndex = 7;
            imgNameLbl.Text = "No image yet";
            // 
            // SelectImageBtn
            // 
            SelectImageBtn.Font = new Font("Constantia", 9F);
            SelectImageBtn.Location = new Point(24, 48);
            SelectImageBtn.Name = "SelectImageBtn";
            SelectImageBtn.Size = new Size(182, 34);
            SelectImageBtn.TabIndex = 5;
            SelectImageBtn.Text = "Select";
            SelectImageBtn.UseVisualStyleBackColor = true;
            SelectImageBtn.Click += SelectImageBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Constantia", 9F, FontStyle.Bold);
            label1.Location = new Point(24, 17);
            label1.Name = "label1";
            label1.Size = new Size(69, 22);
            label1.TabIndex = 6;
            label1.Text = "Image:";
            // 
            // percentageLbl
            // 
            percentageLbl.AutoSize = true;
            percentageLbl.Location = new Point(179, 95);
            percentageLbl.Name = "percentageLbl";
            percentageLbl.Size = new Size(27, 25);
            percentageLbl.TabIndex = 5;
            percentageLbl.Text = "%";
            // 
            // percentageTB
            // 
            percentageTB.Font = new Font("Constantia", 9F);
            percentageTB.Location = new Point(142, 92);
            percentageTB.Name = "percentageTB";
            percentageTB.Size = new Size(41, 29);
            percentageTB.TabIndex = 4;
            percentageTB.Text = "100";
            // 
            // SelectImageDialog
            // 
            SelectImageDialog.FileName = "SelectImageDialog";
            // 
            // InfoPnl
            // 
            InfoPnl.BackColor = SystemColors.Info;
            InfoPnl.BorderStyle = BorderStyle.FixedSingle;
            InfoPnl.Controls.Add(NextTipBtn);
            InfoPnl.Controls.Add(InfoLbl2);
            InfoPnl.Location = new Point(28, 330);
            InfoPnl.Name = "InfoPnl";
            InfoPnl.Size = new Size(730, 85);
            InfoPnl.TabIndex = 5;
            InfoPnl.Visible = false;
            // 
            // NextTipBtn
            // 
            NextTipBtn.BackColor = Color.Orange;
            NextTipBtn.Font = new Font("Segoe UI", 8F);
            NextTipBtn.ForeColor = SystemColors.AppWorkspace;
            NextTipBtn.Image = (Image)resources.GetObject("NextTipBtn.Image");
            NextTipBtn.Location = new Point(680, 3);
            NextTipBtn.Name = "NextTipBtn";
            NextTipBtn.Size = new Size(41, 40);
            NextTipBtn.TabIndex = 1;
            NextTipBtn.UseVisualStyleBackColor = false;
            NextTipBtn.Click += NextTipBtn_Click;
            // 
            // InfoLbl2
            // 
            InfoLbl2.AutoSize = true;
            InfoLbl2.Font = new Font("Constantia", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            InfoLbl2.Location = new Point(25, 31);
            InfoLbl2.Name = "InfoLbl2";
            InfoLbl2.Size = new Size(0, 22);
            InfoLbl2.TabIndex = 0;
            // 
            // SaveBtn
            // 
            SaveBtn.Enabled = false;
            SaveBtn.Location = new Point(529, 266);
            SaveBtn.Name = "SaveBtn";
            SaveBtn.Size = new Size(229, 34);
            SaveBtn.TabIndex = 6;
            SaveBtn.Text = "Save Downsized";
            SaveBtn.UseVisualStyleBackColor = true;
            SaveBtn.Click += SaveBtn_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(SaveBtn);
            Controls.Add(InfoPnl);
            Controls.Add(panel1);
            Controls.Add(pb);
            Controls.Add(ResizeBtn);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pb).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            InfoPnl.ResumeLayout(false);
            InfoPnl.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        public PictureBox pb;
        public Button ResizeBtn;
        public Label InfoLbl;
        public Panel panel1;
        public Label percentageLbl;
        public TextBox percentageTB;
        public OpenFileDialog SelectImageDialog;
        public Button SelectImageBtn;
        public Label label1;
        public Panel InfoPnl;
        public Label InfoLbl2;
        public Button SaveBtn;
        public Label imgNameLbl;
        public ComboBox TypeCB;
        public Label TypeLbl;
        private Button NextTipBtn;
    }
}
