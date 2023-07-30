using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace Ambrosinus_Toolkit.Display
{
    // Token: 0x02000017 RID: 23
    public class GradientGenComponent : GH_Component
    {
        // Token: 0x060000D3 RID: 211 RVA: 0x00007062 File Offset: 0x00005262
        public GradientGenComponent() : base("GradientGen", "LA_GradGen", "Gradient Generator creates a custom gradient component \nby Luciano Ambrosini", "Ambrosinus", "1.Display")
        {
        }

        // Token: 0x060000D4 RID: 212 RVA: 0x00007084 File Offset: 0x00005284
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("GripPerc", "GripPerc", "list of grip positions from 0 to 1", (GH_ParamAccess)1);
            pManager.AddColourParameter("Lcol", "Lcol", "List of RGB colours (Left Grip Colour)", (GH_ParamAccess)1);
            pManager.AddColourParameter("Rcol", "Rcol", "List of RGB colours (Right Grip Colour)", (GH_ParamAccess)1);
            pManager.AddBooleanParameter("Interp", "Interp", "setup a smoother colours transition", 0);
            pManager.AddBooleanParameter("Lck", "Lck", "Lock prevents Gradient component from editing", 0);
            pManager.AddNumberParameter("t", "t", "sliding parameter to check single colour", 0);
            pManager.AddBooleanParameter("Bake", "Bake", "Bake your custom Gradient component on canvas", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
            pManager[4].MutableNickName = false;
            pManager[5].MutableNickName = false;
            pManager[6].MutableNickName = false;
        }

        // Token: 0x060000D5 RID: 213 RVA: 0x0000718D File Offset: 0x0000538D
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("C", "C", "RGB value at t param ", 0);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x060000D6 RID: 214 RVA: 0x000071B3 File Offset: 0x000053B3
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000D7 RID: 215 RVA: 0x000071DC File Offset: 0x000053DC
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000D8 RID: 216 RVA: 0x00007208 File Offset: 0x00005408
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000D9 RID: 217 RVA: 0x00007218 File Offset: 0x00005418
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<double> list = new List<double>();
            if (!DA.GetDataList<double>(0, list))
            {
                return;
            }
            List<Color> list2 = new List<Color>();
            if (!DA.GetDataList<Color>(1, list2))
            {
                return;
            }
            List<Color> list3 = new List<Color>();
            if (!DA.GetDataList<Color>(2, list3))
            {
                return;
            }
            bool linear = false;
            if (!DA.GetData<bool>(3, ref linear))
            {
                return;
            }
            bool locked = false;
            if (!DA.GetData<bool>(4, ref locked))
            {
                return;
            }
            double num = 0.0;
            if (!DA.GetData<double>(5, ref num))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(6, ref flag))
            {
                return;
            }
            if (list2.Count == 0 || list3.Count == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, assign at least one List of RGB colors");
                return;
            }
            GH_GradientControl gh_GradientControl = new GH_GradientControl();
            int num2 = Convert.ToInt32(gh_GradientControl.Gradient.GripCount);
            List<double> list4 = list;
            int count = list.Count;
            for (int i = 0; i < num2; i++)
            {
                gh_GradientControl.Gradient.RemoveGrip(0);
            }
            for (int j = 0; j < count; j++)
            {
                gh_GradientControl.Gradient.AddGrip(list4[j], list2[j], list3[j]);
            }
            gh_GradientControl.Gradient.Linear = linear;
            gh_GradientControl.Gradient.Locked = locked;
            Color color = gh_GradientControl.Gradient.ColourAt(num);
            this.Component = this;
            this.GrasshopperDocument = base.OnPingDocument();
            if (flag)
            {
                gh_GradientControl.CreateAttributes();
                gh_GradientControl.Attributes.Pivot = new PointF(this.Component.Attributes.Pivot.X + 60f, this.Component.Attributes.Pivot.Y - 100f);
                this.GrasshopperDocument.AddObject(gh_GradientControl, false, int.MaxValue);
            }
            DA.SetData(0, color);
        }

        // Token: 0x1700005F RID: 95
        // (get) Token: 0x060000DA RID: 218 RVA: 0x000073EE File Offset: 0x000055EE
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000060 RID: 96
        // (get) Token: 0x060000DB RID: 219 RVA: 0x000073F1 File Offset: 0x000055F1
        protected override Bitmap Icon
        {
            get
            {
                return Resources.GradGen_logo_24;
            }
        }

        // Token: 0x17000061 RID: 97
        // (get) Token: 0x060000DC RID: 220 RVA: 0x000073F8 File Offset: 0x000055F8
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("BEF213B9-6C4F-461B-AFF9-D695A1EFD069");
            }
        }

        // Token: 0x04000032 RID: 50
        private GH_Document GrasshopperDocument;

        // Token: 0x04000033 RID: 51
        private IGH_Component Component;

        // Token: 0x04000034 RID: 52
        public bool myMenu_info;
    }
}
