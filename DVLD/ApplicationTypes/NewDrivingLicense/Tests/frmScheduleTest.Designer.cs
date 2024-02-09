namespace DVLD.ApplicationTypes.NewDrivingLicense
{
    partial class frmScheduleTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScheduleTest));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnExist = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlScheduleTest11 = new DVLD.Controls.ctrlScheduleTest1();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ctrlScheduleTest11);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 618);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Vision Test";
            // 
            // btnExist
            // 
            this.btnExist.BackColor = System.Drawing.Color.Red;
            this.btnExist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExist.ForeColor = System.Drawing.SystemColors.Control;
            this.btnExist.Location = new System.Drawing.Point(442, 12);
            this.btnExist.Name = "btnExist";
            this.btnExist.Size = new System.Drawing.Size(28, 23);
            this.btnExist.TabIndex = 25;
            this.btnExist.Text = "X";
            this.btnExist.UseVisualStyleBackColor = false;
            this.btnExist.Click += new System.EventHandler(this.btnExist_Click);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(180, 668);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(118, 38);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlScheduleTest11
            // 
            this.ctrlScheduleTest11.ApplicationDate = new System.DateTime(((long)(0)));
            this.ctrlScheduleTest11.Location = new System.Drawing.Point(6, 19);
            this.ctrlScheduleTest11.Name = "ctrlScheduleTest11";
            this.ctrlScheduleTest11.PaidFees = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ctrlScheduleTest11.Size = new System.Drawing.Size(465, 619);
            this.ctrlScheduleTest11.TabIndex = 0;
            // 
            // frmScheduleTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 718);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnExist);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmScheduleTest";
            this.Text = "frmScheduleTest";
            this.Load += new System.EventHandler(this.frmScheduleTest_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExist;
        private System.Windows.Forms.Button btnClose;
        private Controls.ctrlScheduleTest1 ctrlScheduleTest11;
    }
}