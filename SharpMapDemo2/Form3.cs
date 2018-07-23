using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpMap.Forms;
using SharpMap.Layers;
using SharpMap.Data;

namespace SharpMapDemo2
{
    public partial class Form3 : Form
    {
        private SharpMap.Forms.MapBox mapBox1;
        private SharpMap.Forms.ToolBar.MapZoomToolStrip mapZoomToolStrip1;
        private System.Windows.Forms.ToolStripButton toolsbIdentity;

        public Form3()
        {
            InitializeComponent();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.components = new System.ComponentModel.Container();

            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.mapZoomToolStrip1 = new SharpMap.Forms.ToolBar.MapZoomToolStrip(this.components);
            this.toolsbIdentity = new System.Windows.Forms.ToolStripButton();
            this.mapZoomToolStrip1.SuspendLayout();

            // 
            // mapBox1
            // 
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox1.BackColor = System.Drawing.Color.White;
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(0, 0);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = false;
            this.mapBox1.Size = new System.Drawing.Size(580, 407);
            this.mapBox1.TabIndex = 0;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = -2D;
            this.mapBox1.MapQueried += new SharpMap.Forms.MapBox.MapQueryHandler(this.mapBox1_MapQueried);
            // 
            // mapZoomToolStrip1
            // 
            this.mapZoomToolStrip1.Enabled = false;
            this.mapZoomToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsbIdentity});
            this.mapZoomToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.mapZoomToolStrip1.MapControl = this.mapBox1;
            this.mapZoomToolStrip1.Name = "mapZoomToolStrip1";
            this.mapZoomToolStrip1.Size = new System.Drawing.Size(580, 25);
            this.mapZoomToolStrip1.TabIndex = 2;
            this.mapZoomToolStrip1.Text = "mapZoomToolStrip1";
            // 
            // toolsbIdentity
            // 
            this.toolsbIdentity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolsbIdentity.Image = ((System.Drawing.Image)(resources.GetObject("identify")));
            this.toolsbIdentity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolsbIdentity.Name = "toolsbIdentity";
            this.toolsbIdentity.Size = new System.Drawing.Size(23, 22);
            this.toolsbIdentity.Text = "查询";
            this.toolsbIdentity.Click += new System.EventHandler(this.toolsbIdentity_Click);

            this.Controls.Add(this.mapZoomToolStrip1);
            this.Controls.Add(this.mapBox1);

            this.mapZoomToolStrip1.ResumeLayout(false);
            this.mapZoomToolStrip1.PerformLayout();

            this.mapBox1.Map.Layers.Clear();

            SharpMap.Layers.VectorLayer vlayer = new SharpMap.Layers.VectorLayer("states");
            string start = Application.StartupPath;
            string path = @"D:\workspace\Code\2016\ExcelReader\ExcelReader\bin\Debug\tmp.shp";
            vlayer.DataSource = new SharpMap.Data.Providers.ShapeFile(path);
            vlayer.Style.Fill = new SolidBrush(Color.GreenYellow);
            vlayer.Style.Outline = System.Drawing.Pens.Gray;
            vlayer.Style.EnableOutline = true;
            mapBox1.Map.Layers.Add(vlayer);


            SharpMap.Layers.LabelLayer labelLayer = new LabelLayer("label");
            labelLayer.DataSource = vlayer.DataSource;
            labelLayer.Enabled = true;
            labelLayer.LabelColumn = "mapNo";
            labelLayer.Style = new SharpMap.Styles.LabelStyle();
            labelLayer.Style.ForeColor = Color.Black;
            labelLayer.Style.Font = new System.Drawing.Font(FontFamily.GenericSerif, 13);
            labelLayer.Style.Offset = new PointF(3, 3);
            labelLayer.Style.Halo = new Pen(Color.Yellow, 2);
            labelLayer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            labelLayer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            labelLayer.LabelFilter = SharpMap.Rendering.LabelCollisionDetection.ThoroughCollisionDetection;
            labelLayer.Style.CollisionDetection = true;
            labelLayer.MaxVisible = 200000;
            mapBox1.Map.Layers.Add(labelLayer);


            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
        }

        private void mapBox1_MapQueried(SharpMap.Data.FeatureDataTable data)
        {
            int count = data.Rows.Count;
            if (data.Rows.Count > 0) ShowSelectLayer(data);
            foreach (DataRow dr in data.Rows)
            {
                string s = "";
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    s = s + dr[i].ToString() + ",";
                }
                Console.WriteLine(s);
            }
        }

        private void toolsbIdentity_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapBox.Tools.QueryBox); });
        }

        private void changeMode(MapBox.Tools tool)
        {
            this.mapBox1.ActiveTool = tool;
            toolsbIdentity.Checked = (tool == MapBox.Tools.QueryBox);
        }

        void ShowSelectLayer(SharpMap.Data.FeatureDataTable data)
        {
            Layer layer = null;

            foreach (Layer tmplayer in this.mapBox1.Map.Layers)
            {
                if (tmplayer.LayerName == "selectedLyr")
                {
                    layer = tmplayer;
                }
            }

            if (layer != null)
                this.mapBox1.Map.Layers.Remove(layer);

            SharpMap.Layers.VectorLayer selectedLyr = new SharpMap.Layers.VectorLayer("selectedLyr");
            foreach (FeatureDataRow row in data.Rows)
            {
                selectedLyr.DataSource = new SharpMap.Data.Providers.GeometryProvider(row as SharpMap.Data.FeatureDataRow);
                selectedLyr.Style.EnableOutline = true;
                selectedLyr.Style.Outline = new Pen(Color.FromArgb(50, 255, 255), 4);
                selectedLyr.Style.Fill = new SolidBrush(Color.Transparent);
                mapBox1.Map.Layers.Add(selectedLyr);
                this.mapBox1.Refresh();
            }
        }
    }
}
