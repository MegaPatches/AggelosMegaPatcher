namespace Aggelos_Save_Mod
{
    partial class SceneGuideDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneGuideDialog));
            this.picSceneGuideMap = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSceneGuideMap)).BeginInit();
            this.SuspendLayout();
            // 
            // picSceneGuideMap
            // 
            this.picSceneGuideMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picSceneGuideMap.Image = global::Aggelos_Save_Mod.Properties.Resources.Scenes_Guide;
            this.picSceneGuideMap.Location = new System.Drawing.Point(0, 0);
            this.picSceneGuideMap.Name = "picSceneGuideMap";
            this.picSceneGuideMap.Size = new System.Drawing.Size(1087, 645);
            this.picSceneGuideMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSceneGuideMap.TabIndex = 0;
            this.picSceneGuideMap.TabStop = false;
            // 
            // SceneGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 645);
            this.Controls.Add(this.picSceneGuideMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SceneGuide";
            this.Text = "Scene Guide";
            ((System.ComponentModel.ISupportInitialize)(this.picSceneGuideMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSceneGuideMap;
    }
}