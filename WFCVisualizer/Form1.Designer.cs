namespace WFCVisualizer
{
    partial class Form1
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
            btnStep = new Button();
            controlsGroup = new GroupBox();
            btnReset = new Button();
            btnComplete = new Button();
            btnDisplayKernels = new Button();
            btnPlay = new Button();
            status = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            progressBar = new ToolStripProgressBar();
            btnNext = new Button();
            controlsGroup.SuspendLayout();
            status.SuspendLayout();
            SuspendLayout();
            // 
            // btnStep
            // 
            btnStep.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnStep.Location = new Point(573, 22);
            btnStep.Name = "btnStep";
            btnStep.Size = new Size(82, 60);
            btnStep.TabIndex = 1;
            btnStep.Text = "Step";
            btnStep.UseVisualStyleBackColor = true;
            btnStep.Click += btnStep_Click;
            // 
            // controlsGroup
            // 
            controlsGroup.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            controlsGroup.Controls.Add(btnNext);
            controlsGroup.Controls.Add(btnReset);
            controlsGroup.Controls.Add(btnComplete);
            controlsGroup.Controls.Add(btnDisplayKernels);
            controlsGroup.Controls.Add(btnPlay);
            controlsGroup.Controls.Add(btnStep);
            controlsGroup.Location = new Point(12, 478);
            controlsGroup.Name = "controlsGroup";
            controlsGroup.Size = new Size(749, 88);
            controlsGroup.TabIndex = 2;
            controlsGroup.TabStop = false;
            controlsGroup.Text = "Controls";
            // 
            // btnReset
            // 
            btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnReset.Location = new Point(249, 22);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 60);
            btnReset.TabIndex = 5;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnComplete
            // 
            btnComplete.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnComplete.Location = new Point(330, 22);
            btnComplete.Name = "btnComplete";
            btnComplete.Size = new Size(75, 60);
            btnComplete.TabIndex = 4;
            btnComplete.Text = "Complete";
            btnComplete.UseVisualStyleBackColor = true;
            btnComplete.Click += btnComplete_Click;
            // 
            // btnDisplayKernels
            // 
            btnDisplayKernels.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnDisplayKernels.Location = new Point(411, 22);
            btnDisplayKernels.Name = "btnDisplayKernels";
            btnDisplayKernels.Size = new Size(75, 60);
            btnDisplayKernels.TabIndex = 3;
            btnDisplayKernels.Text = "Display Kernels";
            btnDisplayKernels.UseVisualStyleBackColor = true;
            btnDisplayKernels.Click += btnDisplayKernels_Click;
            // 
            // btnPlay
            // 
            btnPlay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnPlay.Location = new Point(661, 22);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(82, 60);
            btnPlay.TabIndex = 2;
            btnPlay.Text = "Play";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // status
            // 
            status.AutoSize = false;
            status.Items.AddRange(new ToolStripItem[] { lblStatus, progressBar });
            status.Location = new Point(0, 569);
            status.Name = "status";
            status.Size = new Size(773, 22);
            status.TabIndex = 3;
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 17);
            // 
            // progressBar
            // 
            progressBar.Alignment = ToolStripItemAlignment.Right;
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(100, 16);
            progressBar.Style = ProgressBarStyle.Continuous;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(492, 22);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 60);
            btnNext.TabIndex = 4;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(773, 591);
            Controls.Add(status);
            Controls.Add(controlsGroup);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            controlsGroup.ResumeLayout(false);
            status.ResumeLayout(false);
            status.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnStep;
        private GroupBox controlsGroup;
        private Button btnPlay;
        private Button btnDisplayKernels;
        private StatusStrip status;
        private ToolStripStatusLabel lblStatus;
        private Button btnComplete;
        private ToolStripProgressBar progressBar;
        private Button btnReset;
        private Button btnNext;
    }
}
