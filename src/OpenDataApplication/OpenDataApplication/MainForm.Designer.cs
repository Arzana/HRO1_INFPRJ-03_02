namespace OpenDataApplication
{
    public sealed partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.map = new GMap.NET.WindowsForms.GMapControl();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.BtnCalcRoute = new System.Windows.Forms.Button();
            this.LblVisibility = new System.Windows.Forms.Label();
            this.LblInfo = new System.Windows.Forms.Label();
            this.LblEnd = new System.Windows.Forms.Label();
            this.CBoxEnd = new System.Windows.Forms.ComboBox();
            this.LblStart = new System.Windows.Forms.Label();
            this.CBoxStart = new System.Windows.Forms.ComboBox();
            this.Bus = new System.Windows.Forms.CheckBox();
            this.Tram = new System.Windows.Forms.CheckBox();
            this.Metro = new System.Windows.Forms.CheckBox();
            this.Trein = new System.Windows.Forms.CheckBox();
            this.cSVReaderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mainFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cSVReaderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // map
            // 
            this.map.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.map.Bearing = 0F;
            this.map.CanDragMap = true;
            this.map.EmptyTileColor = System.Drawing.Color.Navy;
            this.map.GrayScaleMode = false;
            this.map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.map.LevelsKeepInMemmory = 5;
            this.map.Location = new System.Drawing.Point(3, 3);
            this.map.MarkersEnabled = true;
            this.map.MaxZoom = 18;
            this.map.MinZoom = 8;
            this.map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            this.map.Name = "map";
            this.map.NegativeMode = false;
            this.map.PolygonsEnabled = true;
            this.map.RetryLoadTile = 0;
            this.map.RoutesEnabled = true;
            this.map.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.map.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.map.ShowTileGridLines = false;
            this.map.Size = new System.Drawing.Size(841, 587);
            this.map.TabIndex = 0;
            this.map.Zoom = 0D;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.map);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.BtnCalcRoute);
            this.splitContainer.Panel2.Controls.Add(this.LblVisibility);
            this.splitContainer.Panel2.Controls.Add(this.LblInfo);
            this.splitContainer.Panel2.Controls.Add(this.LblEnd);
            this.splitContainer.Panel2.Controls.Add(this.CBoxEnd);
            this.splitContainer.Panel2.Controls.Add(this.LblStart);
            this.splitContainer.Panel2.Controls.Add(this.CBoxStart);
            this.splitContainer.Panel2.Controls.Add(this.Bus);
            this.splitContainer.Panel2.Controls.Add(this.Tram);
            this.splitContainer.Panel2.Controls.Add(this.Metro);
            this.splitContainer.Panel2.Controls.Add(this.Trein);
            this.splitContainer.Panel2MinSize = 450;
            this.splitContainer.Size = new System.Drawing.Size(1341, 593);
            this.splitContainer.SplitterDistance = 847;
            this.splitContainer.TabIndex = 1;
            // 
            // BtnCalcRoute
            // 
            this.BtnCalcRoute.Location = new System.Drawing.Point(137, 153);
            this.BtnCalcRoute.Name = "BtnCalcRoute";
            this.BtnCalcRoute.Size = new System.Drawing.Size(108, 23);
            this.BtnCalcRoute.TabIndex = 11;
            this.BtnCalcRoute.Text = "Calculate Route";
            this.BtnCalcRoute.UseVisualStyleBackColor = true;
            this.BtnCalcRoute.Click += new System.EventHandler(this.BtnCalcRoute_Click);
            // 
            // LblVisibility
            // 
            this.LblVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblVisibility.AutoSize = true;
            this.LblVisibility.Location = new System.Drawing.Point(347, 33);
            this.LblVisibility.Name = "LblVisibility";
            this.LblVisibility.Size = new System.Drawing.Size(72, 13);
            this.LblVisibility.TabIndex = 10;
            this.LblVisibility.Text = "Zichtbaarheid";
            // 
            // LblInfo
            // 
            this.LblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblInfo.AutoSize = true;
            this.LblInfo.Location = new System.Drawing.Point(66, 192);
            this.LblInfo.MaximumSize = new System.Drawing.Size(500, 500);
            this.LblInfo.Name = "LblInfo";
            this.LblInfo.Size = new System.Drawing.Size(90, 13);
            this.LblInfo.TabIndex = 9;
            this.LblInfo.Text = "Stationsinformatie";
            // 
            // LblEnd
            // 
            this.LblEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblEnd.AutoSize = true;
            this.LblEnd.Location = new System.Drawing.Point(82, 96);
            this.LblEnd.Name = "LblEnd";
            this.LblEnd.Size = new System.Drawing.Size(49, 13);
            this.LblEnd.TabIndex = 8;
            this.LblEnd.Text = "Eindpunt";
            // 
            // CBoxEnd
            // 
            this.CBoxEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBoxEnd.FormattingEnabled = true;
            this.CBoxEnd.Location = new System.Drawing.Point(69, 116);
            this.CBoxEnd.Name = "CBoxEnd";
            this.CBoxEnd.Size = new System.Drawing.Size(244, 21);
            this.CBoxEnd.TabIndex = 7;
            this.CBoxEnd.SelectedIndexChanged += new System.EventHandler(this.CBoxEnd_SelectedIndexChanged);
            // 
            // LblStart
            // 
            this.LblStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStart.AutoSize = true;
            this.LblStart.Location = new System.Drawing.Point(82, 30);
            this.LblStart.Name = "LblStart";
            this.LblStart.Size = new System.Drawing.Size(55, 13);
            this.LblStart.TabIndex = 6;
            this.LblStart.Text = "Beginpunt";
            // 
            // CBoxStart
            // 
            this.CBoxStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBoxStart.FormattingEnabled = true;
            this.CBoxStart.Location = new System.Drawing.Point(69, 49);
            this.CBoxStart.Name = "CBoxStart";
            this.CBoxStart.Size = new System.Drawing.Size(244, 21);
            this.CBoxStart.TabIndex = 5;
            this.CBoxStart.SelectedIndexChanged += new System.EventHandler(this.CBoxStart_SelectedIndexChanged);
            // 
            // Bus
            // 
            this.Bus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bus.AutoSize = true;
            this.Bus.Checked = true;
            this.Bus.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Bus.Location = new System.Drawing.Point(350, 122);
            this.Bus.Name = "Bus";
            this.Bus.Size = new System.Drawing.Size(44, 17);
            this.Bus.TabIndex = 3;
            this.Bus.Text = "Bus";
            this.Bus.UseVisualStyleBackColor = true;
            this.Bus.CheckedChanged += new System.EventHandler(this.CheckBus_CheckedChanged);
            // 
            // Tram
            // 
            this.Tram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Tram.AutoSize = true;
            this.Tram.Checked = true;
            this.Tram.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Tram.Location = new System.Drawing.Point(350, 99);
            this.Tram.Name = "Tram";
            this.Tram.Size = new System.Drawing.Size(50, 17);
            this.Tram.TabIndex = 2;
            this.Tram.Text = "Tram";
            this.Tram.UseVisualStyleBackColor = true;
            this.Tram.CheckedChanged += new System.EventHandler(this.CheckTram_CheckedChanged);
            // 
            // Metro
            // 
            this.Metro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Metro.AutoSize = true;
            this.Metro.Checked = true;
            this.Metro.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Metro.Location = new System.Drawing.Point(350, 76);
            this.Metro.Name = "Metro";
            this.Metro.Size = new System.Drawing.Size(53, 17);
            this.Metro.TabIndex = 1;
            this.Metro.Text = "Metro";
            this.Metro.UseVisualStyleBackColor = true;
            this.Metro.CheckedChanged += new System.EventHandler(this.CheckMetro_CheckedChanged);
            // 
            // Trein
            // 
            this.Trein.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Trein.AutoSize = true;
            this.Trein.Checked = true;
            this.Trein.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Trein.Location = new System.Drawing.Point(350, 53);
            this.Trein.Name = "Trein";
            this.Trein.Size = new System.Drawing.Size(50, 17);
            this.Trein.TabIndex = 0;
            this.Trein.Text = "Trein";
            this.Trein.UseVisualStyleBackColor = true;
            this.Trein.CheckedChanged += new System.EventHandler(this.CheckTrein_CheckedChanged);
            // 
            // cSVReaderBindingSource
            // 
            this.cSVReaderBindingSource.DataSource = typeof(OpenDataApplication.Core.CSVReader);
            // 
            // mainFormBindingSource
            // 
            this.mainFormBindingSource.DataSource = typeof(OpenDataApplication.MainForm);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1362, 617);
            this.Controls.Add(this.splitContainer);
            this.Name = "MainForm";
            this.Text = "OpenData";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cSVReaderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl map;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label LblInfo;
        private System.Windows.Forms.Label LblEnd;
        private System.Windows.Forms.ComboBox CBoxEnd;
        private System.Windows.Forms.Label LblStart;
        private System.Windows.Forms.ComboBox CBoxStart;
        private System.Windows.Forms.CheckBox Bus;
        private System.Windows.Forms.CheckBox Tram;
        private System.Windows.Forms.CheckBox Metro;
        private System.Windows.Forms.CheckBox Trein;
        private System.Windows.Forms.Label LblVisibility;
        private System.Windows.Forms.BindingSource cSVReaderBindingSource;
        private System.Windows.Forms.BindingSource mainFormBindingSource;
        private System.Windows.Forms.Button BtnCalcRoute;
    }
}

