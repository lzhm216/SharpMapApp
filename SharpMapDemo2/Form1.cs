using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpMap.Forms;

namespace SharpMapDemo2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            SharpMap.Layers.VectorLayer vlayer = new SharpMap.Layers.VectorLayer("states");
            string start = Application.StartupPath;
            string path = start + "\\data\\states_ugl\\states_ugl.shp";
            vlayer.DataSource = new SharpMap.Data.Providers.ShapeFile(path);
            mapBox1.Map.Layers.Add(vlayer);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
        }

        private void mapBox1_MapQueried(SharpMap.Data.FeatureDataTable data)
        {
            int count = data.Rows.Count;
            foreach (DataRow dr in data.Rows)
            {
                string s = "";
               for(int i =0;i<data.Columns.Count;i++){
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

            //ZoomInModeToolStripButton.Checked = (tool == MapBox.Tools.ZoomIn);
            //ZoomOutModeToolStripButton.Checked = (tool == MapBox.Tools.ZoomOut);
            //PanToolStripButton.Checked = (tool == MapBox.Tools.Pan);
            toolsbIdentity.Checked = (tool == MapBox.Tools.QueryBox);
        }
    }
}
