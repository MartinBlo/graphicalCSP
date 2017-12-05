namespace GraphicalConstraintProgramming
{
    partial class ChangeVarForm
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
            this.varname = new System.Windows.Forms.Label();
            this.vartype = new System.Windows.Forms.Label();
            this.fixedVal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // varname
            // 
            this.varname.AutoSize = true;
            this.varname.Location = new System.Drawing.Point(129, 9);
            this.varname.Name = "varname";
            this.varname.Size = new System.Drawing.Size(35, 13);
            this.varname.TabIndex = 0;
            this.varname.Text = "label1";
            // 
            // vartype
            // 
            this.vartype.AutoSize = true;
            this.vartype.Location = new System.Drawing.Point(129, 42);
            this.vartype.Name = "vartype";
            this.vartype.Size = new System.Drawing.Size(35, 13);
            this.vartype.TabIndex = 1;
            this.vartype.Text = "label1";
            // 
            // fixedVal
            // 
            this.fixedVal.Location = new System.Drawing.Point(132, 73);
            this.fixedVal.Name = "fixedVal";
            this.fixedVal.Size = new System.Drawing.Size(65, 20);
            this.fixedVal.TabIndex = 2;
            this.fixedVal.TextChanged += new System.EventHandler(this.fixedVal_TextChanged);
            this.fixedVal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fixedVal_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Variable name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Type";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Specify fixed value";
            // 
            // ChangeVarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 114);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fixedVal);
            this.Controls.Add(this.vartype);
            this.Controls.Add(this.varname);
            this.Name = "ChangeVarForm";
            this.Text = "Change Variable";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label varname;
        private System.Windows.Forms.Label vartype;
        private System.Windows.Forms.TextBox fixedVal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}