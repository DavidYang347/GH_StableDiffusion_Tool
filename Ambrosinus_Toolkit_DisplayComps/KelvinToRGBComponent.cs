using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.DisplayComps
{
    // Token: 0x0200001B RID: 27
    public class KelvinToRGBComponent : GH_Component
    {
        // Token: 0x060000FB RID: 251 RVA: 0x00007DEA File Offset: 0x00005FEA
        public KelvinToRGBComponent() : base("KelvinToRGB", "LA_KelvinToRGB", "Convert Kelvin temperature to RGB value\nby Luciano Ambrosini", "Ambrosinus", "1.Display")
        {
        }

        // Token: 0x060000FC RID: 252 RVA: 0x00007E0B File Offset: 0x0000600B
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Ktemp", "Ktemp", "Assign a Kelvin temperature value (integer) as a list (default: 2700K)", (GH_ParamAccess)1, 2700);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x060000FD RID: 253 RVA: 0x00007E38 File Offset: 0x00006038
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("convRGB", "convRGB", "RGB colours from Kelvin temperature input", (GH_ParamAccess)1);
            pManager.AddTextParameter("Text", "Text", "info about Kelvin temperature converted", (GH_ParamAccess)1);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x060000FE RID: 254 RVA: 0x00007E8D File Offset: 0x0000608D
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000FF RID: 255 RVA: 0x00007E9C File Offset: 0x0000609C
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x06000100 RID: 256 RVA: 0x00007EC4 File Offset: 0x000060C4
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000101 RID: 257 RVA: 0x00007EF0 File Offset: 0x000060F0
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<int> list = new List<int>();
            if (!DA.GetDataList<int>(0, list))
            {
                return;
            }
            List<double> list2 = new List<double>();
            List<Color> list3 = new List<Color>();
            List<string> list4 = new List<string>();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                if (list[i] == 0)
                {
                    list2.Add(Convert.ToDouble(1));
                }
                else
                {
                    list2.Add(Convert.ToDouble(list[i]) / 100.0);
                }
            }
            for (int j = 0; j < count; j++)
            {
                int num;
                if (list2[j] <= 66.0)
                {
                    num = 255;
                }
                else
                {
                    double x = list2[j] - 60.0;
                    num = Convert.ToInt32(329.698727466 * Math.Pow(x, -0.1332047592));
                    if (num < 0)
                    {
                        num = 0;
                    }
                    if (num > 255)
                    {
                        num = 255;
                    }
                }
                int num3;
                if (list2[j] <= 66.0)
                {
                    double num2 = list2[j];
                    num3 = Convert.ToInt32(99.4708025861 * Math.Log(num2) - 161.1195681661);
                    if (num3 < 0)
                    {
                        num3 = 0;
                    }
                    if (num3 > 255)
                    {
                        num3 = 255;
                    }
                }
                else
                {
                    double num2 = list2[j] - 60.0;
                    num3 = Convert.ToInt32(288.1221695283 * Math.Pow(num2, -0.0755148492));
                    if (num3 < 0)
                    {
                        num3 = 0;
                    }
                    if (num3 > 255)
                    {
                        num3 = 255;
                    }
                }
                int num4;
                if (list2[j] >= 66.0)
                {
                    num4 = 255;
                }
                else if (list2[j] <= 19.0)
                {
                    num4 = 0;
                }
                else
                {
                    double d = list2[j] - 10.0;
                    num4 = Convert.ToInt32(138.5177312231 * Math.Log(d) - 305.0447927307);
                    if (num4 < 0)
                    {
                        num4 = 0;
                    }
                    if (num4 > 255)
                    {
                        num4 = 255;
                    }
                }
                list3.Add(Color.FromArgb(num, num3, num4));
                List<int> list5 = new List<int>
                {
                    1700,
                    1850,
                    2400,
                    2550,
                    2700,
                    3000,
                    3200,
                    3350,
                    5000,
                    6200,
                    6500
                };
                List<string> list6 = new List<string>
                {
                    "Match flame low pressure sodium lamps (LPS/SOX)",
                    "Candle flame sunset/sunrise",
                    "Standard incandescent lamps",
                    "Soft white incandescent lamps",
                    "Soft white compact fluorescent and LED lamps",
                    "Warm white compact fluorescent and LED lamps",
                    "Studio lamps\u00a0photofloods etc",
                    "Studio CP light",
                    "Horizon daylight | Tubular fluorescent lamps or cool white/daylight | compact fluorescent lamps (CFL)",
                    "Xenon short-arc lamp",
                    "Daylight overcast"
                };
                int count2 = list6.Count;
                string str = null;
                for (int k = 0; k < count2; k++)
                {
                    if (list[j] == list5[k])
                    {
                        str = list6[k];
                        break;
                    }
                    if (list[j] >= 5500 && list[j] <= 6000)
                    {
                        str = "Vertical daylight, electronic flash";
                        break;
                    }
                    if (list[j] >= 6500 && list[j] <= 9500)
                    {
                        str = "LCD or CRT screen";
                        break;
                    }
                    if (list[j] >= 15000 && list[j] <= 27000)
                    {
                        str = "Clear blue poleward sky";
                        break;
                    }
                }
                list4.Add((list2[j] * 100.0).ToString() + "K " + str);
            }
            List<Color> list7 = new List<Color>();
            List<string> list8 = new List<string>();
            list7 = list3;
            list8 = list4;
            DA.SetDataList(0, list7);
            DA.SetDataList(1, list8);
        }

        // Token: 0x1700006B RID: 107
        // (get) Token: 0x06000102 RID: 258 RVA: 0x0000835C File Offset: 0x0000655C
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)4;
            }
        }

        // Token: 0x1700006C RID: 108
        // (get) Token: 0x06000103 RID: 259 RVA: 0x0000835F File Offset: 0x0000655F
        protected override Bitmap Icon
        {
            get
            {
                return Resources.KtoRGB_logo2_24;
            }
        }

        // Token: 0x1700006D RID: 109
        // (get) Token: 0x06000104 RID: 260 RVA: 0x00008366 File Offset: 0x00006566
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("4e79bc9f-60ea-4a72-afc6-259c702d6900");
            }
        }

        // Token: 0x04000038 RID: 56
        public bool myMenu_info;
    }
}
