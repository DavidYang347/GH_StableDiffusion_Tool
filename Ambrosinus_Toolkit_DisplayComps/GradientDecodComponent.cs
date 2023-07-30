using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.GUI.Gradient;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;

namespace Ambrosinus_Toolkit.DisplayComps
{
    // Token: 0x02000019 RID: 25
    public class GradientDecodComponent : GH_Component
    {
        // Token: 0x060000E7 RID: 231 RVA: 0x000076C8 File Offset: 0x000058C8
        public GradientDecodComponent() : base("GradientDecod", "LA_GradDecod", "Gradient Decoder allows to deconstruct gradient component, simplify an assigned gradient with many grip points and convert HEX colour value to RGB value \nby Luciano Ambrosini", "Ambrosinus", "1.Display")
        {
        }

        // Token: 0x060000E8 RID: 232 RVA: 0x000076EC File Offset: 0x000058EC
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("gradient", "gradient", "Assign a Gradient component", 0);
            pManager.AddIntegerParameter("sp", "sp", "number of grip points (default sample=2)", 0, 2);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x060000E9 RID: 233 RVA: 0x00007744 File Offset: 0x00005944
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("numGrips", "numGrips", "Number of grip points from source gradient component", 0);
            pManager.AddColourParameter("sourceRGB", "sourceRGB", "Source RGB colours palette", (GH_ParamAccess)1);
            pManager.AddColourParameter("targetRGB", "targetRGB", "Target RGB colours palette after sampling", (GH_ParamAccess)1);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        // Token: 0x060000EA RID: 234 RVA: 0x000077BD File Offset: 0x000059BD
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000EB RID: 235 RVA: 0x000077CC File Offset: 0x000059CC
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000EC RID: 236 RVA: 0x000077F4 File Offset: 0x000059F4
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000ED RID: 237 RVA: 0x00007820 File Offset: 0x00005A20
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            GH_Colour gh_Colour = new GH_Colour();
            if (!DA.GetData<GH_Colour>(0, ref gh_Colour))
            {
                return;
            }
            int num = 0;
            if (!DA.GetData<int>(1, ref num))
            {
                return;
            }
            if (gh_Colour == null)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, assign at least one Gradient component");
                return;
            }
            List<Color> list = new List<Color>();
            int num2 = 0;
            int num3 = 0;
            List<Color> list2 = new List<Color>();
            List<Color> list3 = new List<Color>();
            try
            {
                GH_GradientControl gh_GradientControl = (GH_GradientControl)base.Params.Input[0].Sources[0].Attributes.GetTopLevel.DocObject;
                GH_PersistentParam<GH_Number> gh_PersistentParam = (GH_PersistentParam<GH_Number>)gh_GradientControl.Params.Input[2];
                if (gh_PersistentParam.VolatileData.IsEmpty)
                {
                    gh_PersistentParam.PersistentData.Append(new GH_Number(1.0));
                    gh_PersistentParam.ExpireSolution(true);
                }
                GH_Gradient gradient = gh_GradientControl.Gradient;
                num2 = gradient.GripCount;
                for (int i = 0; i < gradient.GripCount; i++)
                {
                    list.Add(gradient[i].ColourLeft);
                }
                num3 = num2;
                list2 = list;
            }
            catch
            {
            }
            List<int> list4 = new List<int>();
            List<Color> list5 = new List<Color>();
            num--;
            int count = list.Count;
            int num4 = Convert.ToInt32(count / num);
            if (num4 <= 0 || num + 1 > num2)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, reduce the size of the sampling!(max val=" + num2.ToString() + ")");
                return;
            }
            for (int j = 0; j < num; j++)
            {
                list4.Add(j * num4);
                list5.Add(list[list4[j]]);
            }
            list4.Add(count);
            list5.Add(list[count - 1]);
            list3 = list5;
            DA.SetData(0, num3);
            DA.SetDataList(1, list2);
            DA.SetDataList(2, list3);
        }

        // Token: 0x17000065 RID: 101
        // (get) Token: 0x060000EE RID: 238 RVA: 0x00007A08 File Offset: 0x00005C08
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000066 RID: 102
        // (get) Token: 0x060000EF RID: 239 RVA: 0x00007A0B File Offset: 0x00005C0B
        protected override Bitmap Icon
        {
            get
            {
                return Resources.GradGenSemplogo_24;
            }
        }

        // Token: 0x17000067 RID: 103
        // (get) Token: 0x060000F0 RID: 240 RVA: 0x00007A12 File Offset: 0x00005C12
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("6142D9C1-DAA2-4766-840F-D277E54756D7");
            }
        }

        // Token: 0x04000036 RID: 54
        public bool myMenu_info;
    }
}
