using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Display;

namespace Ambrosinus_Toolkit.ImageComps
{
    // Token: 0x02000015 RID: 21
    public class ViewCaptureComponent : GH_Component
    {
        // Token: 0x060000BF RID: 191 RVA: 0x0000681E File Offset: 0x00004A1E
        public ViewCaptureComponent() : base("ViewCapture", "LA_ViewCapture", "This component can save the active viewport as Named View and as an Image file (jpeg/png) in the 'ViewsCaptured' folder.It can provide a list of all saved images path.Right click on this component for more info.\nby Luciano Ambrosini", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x060000C0 RID: 192 RVA: 0x00006840 File Offset: 0x00004A40
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Directory Path", "DirPath", "Assign the path of the folder wherein save all the images generated", 0);
            pManager.AddIntegerParameter("Image Format", "ImgFormat", "Output image format (Default=PNG - png=0,jpeg=1)", 0, 0);
            pManager.AddTextParameter("Image Filename", "ImgName", "Assign a custom filename to the output image", 0);
            pManager.AddNumberParameter("Scale factor", "ScaleF", "Scale factor value as viewport width and height multiplier (Default=1.0)", 0, 1.0);
            pManager.AddBooleanParameter("Run", "Run", "Run this component", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
            pManager[4].MutableNickName = false;
        }

        // Token: 0x060000C1 RID: 193 RVA: 0x0000690C File Offset: 0x00004B0C
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Image Path", "ImgPath", "Latest output image full path", 0);
            pManager.AddTextParameter("Images List", "ImgList", "All images saved in the 'ViewsCapture' folder", (GH_ParamAccess)1);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x060000C2 RID: 194 RVA: 0x00006961 File Offset: 0x00004B61
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000C3 RID: 195 RVA: 0x00006970 File Offset: 0x00004B70
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000C4 RID: 196 RVA: 0x00006998 File Offset: 0x00004B98
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000C5 RID: 197 RVA: 0x000069C4 File Offset: 0x00004BC4
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            if (!DA.GetData<string>(0, ref text))
            {
                return;
            }
            int num = 0;
            if (!DA.GetData<int>(1, ref num))
            {
                return;
            }
            string text2 = "";
            if (!DA.GetData<string>(2, ref text2))
            {
                return;
            }
            double num2 = 1.0;
            if (!DA.GetData<double>(3, ref num2))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(4, ref flag))
            {
                return;
            }
            List<string> list = new List<string>();
            string text3 = "";
            StoreIMG storeIMG = new StoreIMG();
            storeIMG.stored = false;
            int num3 = storeIMG.counter;
            ImageFormat format = ImageFormat.Jpeg;
            List<string> list2 = new List<string>();
            string[] array = new string[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".bmp",
                ".gif"
            };
            string text4 = "ViewsCaptured";
            string text5 = string.Format("{0}{1}", text, text4);
            if (!Directory.Exists(text5))
            {
                Directory.CreateDirectory(text5);
            }
            else
            {
                foreach (string str in array)
                {
                    foreach (string item in Directory.EnumerateFiles(text5, "*" + str, SearchOption.AllDirectories))
                    {
                        list2.Add(item);
                    }
                }
                storeIMG.counter = list2.Count;
            }
            storeIMG.folderpath = text5;
            num3 = storeIMG.counter + 1;
            if (flag)
            {
                RhinoDoc activeDoc = RhinoDoc.ActiveDoc;
                RhinoView activeView = activeDoc.Views.ActiveView;
                string text6 = string.Format("{0}_{1}", text2, num3);
                Guid id = activeView.ActiveViewport.Id;
                activeDoc.NamedViews.Add(text6, id);
                if (base.Params.Input[4].SourceCount == 0)
                {
                    num2 = 1.0;
                }
                double num4 = num2;
                int width = Convert.ToInt32((double)activeView.ActiveViewport.Size.Width * num4);
                int height = Convert.ToInt32((double)activeView.ActiveViewport.Size.Height * num4);
                Bitmap bitmap = activeDoc.Views.Find(id).CaptureToBitmap(new Size(width, height), false, false, false);
                string text7;
                if (num != 0)
                {
                    text7 = "jpg";
                    format = ImageFormat.Jpeg;
                }
                else
                {
                    text7 = "png";
                    format = ImageFormat.Png;
                }
                string filepath = string.Format("{0}{1}\\{2}_x{3}_{4}.{5}", new object[]
                {
                    text,
                    text4,
                    text2,
                    num2,
                    num3,
                    text7
                });
                storeIMG.stored = true;
                storeIMG.filepath = filepath;
                bitmap.Save(storeIMG.filepath, format);
                bitmap.Dispose();
            }
            else
            {
                list2.Clear();
                foreach (string str2 in array)
                {
                    foreach (string item2 in Directory.EnumerateFiles(text5, "*" + str2, SearchOption.AllDirectories))
                    {
                        list2.Add(item2);
                    }
                }
                if (list2.Count == 0)
                {
                    text3 = "Not yet run...";
                    list.Add("No views have yet captured...");
                    return;
                }
                list2.Sort((string x, string y) => File.GetCreationTime(x).CompareTo(File.GetCreationTime(y)));
                List<string> list3 = new List<string>(list2);
                list = list2;
                list3.Reverse();
                text3 = list3[0];
                storeIMG.counter = list2.Count;
            }
            DA.SetData(0, text3);
            DA.SetDataList(1, list);
        }

        // Token: 0x1700005C RID: 92
        // (get) Token: 0x060000C6 RID: 198 RVA: 0x00006D9C File Offset: 0x00004F9C
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x1700005D RID: 93
        // (get) Token: 0x060000C7 RID: 199 RVA: 0x00006D9F File Offset: 0x00004F9F
        protected override Bitmap Icon
        {
            get
            {
                return Resources.viewcapt_icons1_24;
            }
        }

        // Token: 0x1700005E RID: 94
        // (get) Token: 0x060000C8 RID: 200 RVA: 0x00006DA6 File Offset: 0x00004FA6
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D28AD484-AB2E-4BC4-8CB5-A9E3CB0F7B59");
            }
        }

        // Token: 0x04000031 RID: 49
        public bool myMenu_info;
    }
}
