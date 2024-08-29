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
            DFS_cycle_button = new Button();
            RobertsFlores_button = new Button();
            Clear_button = new Button();
            label1 = new Label();
            actions_gb = new GroupBox();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            button1 = new Button();
            graph_string = new Label();
            button7 = new Button();
            actions_gb.SuspendLayout();
            SuspendLayout();
            // 
            // DFS_button
            // 
            DFS_button.Location = new Point(6, 26);
            DFS_button.Name = "DFS_button";
            DFS_button.Size = new Size(151, 49);
            DFS_button.TabIndex = 0;
            DFS_button.Text = "dfs";
            DFS_button.UseVisualStyleBackColor = true;
            DFS_button.Click += DFS_button_Click;
            // 
            // BFS_button
            // 
            BFS_button.Location = new Point(6, 83);
            BFS_button.Name = "BFS_button";
            BFS_button.Size = new Size(151, 49);
            BFS_button.TabIndex = 1;
            BFS_button.Text = "bfs";
            BFS_button.UseVisualStyleBackColor = true;
            BFS_button.Click += BFS_button_Click;
            // 
            // DFS_cycle_button
            // 
            DFS_cycle_button.Location = new Point(163, 26);
            DFS_cycle_button.Name = "DFS_cycle_button";
            DFS_cycle_button.Size = new Size(151, 49);
            DFS_cycle_button.TabIndex = 2;
            DFS_cycle_button.Text = "find cycle";
            DFS_cycle_button.UseVisualStyleBackColor = true;
            DFS_cycle_button.Click += DFS_cycle_button_Click;
            // 
            // RobertsFlores_button
            // 
            RobertsFlores_button.Location = new Point(320, 26);
            RobertsFlores_button.Name = "RobertsFlores_button";
            RobertsFlores_button.Size = new Size(151, 49);
            RobertsFlores_button.TabIndex = 3;
            RobertsFlores_button.Text = "roberts-flores hamiltonyan path";
            RobertsFlores_button.UseVisualStyleBackColor = true;
            RobertsFlores_button.Click += RobertsFlores_button_Click;
            // 
            // Clear_button
            // 
            Clear_button.Location = new Point(972, 426);
            Clear_button.Name = "Clear_button";
            Clear_button.Size = new Size(115, 107);
            Clear_button.TabIndex = 4;
            Clear_button.Text = "clear";
            Clear_button.UseVisualStyleBackColor = true;
            Clear_button.Click += Clear_button_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(893, 9);
            label1.Name = "label1";
            label1.Size = new Size(146, 20);
            label1.TabIndex = 5;
            label1.Text = "Graph adjastency list";
            // 
            // actions_gb
            // 
            actions_gb.Controls.Add(button7);
            actions_gb.Controls.Add(button6);
            actions_gb.Controls.Add(button5);
            actions_gb.Controls.Add(button4);
            actions_gb.Controls.Add(button3);
            actions_gb.Controls.Add(button2);
            actions_gb.Controls.Add(button1);
            actions_gb.Controls.Add(DFS_cycle_button);
            actions_gb.Controls.Add(DFS_button);
            actions_gb.Controls.Add(BFS_button);
            actions_gb.Controls.Add(RobertsFlores_button);
            actions_gb.Location = new Point(12, 400);
            actions_gb.Name = "actions_gb";
            actions_gb.Size = new Size(954, 155);
            actions_gb.TabIndex = 6;
            actions_gb.TabStop = false;
            actions_gb.Text = "actions";
            // 
            // button6
            // 
            button6.Location = new Point(477, 81);
            button6.Name = "button6";
            button6.Size = new Size(151, 49);
            button6.TabIndex = 10;
            button6.Text = "ford";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button5
            // 
            button5.Location = new Point(477, 26);
            button5.Name = "button5";
            button5.Size = new Size(151, 49);
            button5.TabIndex = 9;
            button5.Text = "floyd";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button4
            // 
            button4.Location = new Point(320, 81);
            button4.Name = "button4";
            button4.Size = new Size(151, 49);
            button4.TabIndex = 8;
            button4.Text = "dijkstra";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // button3
            // 
            button3.Location = new Point(163, 83);
            button3.Name = "button3";
            button3.Size = new Size(151, 49);
            button3.TabIndex = 7;
            button3.Text = "fleury euler path";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.Location = new Point(791, 26);
            button2.Name = "button2";
            button2.Size = new Size(151, 49);
            button2.TabIndex = 6;
            button2.Text = "prim mst";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button1
            // 
            button1.Location = new Point(791, 84);
            button1.Name = "button1";
            button1.Size = new Size(151, 49);
            button1.TabIndex = 5;
            button1.Text = "kruskal mst";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // graph_string
            // 
            graph_string.AutoSize = true;
            graph_string.Location = new Point(870, 44);
            graph_string.Name = "graph_string";
            graph_string.Size = new Size(0, 20);
            graph_string.TabIndex = 7;
            // 
            // button7
            // 
            button7.Location = new Point(634, 55);
            button7.Name = "button7";
            button7.Size = new Size(151, 49);
            button7.TabIndex = 11;
            button7.Text = "FIND EXIT";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // ViewForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1099, 567);
            Controls.Add(graph_string);
            Controls.Add(actions_gb);
            Controls.Add(label1);
            Controls.Add(Clear_button);
            Name = "ViewForm";
            Text = "Form1";
            MouseDown += ViewForm_MouseDown;
            MouseUp += ViewForm_MouseUp;
            actions_gb.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button DFS_button;
        private Button BFS_button;
        private Button DFS_cycle_button;
        private Button RobertsFlores_button;
        private Button Clear_button;
        private Label label1;
        private GroupBox actions_gb;
        private Label graph_string;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button button7;
    }
}
