namespace Aggelos_Save_Mod
{
    partial class AddSceneDialog
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
            this.tbSceneName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbScenes = new System.Windows.Forms.ComboBox();
            this.lblX = new System.Windows.Forms.Label();
            this.tbX = new System.Windows.Forms.NumericUpDown();
            this.lblY = new System.Windows.Forms.Label();
            this.tbY = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tbX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbY)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSceneName
            // 
            this.tbSceneName.Location = new System.Drawing.Point(12, 127);
            this.tbSceneName.Name = "tbSceneName";
            this.tbSceneName.Size = new System.Drawing.Size(307, 20);
            this.tbSceneName.TabIndex = 0;
            this.tbSceneName.TextChanged += new System.EventHandler(this.tbSceneName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Scene Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Area Selection:";
            // 
            // cbScenes
            // 
            this.cbScenes.DisplayMember = "Name";
            this.cbScenes.FormattingEnabled = true;
            this.cbScenes.Location = new System.Drawing.Point(12, 25);
            this.cbScenes.Name = "cbScenes";
            this.cbScenes.Size = new System.Drawing.Size(307, 21);
            this.cbScenes.TabIndex = 3;
            this.cbScenes.ValueMember = "ID";
            this.cbScenes.SelectedIndexChanged += new System.EventHandler(this.cbScenes_SelectedIndexChanged);
            // 
            // lblX
            // 
            this.lblX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(107, 55);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(17, 13);
            this.lblX.TabIndex = 114;
            this.lblX.Text = "X:";
            // 
            // tbX
            // 
            this.tbX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbX.Location = new System.Drawing.Point(82, 71);
            this.tbX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.tbX.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(75, 20);
            this.tbX.TabIndex = 115;
            this.tbX.ThousandsSeparator = true;
            this.tbX.ValueChanged += new System.EventHandler(this.tbX_ValueChanged);
            // 
            // lblY
            // 
            this.lblY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(191, 55);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 116;
            this.lblY.Text = "Y:";
            // 
            // tbY
            // 
            this.tbY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbY.Location = new System.Drawing.Point(166, 71);
            this.tbY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.tbY.Minimum = new decimal(new int[] {
            99999,
            0,
            0,
            -2147483648});
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(75, 20);
            this.tbY.TabIndex = 117;
            this.tbY.ThousandsSeparator = true;
            this.tbY.ValueChanged += new System.EventHandler(this.tbY_ValueChanged);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAdd.Location = new System.Drawing.Point(62, 167);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 118;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(194, 166);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 119;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AddSceneDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 202);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.tbY);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.tbX);
            this.Controls.Add(this.cbScenes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSceneName);
            this.Name = "AddSceneDialog";
            this.Text = "Add New Scene";
            ((System.ComponentModel.ISupportInitialize)(this.tbX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbSceneName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbScenes;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.NumericUpDown tbX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.NumericUpDown tbY;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnCancel;
    }
}