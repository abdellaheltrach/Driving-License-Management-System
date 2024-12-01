namespace DVLD.Applications.Local_Driving_License_Application
{
    partial class frmLocalDrivingLicenseApplicationInfo
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
            this.btnClose = new System.Windows.Forms.Button();
            this.ctrlDrivingLicenseAplicationInfo1 = new DVLD.Applications.Local_Driving_License_Application.Control.ctrlDrivingLicenseAplicationInfo();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Image = global::DVLD.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(863, 336);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(126, 37);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ctrlDrivingLicenseAplicationInfo1
            // 
            this.ctrlDrivingLicenseAplicationInfo1.Location = new System.Drawing.Point(3, 2);
            this.ctrlDrivingLicenseAplicationInfo1.Name = "ctrlDrivingLicenseAplicationInfo1";
            this.ctrlDrivingLicenseAplicationInfo1.Size = new System.Drawing.Size(986, 356);
            this.ctrlDrivingLicenseAplicationInfo1.TabIndex = 0;
            this.ctrlDrivingLicenseAplicationInfo1.Load += new System.EventHandler(this.ctrlDrivingLicenseAplicationInfo1_Load);
            // 
            // frmLocalDrivingLicenseApplicationInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 378);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlDrivingLicenseAplicationInfo1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmLocalDrivingLicenseApplicationInfo";
            this.Text = "frmLocalDrivingLicenseApplicationInfo";
            this.ResumeLayout(false);

        }

        #endregion

        private Control.ctrlDrivingLicenseAplicationInfo ctrlDrivingLicenseAplicationInfo1;
        private System.Windows.Forms.Button btnClose;
    }
}