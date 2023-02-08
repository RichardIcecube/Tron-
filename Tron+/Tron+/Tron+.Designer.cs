namespace Tron_
{
    partial class Form1
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
            this.uxBoard = new System.Windows.Forms.PictureBox();
            this.uxTimer = new System.Windows.Forms.Timer(this.components);
            this.uxResetTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.uxBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // uxBoard
            // 
            this.uxBoard.Location = new System.Drawing.Point(0, 0);
            this.uxBoard.Name = "uxBoard";
            this.uxBoard.Size = new System.Drawing.Size(644, 497);
            this.uxBoard.TabIndex = 0;
            this.uxBoard.TabStop = false;
            this.uxBoard.Paint += new System.Windows.Forms.PaintEventHandler(this.uxBoard_Paint);
            // 
            // uxTimer
            // 
            this.uxTimer.Tick += new System.EventHandler(this.UpdateScreen);
            // 
            // uxResetTimer
            // 
            this.uxResetTimer.Tick += new System.EventHandler(this.Reset);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 494);
            this.Controls.Add(this.uxBoard);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Tron+";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.uxBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox uxBoard;
        private System.Windows.Forms.Timer uxTimer;
        private System.Windows.Forms.Timer uxResetTimer;
    }
}

