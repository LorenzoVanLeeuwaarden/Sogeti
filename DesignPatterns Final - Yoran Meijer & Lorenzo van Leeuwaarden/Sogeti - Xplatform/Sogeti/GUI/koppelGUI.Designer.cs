namespace Sogeti
{
    partial class koppelGUI
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
            this.koppelVerwijderButton = new System.Windows.Forms.Button();
            this.koppelListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // koppelVerwijderButton
            // 
            this.koppelVerwijderButton.Location = new System.Drawing.Point(1, 269);
            this.koppelVerwijderButton.Name = "koppelVerwijderButton";
            this.koppelVerwijderButton.Size = new System.Drawing.Size(75, 23);
            this.koppelVerwijderButton.TabIndex = 1;
            this.koppelVerwijderButton.Text = "Delete";
            this.koppelVerwijderButton.UseVisualStyleBackColor = true;
            this.koppelVerwijderButton.Click += new System.EventHandler(this.koppelVerwijderButton_Click);
            // 
            // koppelListView
            // 
            this.koppelListView.Location = new System.Drawing.Point(1, 4);
            this.koppelListView.Name = "koppelListView";
            this.koppelListView.Size = new System.Drawing.Size(455, 259);
            this.koppelListView.TabIndex = 2;
            this.koppelListView.UseCompatibleStateImageBehavior = false;
            // 
            // koppelGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 295);
            this.Controls.Add(this.koppelListView);
            this.Controls.Add(this.koppelVerwijderButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "koppelGUI";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button koppelVerwijderButton;
        private System.Windows.Forms.ListView koppelListView;
    }
}