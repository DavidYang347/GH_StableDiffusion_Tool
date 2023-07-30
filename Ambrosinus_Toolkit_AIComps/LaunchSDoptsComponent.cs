using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using Grasshopper.Kernel;
using Newtonsoft.Json;

namespace Ambrosinus_Toolkit.AIComps
{
    // Token: 0x02000022 RID: 34
    public class LaunchSDoptsComponent : GH_Component
    {
        // Token: 0x06000152 RID: 338 RVA: 0x0000B99E File Offset: 0x00009B9E
        public LaunchSDoptsComponent() : base("SDopts_loc", "LA_SDopts_loc", "This component let user setting the Stable Diffusion models desired (previously loaded in '\\models\\Stable-diffusion'). Default model is SD v.1.5\nby Luciano Ambrosini", "Ambrosinus", "3.AI")
        {
        }

        // Token: 0x06000153 RID: 339 RVA: 0x0000B9C0 File Offset: 0x00009BC0
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Check SDmodels", "ChkModels", "Check all loaded SD models", 0);
            pManager.AddIntegerParameter("ID SDModels", "idMods", "Assign an integer value related to the SD Models list as shown in the 'SDMods' output", 0, 0);
            pManager.AddBooleanParameter("Start Process", "Start", "Set the checkpoint Model corresponding to the selected 'idMods'", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        // Token: 0x06000154 RID: 340 RVA: 0x0000BA3A File Offset: 0x00009C3A
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SDModels list", "SDMods", "Shows all your SD checkpoint models", (GH_ParamAccess)1);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x06000155 RID: 341 RVA: 0x0000BA60 File Offset: 0x00009C60
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000156 RID: 342 RVA: 0x0000BA70 File Offset: 0x00009C70
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Project Info", new EventHandler(this.Menu_A_DoClick), Resources.resprj_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "About SD locally", new EventHandler(this.Menu_B_DoClick), Resources.comps_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Requirements", new EventHandler(this.Menu_C_DoClick), Resources.requirem_logo1_24);
        }

        // Token: 0x06000157 RID: 343 RVA: 0x0000BAF8 File Offset: 0x00009CF8
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000158 RID: 344 RVA: 0x0000BB24 File Offset: 0x00009D24
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/atoolkit-research-project/");
            }
        }

        // Token: 0x06000159 RID: 345 RVA: 0x0000BB50 File Offset: 0x00009D50
        private void Menu_B_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_B = !this.myMenu_B)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally");
            }
        }

        // Token: 0x0600015A RID: 346 RVA: 0x0000BB7C File Offset: 0x00009D7C
        private void Menu_C_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_C = !this.myMenu_C)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally#part1");
            }
        }

        // Token: 0x0600015B RID: 347 RVA: 0x0000BBA8 File Offset: 0x00009DA8
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool flag = false;
            if (!DA.GetData<bool>(0, ref flag))
            {
                return;
            }
            bool flag2 = false;
            if (!DA.GetData<bool>(2, ref flag2))
            {
                return;
            }
            string value = "";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            string ipaddress = ListenIPport.IPaddress;
            string custPort = ListenIPport.CustPort;
            string str = string.Format("http://{0}:{1}", ipaddress, custPort);
            if (flag)
            {
                string address = string.Format(str + "/sdapi/v1/sd-models", Array.Empty<object>());
                using (WebClient webClient = new WebClient())
                {
                    value = webClient.DownloadString(address);
                }
                foreach (A11_SDmodels a11_SDmodels in JsonConvert.DeserializeObject<List<A11_SDmodels>>(value))
                {
                    string title = a11_SDmodels.title;
                    list2.Add(title);
                }
            }
            int index = 0;
            string value2 = "";
            if (base.Params.Input[1].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default SDModel checkpoint set is v1.5");
                value2 = "v1-5-pruned-emaonly.safetensors [6ce0161689]";
            }
            else if (flag)
            {
                DA.GetData<int>(1, ref index);
                value2 = list2[index];
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)20, "Please, run 'ChkModels' before!");
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["sd_model_checkpoint"] = value2;
            Dictionary<string, object> value3 = dictionary;
            if (flag2)
            {
                string value4 = JsonConvert.SerializeObject(value3);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(str + "/sdapi/v1/options");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(value4);
                }
                using (StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()))
                {
                    streamReader.ReadToEnd();
                }
            }
            list = list2;
            DA.SetDataList(0, list);
        }

        // Token: 0x17000080 RID: 128
        // (get) Token: 0x0600015C RID: 348 RVA: 0x0000BDC4 File Offset: 0x00009FC4
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)8;
            }
        }

        // Token: 0x17000081 RID: 129
        // (get) Token: 0x0600015D RID: 349 RVA: 0x0000BDC7 File Offset: 0x00009FC7
        protected override Bitmap Icon
        {
            get
            {
                return Resources.SDMods_icon3_24;
            }
        }

        // Token: 0x17000082 RID: 130
        // (get) Token: 0x0600015E RID: 350 RVA: 0x0000BDCE File Offset: 0x00009FCE
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("77B3B911-5E66-4332-8886-E55B859BD521");
            }
        }

        // Token: 0x0400004E RID: 78
        public bool myMenu_info;

        // Token: 0x0400004F RID: 79
        public bool myMenu_A;

        // Token: 0x04000050 RID: 80
        public bool myMenu_B;

        // Token: 0x04000051 RID: 81
        public bool myMenu_C;
    }
}
