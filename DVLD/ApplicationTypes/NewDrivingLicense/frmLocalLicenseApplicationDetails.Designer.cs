namespace DVLD.ApplicationTypes.NewDrivingLicense
{
    partial class frmLocalLicenseApplicationDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLocalLicenseApplicationDetails));
            this.label1 = new System.Windows.Forms.Label();
            this.ctrlDrivingLicenseAppAndApplicationInfo1 = new DVLD.Controls.ctrlDrivingLicenseAppAndApplicationInfo();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(228, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(257, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application Details";
            // 
            // ctrlDrivingLicenseAppAndApplicationInfo1
            // 
            this.ctrlDrivingLicenseAppAndApplicationInfo1.Location = new System.Drawing.Point(12, 79);
            this.ctrlDrivingLicenseAppAndApplicationInfo1.Name = "ctrlDrivingLicenseAppAndApplicationInfo1";
            this.ctrlDrivingLicenseAppAndApplicationInfo1.Size = new System.Drawing.Size(726, 328);
            this.ctrlDrivingLicenseAppAndApplicationInfo1.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(623, 413);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(112, 33);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmLocalLicenseApplicationDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 461);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlDrivingLicenseAppAndApplicationInfo1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLocalLicenseApplicationDetails";
            this.Text = "frmLocalLicenseApplicationDetails";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private Controls.ctrlDrivingLicenseAppAndApplicationInfo ctrlDrivingLicenseAppAndApplicationInfo1;
        private System.Windows.Forms.Button btnClose;
    }
}