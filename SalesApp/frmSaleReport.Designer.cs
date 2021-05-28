namespace SalesApp
{
    partial class frmSaleReport
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
            this.btnSee = new System.Windows.Forms.Button();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.cRv = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.lblTitle = new System.Windows.Forms.Label();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.lblSSM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSee
            // 
            this.btnSee.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSee.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSee.Location = new System.Drawing.Point(431, 9);
            this.btnSee.Name = "btnSee";
            this.btnSee.Size = new System.Drawing.Size(75, 26);
            this.btnSee.TabIndex = 1;
            this.btnSee.Text = "View";
            this.btnSee.UseVisualStyleBackColor = true;
            this.btnSee.Click += new System.EventHandler(this.btnSee_Click);
            // 
            // startDate
            // 
            this.startDate.CustomFormat = "dd/MM/yyyy";
            this.startDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDate.Location = new System.Drawing.Point(4, 9);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(200, 26);
            this.startDate.TabIndex = 0;
            // 
            // cRv
            // 
            this.cRv.ActiveViewIndex = -1;
            this.cRv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cRv.Cursor = System.Windows.Forms.Cursors.Default;
            this.cRv.Location = new System.Drawing.Point(1, 39);
            this.cRv.Name = "cRv";
            this.cRv.Size = new System.Drawing.Size(1152, 587);
            this.cRv.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitle.Location = new System.Drawing.Point(823, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(66, 24);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "label1";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // endDate
            // 
            this.endDate.CustomFormat = "dd/MM/yyyy";
            this.endDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDate.Location = new System.Drawing.Point(225, 9);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(200, 26);
            this.endDate.TabIndex = 4;
            // 
            // lblSSM
            // 
            this.lblSSM.AutoSize = true;
            this.lblSSM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSSM.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblSSM.Location = new System.Drawing.Point(206, 9);
            this.lblSSM.Name = "lblSSM";
            this.lblSSM.Size = new System.Drawing.Size(17, 24);
            this.lblSSM.TabIndex = 5;
            this.lblSSM.Text = "-";
            this.lblSSM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmSaleReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SandyBrown;
            this.ClientSize = new System.Drawing.Size(1152, 624);
            this.Controls.Add(this.lblSSM);
            this.Controls.Add(this.endDate);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cRv);
            this.Controls.Add(this.btnSee);
            this.Controls.Add(this.startDate);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSaleReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "SalesDetailForm";
            this.Load += new System.EventHandler(this.SalesDetailForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSee;
        private System.Windows.Forms.DateTimePicker startDate;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer cRv;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.Label lblSSM;
    }
}