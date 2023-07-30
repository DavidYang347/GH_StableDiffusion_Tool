using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.DisplayComps
{
    // Token: 0x0200001C RID: 28
    public class WaveLToRGBComponent : GH_Component
    {
        // Token: 0x06000105 RID: 261 RVA: 0x00008372 File Offset: 0x00006572
        public WaveLToRGBComponent() : base("WaveLToRGB", "LA_WaveLToRGB", "Convert Wavelength (light range admitted 380nm-780nm) to RGB value\nby Luciano Ambrosini", "Ambrosinus", "1.Display")
        {
        }

        // Token: 0x06000106 RID: 262 RVA: 0x00008393 File Offset: 0x00006593
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Wlength", "Wlength", "Assign a Wavelength value between 380nm and 780nm as a list (default: 380nm)", (GH_ParamAccess)1, 380);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x06000107 RID: 263 RVA: 0x000083C0 File Offset: 0x000065C0
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddColourParameter("convRGB", "convRGB", "RGB colours from Kelvin temperature input", (GH_ParamAccess)1);
            pManager.AddTextParameter("Text", "Text", "info about Kelvin temperature converted", (GH_ParamAccess)1);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x06000108 RID: 264 RVA: 0x00008415 File Offset: 0x00006615
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000109 RID: 265 RVA: 0x00008424 File Offset: 0x00006624
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x0600010A RID: 266 RVA: 0x0000844C File Offset: 0x0000664C
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x0600010B RID: 267 RVA: 0x00008478 File Offset: 0x00006678
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
            bool flag = true;
            int count = list.Count;
            if ((count == 1 && list[0] < 380) || list[0] > 780)
            {
                flag = false;
            }
            else if (count != 1)
            {
                for (int i = 0; i < count; i++)
                {
                    if (list[i] < 380 || list[i] > 780)
                    {
                        flag = false;
                        break;
                    }
                    flag = true;
                }
            }
            if (!flag)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please,check your value/s and use only Visible light wavelength (380nm-780nm)");
                list4.Add("380nm");
                list3.Add(Color.FromArgb(97, 0, 97));
            }
            else
            {
                for (int j = 0; j < count; j++)
                {
                    list2.Add(Convert.ToDouble(list[j]));
                    list4.Add(list2[j].ToString() + "nm");
                }
                double y = 0.8;
                int num = 255;
                for (int k = 0; k < count; k++)
                {
                    double num2;
                    double num3;
                    double num4;
                    if (list2[k] >= 380.0 && list2[k] < 440.0)
                    {
                        num2 = -(list2[k] - 440.0) / 60.0;
                        num3 = 0.0;
                        num4 = 1.0;
                    }
                    else if (list2[k] >= 440.0 && list2[k] < 490.0)
                    {
                        num2 = 0.0;
                        num3 = (list2[k] - 440.0) / 50.0;
                        num4 = 1.0;
                    }
                    else if (list2[k] >= 490.0 && list2[k] < 510.0)
                    {
                        num2 = 0.0;
                        num3 = 1.0;
                        num4 = -(list2[k] - 510.0) / 20.0;
                    }
                    else if (list2[k] >= 510.0 && list2[k] < 580.0)
                    {
                        num2 = (list2[k] - 510.0) / 70.0;
                        num3 = 1.0;
                        num4 = 0.0;
                    }
                    else if (list2[k] >= 580.0 && list2[k] < 645.0)
                    {
                        num2 = 1.0;
                        num3 = -(list2[k] - 645.0) / 65.0;
                        num4 = 0.0;
                    }
                    else if (list2[k] >= 645.0 && list2[k] < 781.0)
                    {
                        num2 = 1.0;
                        num3 = 0.0;
                        num4 = 0.0;
                    }
                    else
                    {
                        num2 = 0.0;
                        num3 = 0.0;
                        num4 = 0.0;
                    }
                    double num5;
                    if (list2[k] >= 380.0 && list2[k] < 420.0)
                    {
                        num5 = 0.3 + 0.7 * (list2[k] - 380.0) / 40.0;
                    }
                    else if (list2[k] >= 420.0 && list2[k] < 701.0)
                    {
                        num5 = 1.0;
                    }
                    else if (list2[k] >= 701.0 && list2[k] < 781.0)
                    {
                        num5 = 0.3 + 0.7 * (780.0 - list2[k]) / 80.0;
                    }
                    else
                    {
                        num5 = 0.0;
                    }
                    if (num2 != 0.0)
                    {
                        num2 = Math.Round((double)num * Math.Pow(num2 * num5, y));
                    }
                    if (num3 != 0.0)
                    {
                        num3 = Math.Round((double)num * Math.Pow(num3 * num5, y));
                    }
                    if (num4 != 0.0)
                    {
                        num4 = Math.Round((double)num * Math.Pow(num4 * num5, y));
                    }
                    list3.Add(Color.FromArgb(Convert.ToInt32(num2), Convert.ToInt32(num3), Convert.ToInt32(num4)));
                }
            }
            List<Color> list5 = new List<Color>();
            List<string> list6 = new List<string>();
            list5 = list3;
            list6 = list4;
            DA.SetDataList(0, list5);
            DA.SetDataList(1, list6);
        }

        // Token: 0x1700006E RID: 110
        // (get) Token: 0x0600010C RID: 268 RVA: 0x000089A2 File Offset: 0x00006BA2
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)4;
            }
        }

        // Token: 0x1700006F RID: 111
        // (get) Token: 0x0600010D RID: 269 RVA: 0x000089A5 File Offset: 0x00006BA5
        protected override Bitmap Icon
        {
            get
            {
                return Resources.WtoRGB_logo2_24;
            }
        }

        // Token: 0x17000070 RID: 112
        // (get) Token: 0x0600010E RID: 270 RVA: 0x000089AC File Offset: 0x00006BAC
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("80522776-a24e-408c-8e07-b5e222bbc431");
            }
        }

        // Token: 0x04000039 RID: 57
        public bool myMenu_info;
    }
}
