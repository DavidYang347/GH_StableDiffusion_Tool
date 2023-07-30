using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.ManageComps
{
    // Token: 0x02000010 RID: 16
    public class OpenDirComponent : GH_Component
    {
        // Token: 0x0600008D RID: 141 RVA: 0x0000502E File Offset: 0x0000322E
        public OpenDirComponent() : base("OpenDir", "LA_OpenDir", "This component simply open the directory passed as input in Windows File Explorer", "Ambrosinus", "4.Manage")
        {
        }

        // Token: 0x0600008E RID: 142 RVA: 0x00005050 File Offset: 0x00003250
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Directory Path", "DirPath", "Assign the path of the folder wherein save all the images generated", 0);
            pManager.AddBooleanParameter("Open", "Open", "Run this component", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x0600008F RID: 143 RVA: 0x000050A5 File Offset: 0x000032A5
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        // Token: 0x06000090 RID: 144 RVA: 0x000050A7 File Offset: 0x000032A7
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000091 RID: 145 RVA: 0x000050B6 File Offset: 0x000032B6
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x06000092 RID: 146 RVA: 0x000050DC File Offset: 0x000032DC
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000093 RID: 147 RVA: 0x00005108 File Offset: 0x00003308
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            if (!DA.GetData<string>(0, ref text))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(1, ref flag))
            {
                return;
            }
            if (flag)
            {
                if (Directory.Exists(text))
                {
                    Process.Start(text);
                    this.ExpireSolution(true);
                }
                else
                {
                    this.AddRuntimeMessage((GH_RuntimeMessageLevel)20, string.Format("No " + text + " has been found!", Array.Empty<object>()));
                }
            }
            this.ExpireSolution(true);
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x06000094 RID: 148 RVA: 0x00005179 File Offset: 0x00003379
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x06000095 RID: 149 RVA: 0x0000517C File Offset: 0x0000337C
        protected override Bitmap Icon
        {
            get
            {
                return Resources.opendir_icon1_24;
            }
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x06000096 RID: 150 RVA: 0x00005183 File Offset: 0x00003383
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("44C72349-3CA0-4AA5-A999-3EAD72792D87");
            }
        }

        // Token: 0x0400002C RID: 44
        public bool myMenu_info;
    }
}
