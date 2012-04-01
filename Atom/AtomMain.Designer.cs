namespace Atom
{
    partial class AtomMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AtomMain));
            this.listForm = new System.Windows.Forms.ListBox();
            this.incomingData = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listForm
            // 
            this.listForm.FormattingEnabled = true;
            this.listForm.Location = new System.Drawing.Point(12, 12);
            this.listForm.Name = "listForm";
            this.listForm.Size = new System.Drawing.Size(287, 446);
            this.listForm.TabIndex = 0;
            this.listForm.DoubleClick += new System.EventHandler(this.fileList_DoubleClick);
            // 
            // incomingData
            // 
            this.incomingData.Location = new System.Drawing.Point(305, 12);
            this.incomingData.Multiline = true;
            this.incomingData.Name = "incomingData";
            this.incomingData.Size = new System.Drawing.Size(319, 446);
            this.incomingData.TabIndex = 1;
            this.incomingData.TextChanged += new System.EventHandler(this.incomingData_TextChanged);
            // 
            // AtomMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 477);
            this.Controls.Add(this.incomingData);
            this.Controls.Add(this.listForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AtomMain";
            this.Text = "Atom.io";
            this.Load += new System.EventHandler(this.AtomMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listForm;
        public System.Windows.Forms.TextBox incomingData;

    }
}

