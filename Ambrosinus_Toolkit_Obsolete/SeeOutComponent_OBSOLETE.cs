using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Functions;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace Ambrosinus_Toolkit.Obsolete
{
    // Token: 0x0200000F RID: 15
    public class SeeOutComponent_OBSOLETE : GH_Component
    {
        // Token: 0x06000083 RID: 131 RVA: 0x00004D3E File Offset: 0x00002F3E
        public SeeOutComponent_OBSOLETE() : base("SeeOut", "LA_SeeOut", "SeeOut component gives as output the sequential file paths of the images generatedby OpenAI or StabilityAI components.By ID slider you can change the item ID shown in the SeqOut,instead LastOut shows the latest file path\nby Luciano Ambrosini", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x06000084 RID: 132 RVA: 0x00004D60 File Offset: 0x00002F60
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("output", "output", "'output' list from OpenAI or StabilityAI components", (GH_ParamAccess)1);
            pManager.AddIntegerParameter("ID", "ID", "Integer number to identify the image file path inside the output.\nGive to the slider connected to 'ID' this name: 'SeqID', it will be updated automatically according to output size list'", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x06000085 RID: 133 RVA: 0x00004DB8 File Offset: 0x00002FB8
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("SeqOut", "SeqOut", "Image file path according to 'SeqID' slider value", 0);
            pManager.AddTextParameter("LastOut", "LastOut", "Latest image file path of AI generated images", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x06000086 RID: 134 RVA: 0x00004E0D File Offset: 0x0000300D
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000087 RID: 135 RVA: 0x00004E1C File Offset: 0x0000301C
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x06000088 RID: 136 RVA: 0x00004E44 File Offset: 0x00003044
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000089 RID: 137 RVA: 0x00004E70 File Offset: 0x00003070
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
            string name = "SeqID";
            GH_NumberSlider gh_NumberSlider = (GH_NumberSlider)CustomFX.FindObj(base.OnPingDocument(), name, null);
            if (gh_NumberSlider == null)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, assign to 'SeeOut' component\na ID-slider with 'SeqID' as its name!");
                return;
            }
            gh_NumberSlider.Slider.Minimum = 0m;
            gh_NumberSlider.Slider.Maximum = num2;
            gh_NumberSlider.Slider.FixDomain();
            gh_NumberSlider.TrySetSliderValue(num2);
            string text = list2[num];
            string text2 = list2[num2];
            DA.SetData(0, text);
            DA.SetData(1, text2);
        }

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x0600008A RID: 138 RVA: 0x00005018 File Offset: 0x00003218
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)(-1);
            }
        }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x0600008B RID: 139 RVA: 0x0000501B File Offset: 0x0000321B
        protected override Bitmap Icon
        {
            get
            {
                return Resources.SeeOut_icon1_24;
            }
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x0600008C RID: 140 RVA: 0x00005022 File Offset: 0x00003222
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("AFBD8925-4580-491D-90E7-646025BD26BC");
            }
        }

        // Token: 0x0400002B RID: 43
        public bool myMenu_info;
    }
}
