using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.ImageComps
{
    // Token: 0x02000012 RID: 18
    public class ImageConvComponent : GH_Component
    {
        // Token: 0x060000A1 RID: 161 RVA: 0x000055E7 File Offset: 0x000037E7
        public ImageConvComponent() : base("ImgConv", "LA_ImgConv", "Read image file info and can convert image to these format: Bmp, Emf, Exif, Gif, Icon, Jpeg, MemoryBmp, Png, Tiff, Wmf", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x00005608 File Offset: 0x00003808
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("PathIMG", "PathIMG", "Base image path", 0, "");
            pManager.AddIntegerParameter("Ftype", "Ftype", "Image file format - Bmp=0, Emf=1, Exif=2, Gif=3, Icon=4, Jpeg=5, MemoryBmp=6, Png=7, Tiff=8, Wmf=9", 0, 5);
            pManager.AddBooleanParameter("Run", "Run", "Run conversion", 0, false);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x00005688 File Offset: 0x00003888
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("ConvIMG", "ConvIMG", "Converted image path", 0);
            pManager.AddTextParameter("Info", "Info", "Image extra info", 0);
            pManager.AddIntegerParameter("Width", "Width", "Width of the image", 0);
            pManager.AddIntegerParameter("Height", "Height", "Height of the image", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x00005725 File Offset: 0x00003925
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x00005734 File Offset: 0x00003934
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000A6 RID: 166 RVA: 0x0000575C File Offset: 0x0000395C
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000A7 RID: 167 RVA: 0x00005788 File Offset: 0x00003988
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            if (!DA.GetData<string>(0, ref text))
            {
                return;
            }
            int num = 5;
            if (!DA.GetData<int>(1, ref num))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(2, ref flag))
            {
                return;
            }
            if (string.IsNullOrEmpty(text))
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, add a valid file path!");
            }
            string text2 = "";
            Image image = Image.FromFile(text);
            string str = "";
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
            string directoryName = Path.GetDirectoryName(text);
            Bitmap bitmap = new Bitmap(text);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            Bitmap bitmap2 = new Bitmap(bitmap);
            if (num == 0)
            {
                image.RawFormat.Equals(ImageFormat.Bmp);
                str = ".bmp";
            }
            else if (num == 1)
            {
                image.RawFormat.Equals(ImageFormat.Emf);
                str = ".emf";
            }
            else if (num == 2)
            {
                image.RawFormat.Equals(ImageFormat.Exif);
                str = ".exif";
            }
            else if (num == 3)
            {
                image.RawFormat.Equals(ImageFormat.Gif);
                str = ".gif";
            }
            else if (num == 4)
            {
                image.RawFormat.Equals(ImageFormat.Icon);
                str = ".icon";
            }
            else if (num == 5)
            {
                image.RawFormat.Equals(ImageFormat.Jpeg);
                str = ".jpg";
            }
            else if (num == 6)
            {
                image.RawFormat.Equals(ImageFormat.MemoryBmp);
                str = ".memorybmp";
            }
            else if (num == 7)
            {
                image.RawFormat.Equals(ImageFormat.Png);
                str = ".png";
            }
            else if (num == 8)
            {
                image.RawFormat.Equals(ImageFormat.Tiff);
                str = ".tiff";
            }
            else if (num == 9)
            {
                image.RawFormat.Equals(ImageFormat.Wmf);
                str = ".wmf";
            }
            if (flag)
            {
                string text3 = directoryName + "\\ConvIMG";
                string text4 = text3 + "\\" + fileNameWithoutExtension + str;
                Directory.CreateDirectory(text3);
                image.Save(text4);
                text2 = text4;
                DA.SetData(0, text2);
            }
            DA.SetData(0, text2);
            string extension = Path.GetExtension(text);
            string text5 = Convert.ToString(bitmap2.Size) + "\nImageFormat\\" + extension.ToUpper();
            DA.SetData(1, text5);
            int width = bitmap2.Width;
            DA.SetData(2, width);
            int height = bitmap2.Height;
            DA.SetData(3, height);
        }

        // Token: 0x17000053 RID: 83
        // (get) Token: 0x060000A8 RID: 168 RVA: 0x00005A05 File Offset: 0x00003C05
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x060000A9 RID: 169 RVA: 0x00005A08 File Offset: 0x00003C08
        protected override Bitmap Icon
        {
            get
            {
                return Resources.ImageConv_icon2_24;
            }
        }

        // Token: 0x17000055 RID: 85
        // (get) Token: 0x060000AA RID: 170 RVA: 0x00005A0F File Offset: 0x00003C0F
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("02BE57FD-8CF1-483E-A009-678961E0C609");
            }
        }

        // Token: 0x0400002E RID: 46
        public bool myMenu_info;
    }
}
