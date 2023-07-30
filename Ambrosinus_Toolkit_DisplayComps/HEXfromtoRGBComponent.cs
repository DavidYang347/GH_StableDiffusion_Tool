using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.DisplayComps
{
    // Token: 0x0200001A RID: 26
    public class HEXfromtoRGBComponent : GH_Component
    {
        // Token: 0x060000F1 RID: 241 RVA: 0x00007A1E File Offset: 0x00005C1E
        public HEXfromtoRGBComponent() : base("HEXfromtoRGB", "LA_HEXfromtoRGB", "Convert HEX color to RGB value and viceversa \nby Luciano Ambrosini", "Ambrosinus", "1.Display")
        {
        }

        // Token: 0x060000F2 RID: 242 RVA: 0x00007A40 File Offset: 0x00005C40
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("HEX", "HEX", "Assign a HEX value with '#' as a list (default: white)", (GH_ParamAccess)1, "#ffffff");
            pManager.AddColourParameter("RGB", "RGB", "Assign a RGB value as a list (default: white)", (GH_ParamAccess)1, Color.White);
            pManager.AddBooleanParameter("lowcase", "lowcase", "lowcase HEX values layout True/False", 0, false);
            pManager.AddBooleanParameter("hashtag", "hashtag", "hashtag prefix for the HEX values layout True/False", 0, false);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
        }

        // Token: 0x060000F3 RID: 243 RVA: 0x00007AEC File Offset: 0x00005CEC
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("convRGB", "convRGB", "RGB colours from HEX input", (GH_ParamAccess)1);
            pManager.AddTextParameter("convHEX", "convHEX", "HEX colours from RGB input", (GH_ParamAccess)1);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x060000F4 RID: 244 RVA: 0x00007B41 File Offset: 0x00005D41
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000F5 RID: 245 RVA: 0x00007B50 File Offset: 0x00005D50
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000F6 RID: 246 RVA: 0x00007B78 File Offset: 0x00005D78
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000F7 RID: 247 RVA: 0x00007BA4 File Offset: 0x00005DA4
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> list = new List<string>();
            if (!DA.GetDataList<string>(0, list))
            {
                return;
            }
            List<Color> list2 = new List<Color>();
            if (!DA.GetDataList<Color>(1, list2))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(2, ref flag))
            {
                return;
            }
            bool flag2 = false;
            if (!DA.GetData<bool>(3, ref flag2))
            {
                return;
            }
            List<Color> list3 = new List<Color>();
            for (int i = 0; i < list.Count; i++)
            {
                list3.Add(ColorTranslator.FromHtml(list[i]));
            }
            List<string> list4 = new List<string>();
            List<string> list5 = new List<string>();
            List<string> list6 = new List<string>();
            List<string> list7 = new List<string>();
            string arg = "#";
            for (int j = 0; j < list2.Count; j++)
            {
                list4.Add(list2[j].R.ToString("X2") + list2[j].G.ToString("X2") + list2[j].B.ToString("X2"));
                list5.Add(list4[j]);
                list6.Add(list5[j].ToLower());
            }
            if (flag && flag2)
            {
                using (List<string>.Enumerator enumerator = list6.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string arg2 = enumerator.Current;
                        list7.Add(string.Format("{0}{1}", arg, arg2));
                    }
                    goto IL_1DA;
                }
            }
            if (!flag && flag2)
            {
                using (List<string>.Enumerator enumerator = list4.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string arg3 = enumerator.Current;
                        list7.Add(string.Format("{0}{1}", arg, arg3));
                    }
                    goto IL_1DA;
                }
            }
            if (flag && !flag2)
            {
                list7 = list6;
            }
            else
            {
                list7 = list4;
            }
        IL_1DA:
            List<Color> list8 = new List<Color>();
            List<string> list9 = new List<string>();
            list8 = list3;
            list9 = list7;
            DA.SetDataList(0, list8);
            DA.SetDataList(1, list9);
        }

        // Token: 0x17000068 RID: 104
        // (get) Token: 0x060000F8 RID: 248 RVA: 0x00007DD4 File Offset: 0x00005FD4
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)4;
            }
        }

        // Token: 0x17000069 RID: 105
        // (get) Token: 0x060000F9 RID: 249 RVA: 0x00007DD7 File Offset: 0x00005FD7
        protected override Bitmap Icon
        {
            get
            {
                return Resources.HEXfromtoRGBlogo_24;
            }
        }

        // Token: 0x1700006A RID: 106
        // (get) Token: 0x060000FA RID: 250 RVA: 0x00007DDE File Offset: 0x00005FDE
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("989e24cf-e5f6-4ecc-beaa-d0be53ea4040");
            }
        }

        // Token: 0x04000037 RID: 55
        public bool myMenu_info;
    }
}
