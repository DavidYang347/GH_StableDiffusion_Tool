using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using Grasshopper.Kernel;


namespace Ambrosinus_Toolkit.About
{
    // Token: 0x02000027 RID: 39
    public class ToolkitVersionComponent : GH_Component
    {
        // Token: 0x0600018E RID: 398 RVA: 0x0000E5E6 File Offset: 0x0000C7E6
        public ToolkitVersionComponent() : base("Ambrosinus-Toolkit Info", "Version", "Returns the Ambrosinus-Toolkit version info", "Ambrosinus", "0.About")
        {
        }

        // Token: 0x0600018F RID: 399 RVA: 0x0000E607 File Offset: 0x0000C807
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        // Token: 0x06000190 RID: 400 RVA: 0x0000E60C File Offset: 0x0000C80C
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("YourVersion", "YourVers", "Toolkit version info", 0);
            pManager.AddTextParameter("LatestVersion", "CheckVers", "Check and only notify the latest Toolkit version available (install it manually from Food4Rhino or automatically from Rhino Package Manager)", 0);
            pManager.AddTextParameter("AuthorContacts", "Contacts", "Ambrosinus-Toolkit contacts", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        // Token: 0x06000191 RID: 401 RVA: 0x0000E688 File Offset: 0x0000C888
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            string message = "v" + new ToolkitVersionProperties().Version;
            base.Message = message;
            base.MutableNickName = false;
        }

        // Token: 0x06000192 RID: 402 RVA: 0x0000E6C0 File Offset: 0x0000C8C0
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "Website", new EventHandler(this.Menu_Main_DoClick), Resources.tab_icon_Ambrosinus);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "GitHub", new EventHandler(this.Menu_A_DoClick), Resources.GitHub_logo1_24);
        }

        // Token: 0x06000193 RID: 403 RVA: 0x0000E72C File Offset: 0x0000C92C
        private void Menu_Main_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_Main = !this.myMenu_Main)
            {
                Process.Start("http://www.lucianoambrosini.it");
            }
        }

        // Token: 0x06000194 RID: 404 RVA: 0x0000E758 File Offset: 0x0000C958
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000195 RID: 405 RVA: 0x0000E784 File Offset: 0x0000C984
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://github.com/lucianoambrosini");
            }
        }

        // Token: 0x06000196 RID: 406 RVA: 0x0000E7B0 File Offset: 0x0000C9B0
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string authorContact = new AmbrosinusToolkitInfo().AuthorContact;
            string currentVersionInfo = new ReadCloudTxtFile().CurrentVersionInfo;
            string latestVersionInfo = new ReadCloudTxtFile().LatestVersionInfo;
            string version = new ToolkitVersionProperties().Version;
            char c = '.';
            string[] array = version.Split(new char[]
            {
                c
            }, StringSplitOptions.RemoveEmptyEntries);
            string[] array2 = latestVersionInfo.Split(new char[]
            {
                c
            }, StringSplitOptions.RemoveEmptyEntries);
            int num = Convert.ToInt32(array[0]);
            int num2 = Convert.ToInt32(array[1]);
            int num3 = Convert.ToInt32(array[2]);
            int num4 = Convert.ToInt32(string.Format("{0}{1}{2}", num, num2, num3));
            int num5 = Convert.ToInt32(array2[0]);
            int num6 = Convert.ToInt32(array2[1]);
            int num7 = Convert.ToInt32(array2[2]);
            int num8 = Convert.ToInt32(string.Format("{0}{1}{2}", num5, num6, num7));
            string str;
            if (num4 == num8)
            {
                str = "Your Toolkit version is the latest one. CONGRATULATIONS!";
            }
            else if (num4 > num8)
            {
                str = "Your are testing a WIP Toolkit version. YOUR ARE A DEV!";
            }
            else
            {
                str = "Your Toolkit version is OUTDATED, PLEASE UPDATE with the latest version! (manually from Food4Rhino or automatically from Rhino Package Manager)\n\nPlease, go here: https://github.com/lucianoambrosini/Ambrosinus-Toolkit";
            }
            string text = "INSTALLED VERSION: " + version + "\n\n" + str;
            DA.SetData(0, text);
            DA.SetData(1, currentVersionInfo);
            DA.SetData(2, authorContact);
        }

        // Token: 0x1700008F RID: 143
        // (get) Token: 0x06000197 RID: 407 RVA: 0x0000E8F2 File Offset: 0x0000CAF2
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000090 RID: 144
        // (get) Token: 0x06000198 RID: 408 RVA: 0x0000E8F5 File Offset: 0x0000CAF5
        protected override Bitmap Icon
        {
            get
            {
                return Resources.infoVersion_logo_24;
            }
        }

        // Token: 0x17000091 RID: 145
        // (get) Token: 0x06000199 RID: 409 RVA: 0x0000E8FC File Offset: 0x0000CAFC
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("E11F2CE6-7A50-44C9-BF96-BB1F0F4F58D9");
            }
        }

        // Token: 0x0400005D RID: 93
        public bool myMenu_Main;

        // Token: 0x0400005E RID: 94
        public bool myMenu_info;

        // Token: 0x0400005F RID: 95
        public bool myMenu_A;
    }
}
