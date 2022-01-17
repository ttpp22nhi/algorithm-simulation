
namespace WindowsFormsApplication1
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.label1 = new System.Windows.Forms.Label();
            this.button_lienket = new System.Windows.Forms.Button();
            this.button_nhiphan = new System.Windows.Forms.Button();
            this.button_sapxep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(329, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 94);
            this.label1.TabIndex = 0;
            this.label1.Text = "MENU";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_lienket
            // 
            this.button_lienket.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_lienket.Location = new System.Drawing.Point(321, 257);
            this.button_lienket.Name = "button_lienket";
            this.button_lienket.Size = new System.Drawing.Size(200, 64);
            this.button_lienket.TabIndex = 1;
            this.button_lienket.Text = "Danh sách liên kết";
            this.button_lienket.UseVisualStyleBackColor = true;
            // 
            // button_nhiphan
            // 
            this.button_nhiphan.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_nhiphan.Location = new System.Drawing.Point(321, 367);
            this.button_nhiphan.Name = "button_nhiphan";
            this.button_nhiphan.Size = new System.Drawing.Size(200, 64);
            this.button_nhiphan.TabIndex = 1;
            this.button_nhiphan.Text = "Cây nhị phân";
            this.button_nhiphan.UseVisualStyleBackColor = true;
            this.button_nhiphan.Click += new System.EventHandler(this.button_nhiphan_Click);
            // 
            // button_sapxep
            // 
            this.button_sapxep.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_sapxep.Location = new System.Drawing.Point(321, 147);
            this.button_sapxep.Name = "button_sapxep";
            this.button_sapxep.Size = new System.Drawing.Size(200, 64);
            this.button_sapxep.TabIndex = 1;
            this.button_sapxep.Text = "Thuật toán sắp xếp";
            this.button_sapxep.UseVisualStyleBackColor = true;
            this.button_sapxep.Click += new System.EventHandler(this.button_sapxep_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 528);
            this.Controls.Add(this.button_nhiphan);
            this.Controls.Add(this.button_sapxep);
            this.Controls.Add(this.button_lienket);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.Text = "Home";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_lienket;
        private System.Windows.Forms.Button button_nhiphan;
        private System.Windows.Forms.Button button_sapxep;
    }
}