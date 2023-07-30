using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.AIComps
{
    // Token: 0x0200001F RID: 31
    public class AnsPromptComponent : GH_Component
    {
        // Token: 0x06000129 RID: 297 RVA: 0x0000AA02 File Offset: 0x00008C02
        public AnsPromptComponent() : base("AnsToPrompt", "LA_AnsToPrompt", "Convert Ask To OpenAI answer in a text prompt\nby Luciano Ambrosini", "Ambrosinus", "3.AI")
        {
        }

        // Token: 0x0600012A RID: 298 RVA: 0x0000AA23 File Offset: 0x00008C23
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Ans", "Ans", "Pass 'Ask To OpenAI' answer as input", 0);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x0600012B RID: 299 RVA: 0x0000AA49 File Offset: 0x00008C49
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Prompt", "Prompt", "Ans as text prompt", 0);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x0600012C RID: 300 RVA: 0x0000AA6F File Offset: 0x00008C6F
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x0600012D RID: 301 RVA: 0x0000AA80 File Offset: 0x00008C80
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Project Info", new EventHandler(this.Menu_A_DoClick), Resources.resprj_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "About AskGPT-GH", new EventHandler(this.Menu_B_DoClick), Resources.comps_logo1_24);
        }

        // Token: 0x0600012E RID: 302 RVA: 0x0000AAEC File Offset: 0x00008CEC
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x0600012F RID: 303 RVA: 0x0000AB18 File Offset: 0x00008D18
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/atoolkit-research-project/");
            }
        }

        // Token: 0x06000130 RID: 304 RVA: 0x0000AB44 File Offset: 0x00008D44
        private void Menu_B_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_B = !this.myMenu_B)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/openai-asktoai-inside-grasshopper-with-python/");
            }
        }

        // Token: 0x06000131 RID: 305 RVA: 0x0000AB70 File Offset: 0x00008D70
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            if (!DA.GetData<string>(0, ref text))
            {
                return;
            }
            string text2;
            if (text == null || text == "Ask me something, I will surprise you! ;)")
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, run Ask To OpenAI component and give Ans as input!");
                text2 = "Ask me something, I will surprise you! ;)";
                DA.SetData(0, text2);
                return;
            }
            int num = 3;
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>(text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < num; i++)
            {
                list3.RemoveAt(0);
            }
            for (int j = 0; j < list3.Count; j++)
            {
                char c = list3[j].ToCharArray()[0];
                if ('#' == c)
                {
                    list.Add("\n");
                }
                else
                {
                    list.Add("\n\n");
                }
                list2.Add(list3[j] + list[j]);
            }
            text2 = string.Join("", list2);
            DA.SetData(0, text2);
        }

        // Token: 0x17000077 RID: 119
        // (get) Token: 0x06000132 RID: 306 RVA: 0x0000AC7A File Offset: 0x00008E7A
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000078 RID: 120
        // (get) Token: 0x06000133 RID: 307 RVA: 0x0000AC7D File Offset: 0x00008E7D
        protected override Bitmap Icon
        {
            get
            {
                return Resources.AI_AskToPrompt_icon1_24;
            }
        }

        // Token: 0x17000079 RID: 121
        // (get) Token: 0x06000134 RID: 308 RVA: 0x0000AC84 File Offset: 0x00008E84
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("5CDC95D1-978E-4768-A510-6CA8CECEAAFA");
            }
        }

        // Token: 0x04000042 RID: 66
        public bool myMenu_info;

        // Token: 0x04000043 RID: 67
        public bool myMenu_A;

        // Token: 0x04000044 RID: 68
        public bool myMenu_B;
    }
}
