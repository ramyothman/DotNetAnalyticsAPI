namespace DesktopTest
{
    partial class Browser
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGetXML = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.webBrowser1 = new Awesomium.Windows.Forms.WebControl(this.components);
            this.webSessionProvider1 = new Awesomium.Windows.Forms.WebSessionProvider(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGetXML);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(971, 33);
            this.panel1.TabIndex = 0;
            // 
            // btnGetXML
            // 
            this.btnGetXML.Enabled = false;
            this.btnGetXML.Location = new System.Drawing.Point(857, 5);
            this.btnGetXML.Name = "btnGetXML";
            this.btnGetXML.Size = new System.Drawing.Size(75, 23);
            this.btnGetXML.TabIndex = 2;
            this.btnGetXML.Text = "Get XML";
            this.btnGetXML.UseVisualStyleBackColor = true;
            this.btnGetXML.Click += new System.EventHandler(this.btnGetXML_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(776, 4);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(12, 7);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(758, 20);
            this.txtAddress.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.webBrowser1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 33);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(971, 709);
            this.panel2.TabIndex = 1;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Size = new System.Drawing.Size(971, 709);
            this.webBrowser1.TabIndex = 0;
            
            this.webBrowser1.DocumentReady += new Awesomium.Core.DocumentReadyEventHandler(this.Awesomium_Windows_Forms_WebControl_DocumentReady);
            this.webBrowser1.LoadingFrameComplete += new Awesomium.Core.FrameEventHandler(this.Awesomium_Windows_Forms_WebControl_LoadingFrameComplete);
            // 
            // webSessionProvider1
            // 
            this.webSessionProvider1.Views.Add(this.webBrowser1);
            // 
            // Browser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 742);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Browser";
            this.Text = "Browser";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGetXML;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Panel panel2;
        
        private Awesomium.Windows.Forms.WebControl webBrowser1;
        private Awesomium.Windows.Forms.WebSessionProvider webSessionProvider1;
    }
}

