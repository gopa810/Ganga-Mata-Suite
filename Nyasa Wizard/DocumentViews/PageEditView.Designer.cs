namespace Nyasa_Wizard
{
    partial class PageEditView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setPictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertMantraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.insertNewPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.pagePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setPictureToolStripMenuItem,
            this.insertMantraToolStripMenuItem,
            this.insertTextToolStripMenuItem,
            this.insertLineToolStripMenuItem,
            this.toolStripMenuItem1,
            this.insertNewPageToolStripMenuItem,
            this.toolStripMenuItem2,
            this.pagePropertiesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 170);
            // 
            // setPictureToolStripMenuItem
            // 
            this.setPictureToolStripMenuItem.Name = "setPictureToolStripMenuItem";
            this.setPictureToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.setPictureToolStripMenuItem.Text = "Insert Picture";
            this.setPictureToolStripMenuItem.Click += new System.EventHandler(this.setPictureToolStripMenuItem_Click);
            // 
            // insertMantraToolStripMenuItem
            // 
            this.insertMantraToolStripMenuItem.Name = "insertMantraToolStripMenuItem";
            this.insertMantraToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.insertMantraToolStripMenuItem.Text = "Insert Mantra";
            this.insertMantraToolStripMenuItem.Click += new System.EventHandler(this.insertMantraToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // insertNewPageToolStripMenuItem
            // 
            this.insertNewPageToolStripMenuItem.Name = "insertNewPageToolStripMenuItem";
            this.insertNewPageToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.insertNewPageToolStripMenuItem.Text = "Insert New Page";
            this.insertNewPageToolStripMenuItem.Click += new System.EventHandler(this.insertNewPageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(156, 6);
            // 
            // pagePropertiesToolStripMenuItem
            // 
            this.pagePropertiesToolStripMenuItem.Name = "pagePropertiesToolStripMenuItem";
            this.pagePropertiesToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.pagePropertiesToolStripMenuItem.Text = "Page Properties";
            this.pagePropertiesToolStripMenuItem.Click += new System.EventHandler(this.pagePropertiesToolStripMenuItem_Click);
            // 
            // insertTextToolStripMenuItem
            // 
            this.insertTextToolStripMenuItem.Name = "insertTextToolStripMenuItem";
            this.insertTextToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.insertTextToolStripMenuItem.Text = "Insert Text";
            this.insertTextToolStripMenuItem.Click += new System.EventHandler(this.insertTextToolStripMenuItem_Click);
            // 
            // insertLineToolStripMenuItem
            // 
            this.insertLineToolStripMenuItem.Name = "insertLineToolStripMenuItem";
            this.insertLineToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.insertLineToolStripMenuItem.Text = "Insert Line";
            this.insertLineToolStripMenuItem.Click += new System.EventHandler(this.insertLineToolStripMenuItem_Click);
            // 
            // PageEditView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "PageEditView";
            this.Size = new System.Drawing.Size(475, 378);
            this.SizeChanged += new System.EventHandler(this.PageEditView_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PageEditView_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PageEditView_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PageEditView_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PageEditView_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PageEditView_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PageEditView_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setPictureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertMantraToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem insertNewPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem pagePropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertLineToolStripMenuItem;


    }
}
