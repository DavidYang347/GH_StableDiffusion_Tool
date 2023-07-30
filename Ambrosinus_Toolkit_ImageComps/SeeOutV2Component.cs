using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace Ambrosinus_Toolkit.ImageComps
{
    // Token: 0x02000014 RID: 20
    public class SeeOutV2Component : GH_Component
    {
        // Token: 0x060000B5 RID: 181 RVA: 0x00006522 File Offset: 0x00004722
        public SeeOutV2Component() : base("SeeOut", "LA_SeeOut", "SeeOut component gives as output the sequential file paths of the images generatedby OpenAI or StabilityAI components.By ID slider you can change the item ID shown in the SeqOut,instead LastOut shows the latest file path\nby Luciano Ambrosini", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x060000B6 RID: 182 RVA: 0x00006544 File Offset: 0x00004744
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("output", "output", "'output' list from OpenAI or StabilityAI components", (GH_ParamAccess)1);
            pManager.AddIntegerParameter("ID", "ID", "Integer number to identify the image file path inside the output.\nThe domain of the Slider Number connected to 'ID' input, will be adapted according to the output list size", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x0000659C File Offset: 0x0000479C
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SeqOut", "SeqOut", "Image file path according to 'SeqID' slider value", 0);
            pManager.AddTextParameter("LastOut", "LastOut", "Latest image file path of AI generated images", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x000065F1 File Offset: 0x000047F1
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x00006600 File Offset: 0x00004800
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000BA RID: 186 RVA: 0x00006628 File Offset: 0x00004828
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000BB RID: 187 RVA: 0x00006654 File Offset: 0x00004854
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> list = new List<string>();
            if (!DA.GetDataList<string>(0, list))
            {
                return;
            }
            int num = 0;
            if (!DA.GetData<int>(1, ref num))
            {
                return;
            }
            List<string> list2 = new List<string>();
            string directoryName = Path.GetDirectoryName(list[0]);
            if (directoryName == "" || directoryName == null)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please,Be sure have run LA_StabilityAI-GH component or LA_OpenAI-GH component\n and give its image-full-path output as 'output' input of the 'SeeOut' component!");
                return;
            }
            foreach (string item in from d in Directory.GetFiles(directoryName)
                                    orderby new FileInfo(d).CreationTime descending
                                    select d)
            {
                list2.Add(item);
            }
            if (list2.Count<string>() == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please,Be sure have run LA_StabilityAI-GH component or LA_OpenAI-GH component\n and give its image-full-path output as input of the 'SeeOut' component!");
                return;
            }
            if (num > list2.Count<string>())
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please,decrease ID value\n Your ID value is out of range!");
                return;
            }
            list2.Reverse();
            int num2 = list2.Count<string>() - 1;
            base.OnPingDocument();
            if (base.Params.Input[1].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, assign to 'ID' at least a Slider Number component");
                return;
            }
            GH_NumberSlider gh_NumberSlider = base.Params.Input[1].Sources[0] as GH_NumberSlider;
            if (gh_NumberSlider != null)
            {
                gh_NumberSlider.Slider.Minimum = 0m;
                gh_NumberSlider.Slider.Maximum = num2;
            }
            string text = list2[num];
            string text2 = list2[num2];
            DA.SetData(0, text);
            DA.SetData(1, text2);
        }

        // Token: 0x17000059 RID: 89
        // (get) Token: 0x060000BC RID: 188 RVA: 0x00006808 File Offset: 0x00004A08
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x1700005A RID: 90
        // (get) Token: 0x060000BD RID: 189 RVA: 0x0000680B File Offset: 0x00004A0B
        protected override Bitmap Icon
        {
            get
            {
                return Resources.SeeOut_icon1_24;
            }
        }

        // Token: 0x1700005B RID: 91
        // (get) Token: 0x060000BE RID: 190 RVA: 0x00006812 File Offset: 0x00004A12
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("9ee82528-6754-481b-a79d-8299fa785e24");
            }
        }

        // Token: 0x04000030 RID: 48
        public bool myMenu_info;
    }
}
