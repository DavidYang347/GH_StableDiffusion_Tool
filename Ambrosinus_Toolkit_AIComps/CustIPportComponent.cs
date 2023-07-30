using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.AIComps
{
    // Token: 0x02000020 RID: 32
    public class CustIPportComponent : GH_Component
    {
        // Token: 0x06000135 RID: 309 RVA: 0x0000AC90 File Offset: 0x00008E90
        public CustIPportComponent() : base("CustIPport_loc", "LA_CustIPport_loc", "Set a custom IP address after you have added a custom IP port number, please be sure to have ticked the 'listen' argument!\nby Luciano Ambrosini", "Ambrosinus", "3.AI")
        {
        }

        // Token: 0x06000136 RID: 310 RVA: 0x0000ACB4 File Offset: 0x00008EB4
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Custom IP", "CustIP", "Set a custom IP address after you have added a custom IP port number, please be sure to have ticked the 'listen' argument!", 0, "127.0.0.1");
            pManager.AddTextParameter("Custom Port", "CustPort", "Set a custom Port number, please be sure to have ticked the 'listen' argument and have you launched the WebUI arguments with the same port number!", 0, "7860");
            pManager[0].MutableNickName = false;
            pManager[0].MutableNickName = false;
        }

        // Token: 0x06000137 RID: 311 RVA: 0x0000AD13 File Offset: 0x00008F13
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Set IPport", "SetIPport", "The Ip address and the port set", 0);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x06000138 RID: 312 RVA: 0x0000AD39 File Offset: 0x00008F39
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000139 RID: 313 RVA: 0x0000AD48 File Offset: 0x00008F48
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Project Info", new EventHandler(this.Menu_A_DoClick), Resources.resprj_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "About SD locally", new EventHandler(this.Menu_B_DoClick), Resources.comps_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Requirements", new EventHandler(this.Menu_C_DoClick), Resources.requirem_logo1_24);
        }

        // Token: 0x0600013A RID: 314 RVA: 0x0000ADD0 File Offset: 0x00008FD0
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x0600013B RID: 315 RVA: 0x0000ADFC File Offset: 0x00008FFC
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/atoolkit-research-project/");
            }
        }

        // Token: 0x0600013C RID: 316 RVA: 0x0000AE28 File Offset: 0x00009028
        private void Menu_B_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_B = !this.myMenu_B)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally");
            }
        }

        // Token: 0x0600013D RID: 317 RVA: 0x0000AE54 File Offset: 0x00009054
        private void Menu_C_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_C = !this.myMenu_C)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally#part1");
            }
        }

        // Token: 0x0600013E RID: 318 RVA: 0x0000AE80 File Offset: 0x00009080
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "127.0.0.1";
            if (base.Params.Input[0].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default IP address is 127.0.0.1");
            }
            else
            {
                DA.GetData<string>(0, ref text);
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "I suggest this IP should be the same showed on the server machine at IPv4 value through 'ipconfig command'");
            }
            string text2 = "7860";
            if (base.Params.Input[1].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default Port is 7860");
            }
            else
            {
                DA.GetData<string>(1, ref text2);
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "In order to work you have to 'open' this port on the server machine as TCP inbound traffic");
            }
            ListenIPport.IPaddress = text;
            ListenIPport.CustPort = text2;
            string text3 = string.Format("http://{0}:{1}", text, text2);
            DA.SetData(0, text3);
        }

        // Token: 0x1700007A RID: 122
        // (get) Token: 0x0600013F RID: 319 RVA: 0x0000AF43 File Offset: 0x00009143
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)8;
            }
        }

        // Token: 0x1700007B RID: 123
        // (get) Token: 0x06000140 RID: 320 RVA: 0x0000AF46 File Offset: 0x00009146
        protected override Bitmap Icon
        {
            get
            {
                return Resources.IPport_icons2_24;
            }
        }

        // Token: 0x1700007C RID: 124
        // (get) Token: 0x06000141 RID: 321 RVA: 0x0000AF4D File Offset: 0x0000914D
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("B3C0E57A-49A2-435D-AF4F-EE03CA879F5A");
            }
        }

        // Token: 0x04000045 RID: 69
        public bool myMenu_info;

        // Token: 0x04000046 RID: 70
        public bool myMenu_A;

        // Token: 0x04000047 RID: 71
        public bool myMenu_B;

        // Token: 0x04000048 RID: 72
        public bool myMenu_C;
    }
}
