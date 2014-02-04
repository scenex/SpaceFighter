namespace Editor
{
    partial class MainForm
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
            this.editorDisplay1 = new Editor.CustomControls.EditorDisplay();
            this.SuspendLayout();
            // 
            // editorDisplay1
            // 
            this.editorDisplay1.Location = new System.Drawing.Point(12, 12);
            this.editorDisplay1.Name = "editorDisplay1";
            this.editorDisplay1.Size = new System.Drawing.Size(729, 454);
            this.editorDisplay1.TabIndex = 0;
            this.editorDisplay1.Text = "editorDisplay1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 478);
            this.Controls.Add(this.editorDisplay1);
            this.Name = "MainForm";
            this.Text = "Editor";
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.EditorDisplay editorDisplay1;
    }
}