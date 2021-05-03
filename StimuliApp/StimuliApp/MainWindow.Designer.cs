
using System.Drawing;

namespace StimuliApp
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.LaunchRayCasting_Button = new System.Windows.Forms.Button();
            this.ChooseStimulus_Button = new System.Windows.Forms.Button();
            this.ChooseEyeTrackingData_Button = new System.Windows.Forms.Button();
            this.folderBrowserDialog_chooseProband = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog_chooseStimulus = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_checkmark = new System.Windows.Forms.PictureBox();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.pictureBox_showStimuli = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_checkmark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_showStimuli)).BeginInit();
            this.SuspendLayout();
            // 
            // LaunchRayCasting_Button
            // 
            this.LaunchRayCasting_Button.FlatAppearance.BorderSize = 0;
            this.LaunchRayCasting_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LaunchRayCasting_Button.ForeColor = System.Drawing.Color.White;
            this.LaunchRayCasting_Button.Image = ((System.Drawing.Image)(resources.GetObject("LaunchRayCasting_Button.Image")));
            this.LaunchRayCasting_Button.Location = new System.Drawing.Point(0, 78);
            this.LaunchRayCasting_Button.Name = "LaunchRayCasting_Button";
            this.LaunchRayCasting_Button.Size = new System.Drawing.Size(202, 65);
            this.LaunchRayCasting_Button.TabIndex = 0;
            this.LaunchRayCasting_Button.Text = "Ray Casting starten";
            this.LaunchRayCasting_Button.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.LaunchRayCasting_Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.LaunchRayCasting_Button.UseVisualStyleBackColor = true;
            this.LaunchRayCasting_Button.Click += new System.EventHandler(this.LaunchRayCasting_Button_Click);
            // 
            // ChooseStimulus_Button
            // 
            this.ChooseStimulus_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChooseStimulus_Button.Location = new System.Drawing.Point(416, 29);
            this.ChooseStimulus_Button.Name = "ChooseStimulus_Button";
            this.ChooseStimulus_Button.Size = new System.Drawing.Size(154, 31);
            this.ChooseStimulus_Button.TabIndex = 1;
            this.ChooseStimulus_Button.Text = "Stimulus wählen";
            this.ChooseStimulus_Button.UseVisualStyleBackColor = true;
            this.ChooseStimulus_Button.Click += new System.EventHandler(this.ChooseStimulus_Button_Click);
            // 
            // ChooseEyeTrackingData_Button
            // 
            this.ChooseEyeTrackingData_Button.FlatAppearance.BorderSize = 0;
            this.ChooseEyeTrackingData_Button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChooseEyeTrackingData_Button.ForeColor = System.Drawing.Color.White;
            this.ChooseEyeTrackingData_Button.Image = ((System.Drawing.Image)(resources.GetObject("ChooseEyeTrackingData_Button.Image")));
            this.ChooseEyeTrackingData_Button.Location = new System.Drawing.Point(0, 331);
            this.ChooseEyeTrackingData_Button.Name = "ChooseEyeTrackingData_Button";
            this.ChooseEyeTrackingData_Button.Size = new System.Drawing.Size(202, 65);
            this.ChooseEyeTrackingData_Button.TabIndex = 2;
            this.ChooseEyeTrackingData_Button.Text = "Probanden auswählen";
            this.ChooseEyeTrackingData_Button.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.ChooseEyeTrackingData_Button.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ChooseEyeTrackingData_Button.UseVisualStyleBackColor = true;
            this.ChooseEyeTrackingData_Button.Click += new System.EventHandler(this.ChooseEyeTrackingData_Button_Click);
            // 
            // openFileDialog_chooseStimulus
            // 
            this.openFileDialog_chooseStimulus.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox_checkmark);
            this.panel1.Controls.Add(this.panelLeft);
            this.panel1.Controls.Add(this.ChooseEyeTrackingData_Button);
            this.panel1.Controls.Add(this.LaunchRayCasting_Button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(202, 450);
            this.panel1.TabIndex = 3;
            // 
            // pictureBox_checkmark
            // 
            this.pictureBox_checkmark.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_checkmark.ErrorImage = null;
            this.pictureBox_checkmark.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_checkmark.Image")));
            this.pictureBox_checkmark.Location = new System.Drawing.Point(118, 331);
            this.pictureBox_checkmark.Name = "pictureBox_checkmark";
            this.pictureBox_checkmark.Size = new System.Drawing.Size(16, 16);
            this.pictureBox_checkmark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_checkmark.TabIndex = 4;
            this.pictureBox_checkmark.TabStop = false;
            this.pictureBox_checkmark.Visible = false;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(70)))));
            this.panelLeft.Location = new System.Drawing.Point(192, 82);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(10, 61);
            this.panelLeft.TabIndex = 4;
            // 
            // pictureBox_showStimuli
            // 
            this.pictureBox_showStimuli.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_showStimuli.Image")));
            this.pictureBox_showStimuli.InitialImage = null;
            this.pictureBox_showStimuli.Location = new System.Drawing.Point(292, 78);
            this.pictureBox_showStimuli.Name = "pictureBox_showStimuli";
            this.pictureBox_showStimuli.Size = new System.Drawing.Size(96, 96);
            this.pictureBox_showStimuli.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_showStimuli.TabIndex = 3;
            this.pictureBox_showStimuli.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(44)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox_showStimuli);
            this.Controls.Add(this.ChooseStimulus_Button);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(138)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "GazeRays";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_checkmark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_showStimuli)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button LaunchRayCasting_Button;
        private System.Windows.Forms.Button ChooseStimulus_Button;
        private System.Windows.Forms.Button ChooseEyeTrackingData_Button;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog_chooseProband;
        private System.Windows.Forms.OpenFileDialog openFileDialog_chooseStimulus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox_showStimuli;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.PictureBox pictureBox_checkmark;
    }
}

