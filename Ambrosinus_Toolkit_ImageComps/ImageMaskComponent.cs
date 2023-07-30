using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Functions;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ambrosinus_Toolkit.ImageComps
{
    // Token: 0x02000013 RID: 19
    public class ImageMaskComponent : GH_Component
    {
        // Token: 0x060000AB RID: 171 RVA: 0x00005A1B File Offset: 0x00003C1B
        public ImageMaskComponent() : base("ImgMask", "LA_ImgMask", "Generate PNG and JPG image mask simply drawing it inside Rhino over the BaseIMG", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x060000AC RID: 172 RVA: 0x00005A3C File Offset: 0x00003C3C
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("BaseIMG", "BaseIMG", "Base image path", 0, "");
            pManager.AddCurveParameter("Frame", "Frame", "Rectangular polyline that represents the frame of the BaseIMG", (GH_ParamAccess)1);
            pManager.AddCurveParameter("Curves", "Curves", "Curve polyline that represents main selection outline (List)", (GH_ParamAccess)1);
            pManager.AddCurveParameter("Holes", "Holes", "Curve polyline that represents 'holes' inside the main selection outline (also as List)\nIMPORTANT! If you don't have any 'Holes' please,\npass in both 'Curves' and 'Holes' your Curves selection in order to run this component", (GH_ParamAccess)1);
            pManager.AddIntegerParameter("Width", "Width", "Width of the image mask, if you give a BaseIMG as input this value has to have the same width value", 0);
            pManager.AddIntegerParameter("Height", "Height", "Height of the image mask, if you give a BaseIMG as input this value has to have the same height value", 0);
            pManager.AddTextParameter("MaskPath", "MaskPath", "Image mask output path", 0);
            pManager.AddTextParameter("MaskName", "MaskName", "Custom name of the Image mask", 0);
            pManager.AddColourParameter("MaskColor", "MaskColor", "Custom mask color (Default white)", 0, Color.FromArgb(255, 255, 255, 255));
            pManager.AddColourParameter("BackColor", "BackColor", "Custom back color (Default black)", 0, Color.FromArgb(255, 0, 0, 0));
            pManager.AddIntegerParameter("Mode", "Mode", "Mode=0 generates a PNG format image with a transparent mask and the BaseIMG as background; Any trasparent colors as input will generate a PNG format image; Mode=1 generates a JPG format image with mask and the bagkround with custom colors (Default mask: white/back: black)", 0);
            pManager.AddBooleanParameter("Run", "Run", "Run ImageMask component", 0, false);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
            pManager[4].MutableNickName = false;
            pManager[5].MutableNickName = false;
            pManager[6] .MutableNickName = false;
            pManager[7].MutableNickName = false;
            pManager[8].MutableNickName = false;
            pManager[9].MutableNickName = false;
            pManager[10].MutableNickName = false;
            pManager[11].MutableNickName = false;
        }

        // Token: 0x060000AD RID: 173 RVA: 0x00005C28 File Offset: 0x00003E28
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x060000AE RID: 174 RVA: 0x00005C37 File Offset: 0x00003E37
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("MaskIMG", "MaskIMG", "Full path of the image mask generated", 0);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x060000AF RID: 175 RVA: 0x00005C5D File Offset: 0x00003E5D
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x060000B0 RID: 176 RVA: 0x00005C84 File Offset: 0x00003E84
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x00005CB0 File Offset: 0x00003EB0
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filename = "";
            if (!DA.GetData<string>(0, ref filename))
            {
                return;
            }
            List<Curve> list = new List<Curve>();
            if (!DA.GetDataList<Curve>(1, list))
            {
                return;
            }
            List<Curve> list2 = new List<Curve>();
            if (!DA.GetDataList<Curve>(2, list2))
            {
                return;
            }
            List<Curve> list3 = new List<Curve>();
            if (!DA.GetDataList<Curve>(3, list3))
            {
                return;
            }
            int width = 0;
            if (!DA.GetData<int>(4, ref width))
            {
                return;
            }
            int height = 0;
            if (!DA.GetData<int>(5, ref height))
            {
                return;
            }
            string str = "";
            if (!DA.GetData<string>(6, ref str))
            {
                return;
            }
            string str2 = "myMask";
            if (!DA.GetData<string>(7, ref str2))
            {
                return;
            }
            Color color = default(Color);
            if (!DA.GetData<Color>(8, ref color))
            {
                return;
            }
            Color color2 = default(Color);
            if (!DA.GetData<Color>(9, ref color2))
            {
                return;
            }
            int num = 0;
            if (!DA.GetData<int>(10, ref num))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(11, ref flag))
            {
                return;
            }
            int num2 = 500;
            double num3 = 0.001;
            //new Point3d[num2];
            bool flag2 = false;
            List<Region> list4 = new List<Region>();
            List<Region> list5 = new List<Region>();
            Region region = new Region();
            Region region2 = new Region();
            List<Polyline> list6 = new List<Polyline>();
            List<Polyline> list7 = new List<Polyline>();
            List<Polyline> list8 = new List<Polyline>();
            Point3d[] array = new Point3d[num2];
            for (int i = 0; i < list.Count; i++)
            {
                list[i].DivideByCount(num2, true, out array);
                list6.Add(new Polyline(array));
            }
            for (int j = 0; j < list2.Count; j++)
            {
                list2[j].DivideByCount(num2, true, out array);
                list7.Add(new Polyline(array));
            }
            for (int k = 0; k < list3.Count; k++)
            {
                list3[k].DivideByCount(num2, true, out array);
                list8.Add(new Polyline(array));
            }
            new SolidBrush(Color.White);
            new SolidBrush(Color.Black);
            SolidBrush brush = new SolidBrush(Color.Transparent);
            bool flag3;
            if (color.IsEmpty)
            {
                flag3 = true;
                color = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                flag3 = false;
            }
            bool flag4;
            if (color2.IsEmpty)
            {
                flag4 = true;
                color2 = Color.FromArgb(255, 0, 0, 0);
            }
            else
            {
                flag4 = false;
            }
            string str3;
            if (num == 0 || color.A == 0 || (color2.A == 0 && flag3) || flag4)
            {
                str3 = ".png";
            }
            else
            {
                str3 = ".jpg";
            }
            string text = str + "\\" + str2 + str3;
            if (flag)
            {
                if (num == 0)
                {
                    Bitmap bitmap = new Bitmap(filename);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                    Bitmap bitmap2 = new Bitmap(bitmap);
                    Graphics graphics = Graphics.FromImage(bitmap2);
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    GraphicsPath graphicsPath = new GraphicsPath();
                    PointF[] points = new PointF[list.Count];
                    foreach (Polyline curve in list6)
                    {
                        points = CustomFX.PolylineToPoints(curve);
                    }
                    graphicsPath.AddPolygon(points);
                    Region region3 = new Region(graphicsPath);
                    if (list3.Count != 0)
                    {
                        GraphicsPath graphicsPath2 = new GraphicsPath();
                        PointF[] points2 = new PointF[list3.Count];
                        foreach (Polyline curve2 in list8)
                        {
                            points2 = CustomFX.PolylineToPoints(curve2);
                            graphicsPath2.AddPolygon(points2);
                            region = new Region(graphicsPath2);
                            list4.Add(region);
                        }
                    }
                    PointF[] points3 = new PointF[list2.Count];
                    foreach (Polyline curve3 in list7)
                    {
                        points3 = CustomFX.PolylineToPoints(curve3);
                        GraphicsPath graphicsPath3 = new GraphicsPath();
                        graphicsPath3.AddPolygon(points3);
                        region2 = new Region(graphicsPath3);
                        list5.Add(region2);
                        graphics.FillRegion(brush, region2);
                    }
                    for (int l = 0; l < list2.Count; l++)
                    {
                        for (int m = 0; m < list3.Count; m++)
                        {
                            Point3d[] array2 = list8[m].ToArray();
                            for (int n = 0; n < array2.Length; n++)
                            {
                                flag2 = ((int)list2[l].Contains(array2[n], Plane.WorldXY, num3) == 1);
                            }
                            if (list3.Count != 0 && flag2)
                            {
                                list5[l].Exclude(list4[m]);
                            }
                        }
                    }
                    for (int num4 = 0; num4 < list2.Count; num4++)
                    {
                        region3.Exclude(list5[num4]);
                        graphics.SetClip(list5[num4], CombineMode.Replace);
                        graphics.Clear(Color.Transparent);
                        graphics.ResetClip();
                    }
                    region.Dispose();
                    region2.Dispose();
                    region3.Dispose();
                    bitmap2.RotateFlip(RotateFlipType.Rotate180FlipX);
                    bitmap2.MakeTransparent(Color.Transparent);
                    bitmap2.Save(text, ImageFormat.Png);
                    graphics.Dispose();
                    bitmap2.Dispose();
                }
                else if (num == 1)
                {
                    Bitmap bitmap2 = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    Graphics graphics2 = Graphics.FromImage(bitmap2);
                    graphics2.Clear(color2);
                    graphics2.SmoothingMode = SmoothingMode.HighQuality;
                    GraphicsPath graphicsPath4 = new GraphicsPath();
                    PointF[] points4 = new PointF[list.Count];
                    foreach (Polyline curve4 in list6)
                    {
                        points4 = CustomFX.PolylineToPoints(curve4);
                    }
                    graphicsPath4.AddPolygon(points4);
                    Region region4 = new Region(graphicsPath4);
                    if (list3.Count != 0)
                    {
                        GraphicsPath graphicsPath5 = new GraphicsPath();
                        PointF[] points5 = new PointF[list3.Count];
                        foreach (Polyline curve5 in list8)
                        {
                            points5 = CustomFX.PolylineToPoints(curve5);
                            graphicsPath5.AddPolygon(points5);
                            region = new Region(graphicsPath5);
                            list4.Add(region);
                        }
                    }
                    PointF[] points6 = new PointF[list2.Count];
                    foreach (Polyline curve6 in list7)
                    {
                        points6 = CustomFX.PolylineToPoints(curve6);
                        GraphicsPath graphicsPath6 = new GraphicsPath();
                        graphicsPath6.AddPolygon(points6);
                        region2 = new Region(graphicsPath6);
                        list5.Add(region2);
                        graphics2.FillRegion(brush, region2);
                    }
                    for (int num5 = 0; num5 < list2.Count; num5++)
                    {
                        for (int num6 = 0; num6 < list3.Count; num6++)
                        {
                            Polyline polyline = list8[num6];
                            Point3d[] array3 = polyline.ToArray();
                            for (int num7 = 0; num7 < polyline.Count; num7++)
                            {
                                array3[num7] = polyline[num7];
                            }
                            for (int num8 = 0; num8 < array3.Length; num8++)
                            {
                                flag2 = ((int)list2[num5].Contains(array3[num8], Plane.WorldXY, num3) == 1);
                            }
                            if (list3.Count != 0 && flag2)
                            {
                                list5[num5].Exclude(list4[num6]);
                            }
                        }
                    }
                    for (int num9 = 0; num9 < list2.Count; num9++)
                    {
                        region4.Exclude(list5[num9]);
                        graphics2.SetClip(list5[num9], CombineMode.Replace);
                        graphics2.Clear(color);
                        graphics2.ResetClip();
                    }
                    region.Dispose();
                    region2.Dispose();
                    region4.Dispose();
                    bitmap2.RotateFlip(RotateFlipType.Rotate180FlipX);
                    if (color.A != 0 && color2.A != 0)
                    {
                        bitmap2.Save(text, ImageFormat.Jpeg);
                    }
                    else
                    {
                        bitmap2.MakeTransparent(Color.Transparent);
                        bitmap2.Save(text, ImageFormat.Png);
                    }
                    graphics2.Dispose();
                    bitmap2.Dispose();
                }
            }
            string text2 = text;
            DA.SetData(0, text2);
        }

        // Token: 0x17000056 RID: 86
        // (get) Token: 0x060000B2 RID: 178 RVA: 0x0000650C File Offset: 0x0000470C
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000057 RID: 87
        // (get) Token: 0x060000B3 RID: 179 RVA: 0x0000650F File Offset: 0x0000470F
        protected override Bitmap Icon
        {
            get
            {
                return Resources.ImageMask_icon2_24;
            }
        }

        // Token: 0x17000058 RID: 88
        // (get) Token: 0x060000B4 RID: 180 RVA: 0x00006516 File Offset: 0x00004716
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("D3677238-BE80-4F5D-9C72-CAC2A36493E7");
            }
        }

        // Token: 0x0400002F RID: 47
        public bool myMenu_info;
    }
}
