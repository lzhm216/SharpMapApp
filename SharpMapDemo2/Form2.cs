using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpMap.Data;
using SharpMap.Layers;
using SharpMap.Forms;
using GeoAPI.Geometries;

namespace SharpMapDemo2
{
    public partial class Form2 : Form
    {
        SharpMap.Forms.MapBox mapBox1;

        private System.Windows.Forms.Button button2;

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton ZoomWindowToolStripButton;
        private System.Windows.Forms.ToolStripButton ZoomInToolStripButton;
        private System.Windows.Forms.ToolStripButton ZoomOutToolStripButton;
        private System.Windows.Forms.ToolStripButton PanToolStripButton;
        private System.Windows.Forms.ToolStripButton PreviousToolStripButton;
        private System.Windows.Forms.ToolStripButton NextToolStripButton;
        private System.Windows.Forms.ToolStripButton QueryToolStripButton;

        private int _index;
        private readonly System.Collections.Generic.List<Envelope> _zoomExtentStack = new System.Collections.Generic.List<Envelope>();
        private bool _storeExtentsInternal;
        private bool _storeExtentsUser;
        private bool _blockStoringWhenPanning;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));


            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ZoomWindowToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomInToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomOutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.PanToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.PreviousToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.NextToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.QueryToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();

            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZoomWindowToolStripButton,
            this.ZoomInToolStripButton,
            this.ZoomOutToolStripButton,
            this.PanToolStripButton,
            this.PreviousToolStripButton,
            this.NextToolStripButton,
            this.QueryToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(735, 31);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.ZoomWindowToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomWindowToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("globe")));
            this.ZoomWindowToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ZoomWindowToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomWindowToolStripButton.Name = "toolStripButton1";
            this.ZoomWindowToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.ZoomWindowToolStripButton.Text = "全图";
            this.ZoomWindowToolStripButton.Click += new EventHandler(ZoomWindowToolStripButton_Click);
            // 
            // toolStripButton2
            // 
            this.ZoomInToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomInToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomin")));
            this.ZoomInToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ZoomInToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomInToolStripButton.Name = "toolStripButton2";
            this.ZoomInToolStripButton.Size = new System.Drawing.Size(26, 28);
            this.ZoomInToolStripButton.Text = "放大";
            this.ZoomInToolStripButton.Click += new EventHandler(ZoomInToolStripButton_Click);

            // 
            // toolStripButton3
            // 
            this.ZoomOutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomOutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("zoomout")));
            this.ZoomOutToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ZoomOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomOutToolStripButton.Name = "toolStripButton3";
            this.ZoomOutToolStripButton.Size = new System.Drawing.Size(26, 28);
            this.ZoomOutToolStripButton.Text = "缩小";
            this.ZoomOutToolStripButton.Click += new EventHandler(ZoomOutToolStripButton_Click);
            // 
            // toolStripButton4
            // 
            this.PanToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PanToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("hand")));
            this.PanToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PanToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PanToolStripButton.Name = "toolStripButton4";
            this.PanToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.PanToolStripButton.Text = "平移";
            this.PanToolStripButton.Click += new EventHandler(PanToolStripButton_Click);
            // 
            // toolStripButton6
            // 
            this.PreviousToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PreviousToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("previous")));
            this.PreviousToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.PreviousToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PreviousToolStripButton.Name = "toolStripButton6";
            this.PreviousToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.PreviousToolStripButton.Text = "前一视图";
            this.PreviousToolStripButton.Click += new EventHandler(PreviousToolStripButton_Click);
            // 
            // toolStripButton7
            // 
            this.NextToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.NextToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("next")));
            this.NextToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.NextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.NextToolStripButton.Name = "toolStripButton7";
            this.NextToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.NextToolStripButton.Text = "后一视图";
            this.NextToolStripButton.Click += new EventHandler(NextToolStripButton_Click);
            // 
            // toolStripButton8
            // 
            this.QueryToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.QueryToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("identify")));
            this.QueryToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.QueryToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.QueryToolStripButton.Name = "toolStripButton8";
            this.QueryToolStripButton.Size = new System.Drawing.Size(28, 28);
            this.QueryToolStripButton.Text = "查询";
            this.QueryToolStripButton.Click += new EventHandler(QueryToolStripButton_Click);
            this.Controls.Add(this.toolStrip1);

            // 
            // button2
            // 
            this.button2 = new System.Windows.Forms.Button();
            this.button2.Location = new System.Drawing.Point(158, 99);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(button2_Click);
            this.Controls.Add(this.button2);


            // 
            // mapBox1
            // 
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox1.BackColor = System.Drawing.Color.White;
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(0, 78);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.QueryGrowFactor = 5F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = false;
            this.mapBox1.Size = new System.Drawing.Size(527, 317);
            this.mapBox1.TabIndex = 0;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = -2D;
            this.mapBox1.MapQueried += new SharpMap.Forms.MapBox.MapQueryHandler(mapBox1_MapQueried);
            this.mapBox1.Map.MapViewOnChange += new SharpMap.Map.MapViewChangedHandler(Map_MapViewOnChange);
            this.mapBox1.MouseDown += new MapBox.MouseEventHandler(mapBox1_MouseDown);
            this.mapBox1.MouseUp += new MapBox.MouseEventHandler(mapBox1_MouseUp);
            this.Controls.Add(this.mapBox1);

            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();

            this.mapBox1.Map.Layers.Clear();

            SharpMap.Layers.VectorLayer vlayer = new SharpMap.Layers.VectorLayer("states");
            string start = Application.StartupPath;
            string path =  @"E:\workspace\Code\2016\ExcelReader\ExcelReader\bin\Debug\tmp.shp";
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

        void mapBox1_MouseUp(Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (this.mapBox1.ActiveTool == MapBox.Tools.Pan)
            {
                this._blockStoringWhenPanning = true;
            }
        }

        void mapBox1_MouseDown(Coordinate worldPos, MouseEventArgs imagePos)
        {
            if (this.mapBox1.ActiveTool == MapBox.Tools.Pan)
            {
                this._blockStoringWhenPanning = false;
            }
        }

        void Map_MapViewOnChange()
        {
            if (this._storeExtentsUser && this._storeExtentsInternal && !this._blockStoringWhenPanning)
            {
                this.Add(this.mapBox1.Map.Envelope);
                return;
            }
            this._storeExtentsInternal = true;
        }

        public bool StoreExtents
        {
            get
            {
                return this._storeExtentsUser;
            }
            set
            {
                if (value)
                {
                    this.Add(this.mapBox1.Map.Envelope);
                }
                this._storeExtentsUser = value;
            }
        }

        public bool CanZoomPrevious
        {
            get
            {
                return this._index > 0;
            }
        }

        public bool CanZoomNext
        {
            get
            {
                return this._index < this._zoomExtentStack.Count - 1;
            }
        }

        private void Add(Envelope newExtent)
        {
            if (this._zoomExtentStack.Count - 1 > this._index)
            {
                this._zoomExtentStack.RemoveRange(this._index + 1, this._zoomExtentStack.Count - this._index - 1);
            }
            this._zoomExtentStack.Add(newExtent);
            this._index = this._zoomExtentStack.Count - 1;
        }

        void QueryToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapBox.Tools.QueryPoint); });
        }

        void NextToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.CanZoomNext)
            {
                this._storeExtentsInternal = false;
                this._index++;
                this.mapBox1.Map.ZoomToBox(this._zoomExtentStack[this._index]);
            }
        }

        void PreviousToolStripButton_Click(object sender, EventArgs e)
        {
            if (this.CanZoomPrevious)
            {
                this._storeExtentsInternal = false;
                this._index--;
                this.mapBox1.Map.ZoomToBox(this._zoomExtentStack[this._index]);
            }
        }

        void PanToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapBox.Tools.Pan); });
        }

        void ZoomOutToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapBox.Tools.ZoomOut); });
        }

        void ZoomInToolStripButton_Click(object sender, EventArgs e)
        {
            BeginInvoke((MethodInvoker)delegate { changeMode(MapBox.Tools.ZoomIn); });
        }

        void ZoomWindowToolStripButton_Click(object sender, EventArgs e)
        {
            this.mapBox1.Map.ZoomToExtents();
        }

        private void changeMode(MapBox.Tools tool)
        {
            this.mapBox1.ActiveTool = tool;

            ZoomWindowToolStripButton.Checked = (tool == MapBox.Tools.ZoomWindow);
            ZoomInToolStripButton.Checked = (tool == MapBox.Tools.ZoomIn);
            ZoomOutToolStripButton.Checked = (tool == MapBox.Tools.ZoomOut);
            PanToolStripButton.Checked = (tool == MapBox.Tools.Pan);
            //PreviousToolStripButton.Checked = (tool == MapBox.Tools.);
            QueryToolStripButton.Checked = (tool == MapBox.Tools.QueryBox);
        }

        private void mapBox1_MapQueried(SharpMap.Data.FeatureDataTable data)
        {
            int count = data.Rows.Count;
            foreach (DataRow dr in data.Rows)
            {
                ShowSelectLayer(data);
                string s = "";
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    s = s + dr[i].ToString() + ",";
                }
                Console.WriteLine(s);
            }
        }


        void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.mapBox1.Map.MapScale.ToString());
        }

        //void mapBox1_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (mouseState == "query")
        //    {
        //        GeoAPI.Geometries.Coordinate coord = this.mapBox1.Map.ImageToWorld(new PointF(e.X, e.Y));
        //        FeatureDataSet ds;
        //        ds = QueryMapByClick(coord);
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            string s = "";
        //            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
        //            {
        //                s = s + dr[i].ToString() + ",";
        //            }
        //            MessageBox.Show(s);
        //            //Console.WriteLine(s);
        //        }
        //    }

        //}

        private FeatureDataSet QueryMapByClick(GeoAPI.Geometries.Coordinate coord)
        {
            FeatureDataSet ds = new FeatureDataSet();
            SharpMap.Layers.VectorLayer vlayer = this.mapBox1.Map.Layers[0] as VectorLayer;
            if (!vlayer.DataSource.IsOpen) vlayer.DataSource.Open();

            GeoAPI.Geometries.Envelope env = new GeoAPI.Geometries.Envelope();
            env.Init( coord.X - 0.01, coord.X + 0.01,coord.Y - 0.01,coord.Y + 0.01);

            vlayer.DataSource.ExecuteIntersectionQuery(env, ds);
            if (ds.Tables.Count > 0)
            {
                ShowSelectLayer(ds.Tables[0]);
            }
            return ds;
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
