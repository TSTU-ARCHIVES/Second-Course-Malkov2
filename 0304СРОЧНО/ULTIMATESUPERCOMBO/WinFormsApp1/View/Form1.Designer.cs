namespace View
{
    partial class ViewForm
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
            DFS_button = new Button();
            BFS_button = new Button();
            SuspendLayout();
            // 
            // DFS_button
            // 
            DFS_button.Location = new Point(820, 10);
            DFS_button.Name = "DFS_button";
            DFS_button.Size = new Size(162, 46);
            DFS_button.TabIndex = 0;
            DFS_button.Text = "DFS";
            DFS_button.UseVisualStyleBackColor = true;
            DFS_button.Click += DFS_button_Click;
            // 
            // BFS_button
            // 
            BFS_button.Location = new Point(820, 62);
            BFS_button.Name = "BFS_button";
            BFS_button.Size = new Size(162, 46);
            BFS_button.TabIndex = 1;
            BFS_button.Text = "BFS";
            BFS_button.UseVisualStyleBackColor = true;
            BFS_button.Click += BFS_button_Click;
            // 
            // ViewForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(994, 450);
            Controls.Add(BFS_button);
            Controls.Add(DFS_button);
            Name = "ViewForm";
            Text = "Form1";
            MouseDoubleClick += ViewForm_MouseDoubleClick;
            MouseDown += ViewForm_MouseDown;
            MouseMove += ViewForm_MouseMove;
            MouseUp += ViewForm_MouseUp;
            ResumeLayout(false);
        }

        #endregion

        private Button DFS_button;
        private Button BFS_button;
    }
}
