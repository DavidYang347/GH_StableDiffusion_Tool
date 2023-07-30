using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.Obsolete
{
    // Token: 0x0200000E RID: 14
    public class HEXtoRGBComponent_OBSOLETE : GH_Component
    {
        // Token: 0x06000079 RID: 121 RVA: 0x00004C1B File Offset: 0x00002E1B
        public HEXtoRGBComponent_OBSOLETE() : base("HEXtoRGB", "LA_HEXtoRGB", "Convert HEX color to RGB value \nby Luciano Ambrosini", "Ambrosinus", "Display")
        {
        }

        // Token: 0x0600007A RID: 122 RVA: 0x00004C3C File Offset: 0x00002E3C
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("HEX", "HEX", "Assign a HEX value (with #) as a list", (GH_ParamAccess)1);
        }

        // Token: 0x0600007B RID: 123 RVA: 0x00004C55 File Offset: 0x00002E55
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("convRGB", "convRGB", "RGB colours from HEX input", (GH_ParamAccess)1);
        }

        // Token: 0x0600007C RID: 124 RVA: 0x00004C6E File Offset: 0x00002E6E
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x0600007D RID: 125 RVA: 0x00004C7D File Offset: 0x00002E7D
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x0600007E RID: 126 RVA: 0x00004CA4 File Offset: 0x00002EA4
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x0600007F RID: 127 RVA: 0x00004CD0 File Offset: 0x00002ED0
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> list = new List<string>();
            if (!DA.GetDataList<string>(0, list))
            {
                return;
            }
            List<Color> list2 = new List<Color>();
            for (int i = 0; i < list.Count; i++)
            {
                list2.Add(ColorTranslator.FromHtml(list[i]));
            }
            List<Color> list3 = new List<Color>();
            list3 = list2;
            DA.SetDataList(0, list3);
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x06000080 RID: 128 RVA: 0x00004D28 File Offset: 0x00002F28
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)(-1);
            }
        }

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x06000081 RID: 129 RVA: 0x00004D2B File Offset: 0x00002F2B
        protected override Bitmap Icon
        {
            get
            {
                return Resources.HEXtoRGBlogo_24;
            }
        }

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x06000082 RID: 130 RVA: 0x00004D32 File Offset: 0x00002F32
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E04F0329-7278-48C3-AD78-438A82DE0F4E");
            }
        }

        // Token: 0x0400002A RID: 42
        public bool myMenu_info;
    }
}
