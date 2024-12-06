namespace MazeProject
{
    partial class MainForm
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
            pictureBox = new PictureBox();
            buttonStartSimulation = new Button();
            buttonGenerateMaze = new Button();
            numericMazeWidth = new NumericUpDown();
            label1 = new Label();
            label2 = new Label();
            numericMazeHeight = new NumericUpDown();
            label3 = new Label();
            numericNoAgents = new NumericUpDown();
            label4 = new Label();
            numericMazeSeed = new NumericUpDown();
            buttonStopSimulation = new Button();
            label5 = new Label();
            label6 = new Label();
            labelTurnsToFindExit = new Label();
            labelTimeToFindExit = new Label();
            label8 = new Label();
            numericTurnTime = new NumericUpDown();
            checkBoxAnimations = new CheckBox();
            checkBoxMazeWeights = new CheckBox();
            numericNoSimulations = new NumericUpDown();
            buttonStartMultipleSimulations = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMazeWidth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMazeHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericNoAgents).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericMazeSeed).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericTurnTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericNoSimulations).BeginInit();
            SuspendLayout();
            // 
            // pictureBox
            // 
            pictureBox.Location = new Point(12, 109);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(640, 620);
            pictureBox.TabIndex = 0;
            pictureBox.TabStop = false;
            pictureBox.Paint += pictureBox_Paint;
            // 
            // buttonStartSimulation
            // 
            buttonStartSimulation.Location = new Point(12, 14);
            buttonStartSimulation.Name = "buttonStartSimulation";
            buttonStartSimulation.Size = new Size(100, 23);
            buttonStartSimulation.TabIndex = 1;
            buttonStartSimulation.Text = "Start Simulation";
            buttonStartSimulation.UseVisualStyleBackColor = true;
            buttonStartSimulation.Click += buttonStartSimulation_Click;
            // 
            // buttonGenerateMaze
            // 
            buttonGenerateMaze.Location = new Point(12, 43);
            buttonGenerateMaze.Name = "buttonGenerateMaze";
            buttonGenerateMaze.Size = new Size(100, 23);
            buttonGenerateMaze.TabIndex = 2;
            buttonGenerateMaze.Text = "Generate Maze";
            buttonGenerateMaze.UseVisualStyleBackColor = true;
            buttonGenerateMaze.Click += buttonGenerateMaze_Click;
            // 
            // numericMazeWidth
            // 
            numericMazeWidth.Location = new Point(160, 43);
            numericMazeWidth.Margin = new Padding(0, 3, 3, 3);
            numericMazeWidth.Name = "numericMazeWidth";
            numericMazeWidth.Size = new Size(57, 23);
            numericMazeWidth.TabIndex = 3;
            numericMazeWidth.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(118, 47);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 4;
            label1.Text = "Width";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(223, 47);
            label2.Name = "label2";
            label2.Size = new Size(43, 15);
            label2.TabIndex = 6;
            label2.Text = "Height";
            // 
            // numericMazeHeight
            // 
            numericMazeHeight.Location = new Point(269, 43);
            numericMazeHeight.Margin = new Padding(0, 3, 3, 3);
            numericMazeHeight.Name = "numericMazeHeight";
            numericMazeHeight.Size = new Size(57, 23);
            numericMazeHeight.TabIndex = 5;
            numericMazeHeight.Value = new decimal(new int[] { 15, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(224, 18);
            label3.Name = "label3";
            label3.Size = new Size(105, 15);
            label3.TabIndex = 8;
            label3.Text = "Number of Agents";
            // 
            // numericNoAgents
            // 
            numericNoAgents.Location = new Point(332, 14);
            numericNoAgents.Margin = new Padding(0, 3, 3, 3);
            numericNoAgents.Name = "numericNoAgents";
            numericNoAgents.Size = new Size(54, 23);
            numericNoAgents.TabIndex = 7;
            numericNoAgents.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(332, 47);
            label4.Name = "label4";
            label4.Size = new Size(63, 15);
            label4.TabIndex = 10;
            label4.Text = "Maze Seed";
            // 
            // numericMazeSeed
            // 
            numericMazeSeed.Location = new Point(398, 43);
            numericMazeSeed.Margin = new Padding(0, 3, 3, 3);
            numericMazeSeed.Name = "numericMazeSeed";
            numericMazeSeed.Size = new Size(147, 23);
            numericMazeSeed.TabIndex = 9;
            // 
            // buttonStopSimulation
            // 
            buttonStopSimulation.Location = new Point(118, 14);
            buttonStopSimulation.Name = "buttonStopSimulation";
            buttonStopSimulation.Size = new Size(100, 23);
            buttonStopSimulation.TabIndex = 11;
            buttonStopSimulation.Text = "Stop Simulation";
            buttonStopSimulation.UseVisualStyleBackColor = true;
            buttonStopSimulation.Click += buttonStopSimulation_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(12, 88);
            label5.Margin = new Padding(3);
            label5.Name = "label5";
            label5.Size = new Size(105, 15);
            label5.TabIndex = 12;
            label5.Text = "Turns to find exit:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label6.Location = new Point(213, 88);
            label6.Margin = new Padding(3);
            label6.Name = "label6";
            label6.Size = new Size(103, 15);
            label6.TabIndex = 13;
            label6.Text = "Time to find exit:";
            // 
            // labelTurnsToFindExit
            // 
            labelTurnsToFindExit.AutoSize = true;
            labelTurnsToFindExit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelTurnsToFindExit.Location = new Point(117, 88);
            labelTurnsToFindExit.Margin = new Padding(3);
            labelTurnsToFindExit.Name = "labelTurnsToFindExit";
            labelTurnsToFindExit.Size = new Size(10, 15);
            labelTurnsToFindExit.TabIndex = 15;
            labelTurnsToFindExit.Text = " ";
            // 
            // labelTimeToFindExit
            // 
            labelTimeToFindExit.AutoSize = true;
            labelTimeToFindExit.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            labelTimeToFindExit.Location = new Point(315, 88);
            labelTimeToFindExit.Margin = new Padding(3);
            labelTimeToFindExit.Name = "labelTimeToFindExit";
            labelTimeToFindExit.Size = new Size(10, 15);
            labelTimeToFindExit.TabIndex = 16;
            labelTimeToFindExit.Text = " ";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(391, 18);
            label8.Name = "label8";
            label8.Size = new Size(87, 15);
            label8.TabIndex = 19;
            label8.Text = "Turn Time (ms)";
            // 
            // numericTurnTime
            // 
            numericTurnTime.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numericTurnTime.Location = new Point(483, 14);
            numericTurnTime.Margin = new Padding(0, 3, 3, 3);
            numericTurnTime.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            numericTurnTime.Name = "numericTurnTime";
            numericTurnTime.Size = new Size(62, 23);
            numericTurnTime.TabIndex = 18;
            numericTurnTime.Value = new decimal(new int[] { 1000, 0, 0, 0 });
            // 
            // checkBoxAnimations
            // 
            checkBoxAnimations.AutoSize = true;
            checkBoxAnimations.Checked = true;
            checkBoxAnimations.CheckState = CheckState.Checked;
            checkBoxAnimations.Location = new Point(562, 16);
            checkBoxAnimations.Name = "checkBoxAnimations";
            checkBoxAnimations.Size = new Size(87, 19);
            checkBoxAnimations.TabIndex = 20;
            checkBoxAnimations.Text = "Animations";
            checkBoxAnimations.UseVisualStyleBackColor = true;
            // 
            // checkBoxMazeWeights
            // 
            checkBoxMazeWeights.AutoSize = true;
            checkBoxMazeWeights.Checked = true;
            checkBoxMazeWeights.CheckState = CheckState.Checked;
            checkBoxMazeWeights.Location = new Point(562, 46);
            checkBoxMazeWeights.Name = "checkBoxMazeWeights";
            checkBoxMazeWeights.Size = new Size(97, 19);
            checkBoxMazeWeights.TabIndex = 21;
            checkBoxMazeWeights.Text = "View Weights";
            checkBoxMazeWeights.UseVisualStyleBackColor = true;
            checkBoxMazeWeights.CheckedChanged += checkBoxMazeWeights_CheckedChanged;
            // 
            // numericNoSimulations
            // 
            numericNoSimulations.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numericNoSimulations.Location = new Point(595, 80);
            numericNoSimulations.Margin = new Padding(0, 3, 3, 3);
            numericNoSimulations.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericNoSimulations.Name = "numericNoSimulations";
            numericNoSimulations.Size = new Size(54, 23);
            numericNoSimulations.TabIndex = 22;
            numericNoSimulations.Value = new decimal(new int[] { 10, 0, 0, 0 });
            // 
            // buttonStartMultipleSimulations
            // 
            buttonStartMultipleSimulations.Location = new Point(425, 80);
            buttonStartMultipleSimulations.Name = "buttonStartMultipleSimulations";
            buttonStartMultipleSimulations.Size = new Size(158, 23);
            buttonStartMultipleSimulations.TabIndex = 23;
            buttonStartMultipleSimulations.Text = "Start Multiple Simulations";
            buttonStartMultipleSimulations.UseVisualStyleBackColor = true;
            buttonStartMultipleSimulations.Click += buttonStartMultipleSimulations_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(664, 741);
            Controls.Add(buttonStartMultipleSimulations);
            Controls.Add(numericNoSimulations);
            Controls.Add(checkBoxMazeWeights);
            Controls.Add(checkBoxAnimations);
            Controls.Add(label8);
            Controls.Add(numericTurnTime);
            Controls.Add(labelTimeToFindExit);
            Controls.Add(labelTurnsToFindExit);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(buttonStopSimulation);
            Controls.Add(label4);
            Controls.Add(numericMazeSeed);
            Controls.Add(label3);
            Controls.Add(numericNoAgents);
            Controls.Add(label2);
            Controls.Add(numericMazeHeight);
            Controls.Add(label1);
            Controls.Add(numericMazeWidth);
            Controls.Add(buttonGenerateMaze);
            Controls.Add(buttonStartSimulation);
            Controls.Add(pictureBox);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMazeWidth).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMazeHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericNoAgents).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericMazeSeed).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericTurnTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericNoSimulations).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox;
        private Button buttonStartSimulation;
        private Button buttonGenerateMaze;
        private NumericUpDown numericMazeWidth;
        private Label label1;
        private Label label2;
        private NumericUpDown numericMazeHeight;
        private Label label3;
        private NumericUpDown numericNoAgents;
        private Label label4;
        private NumericUpDown numericMazeSeed;
        private Button buttonStopSimulation;
        private Label label5;
        private Label label6;
        private Label labelTurnsToFindExit;
        private Label labelTimeToFindExit;
        private Label label8;
        private NumericUpDown numericTurnTime;
        private CheckBox checkBoxAnimations;
        private CheckBox checkBoxMazeWeights;
        private NumericUpDown numericNoSimulations;
        private Button buttonStartMultipleSimulations;
    }
}
