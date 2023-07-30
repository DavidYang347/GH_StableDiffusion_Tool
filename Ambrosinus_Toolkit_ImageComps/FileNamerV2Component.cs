using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.ImageComps
{
    // Token: 0x02000011 RID: 17
    public class FileNamerV2Component : GH_Component
    {
        // Token: 0x06000097 RID: 151 RVA: 0x0000518F File Offset: 0x0000338F
        public FileNamerV2Component() : base("FileNamer", "LA_FileNAmer", "FileNamer can find and increment filename in a folder. This is helpful for MaskIMG and GrayGaussMask components\nby Luciano Ambrosini", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x06000098 RID: 152 RVA: 0x000051B0 File Offset: 0x000033B0
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("DirPath", "DirPath", "Folder path address", 0);
            pManager.AddTextParameter("MaskName", "MaskName", "Custom filename", 0);
            pManager.AddBooleanParameter("Run", "Run", "Run component", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        // Token: 0x06000099 RID: 153 RVA: 0x0000522C File Offset: 0x0000342C
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("MaskPath", "MaskPath", "Add a 'IMGs' folder to DirPath", 0);
            pManager.AddTextParameter("Filename", "Filename", "Filename to avoid overwriting action", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x0600009A RID: 154 RVA: 0x00005281 File Offset: 0x00003481
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x0600009B RID: 155 RVA: 0x00005290 File Offset: 0x00003490
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x0600009C RID: 156 RVA: 0x000052B8 File Offset: 0x000034B8
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x0600009D RID: 157 RVA: 0x000052E4 File Offset: 0x000034E4
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            if (!DA.GetData<string>(0, ref text))
            {
                return;
            }
            string text2 = "";
            if (!DA.GetData<string>(1, ref text2))
            {
                return;
            }
            bool flag = false;
            if (!DA.GetData<bool>(2, ref flag))
            {
                return;
            }
            string text3 = text + "\\IMGs";
            string text4 = "MyMask";
            List<string> list = new List<string>();
            List<int> list2 = new List<int>();
            string arg = "";
            string text5 = "";
            bool flag2 = !flag;
            if (text != null && Directory.Exists(text3))
            {
                if (flag2)
                {
                    foreach (string item in Directory.GetFiles(text3, "*.png", SearchOption.AllDirectories))
                    {
                        list.Add(item);
                    }
                    if (list.Count<string>() != 0)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(list[j]);
                            char c = '_';
                            string a = fileNameWithoutExtension.Split(new char[]
                            {
                                c
                            }, StringSplitOptions.RemoveEmptyEntries)[0];
                            if (text2 != null)
                            {
                                arg = text2;
                                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Custom name has been assigned!");
                                if (a == text2)
                                {
                                    list2.Add(1);
                                }
                                else
                                {
                                    list2.Add(0);
                                }
                            }
                            else
                            {
                                arg = text4;
                                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "No custom name given\nso a default one has been assigned!");
                                if (a == text4)
                                {
                                    list2.Add(1);
                                }
                                else
                                {
                                    this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "No duplicates found!");
                                    list2.Add(0);
                                }
                            }
                        }
                        int num = list2.Sum() + 1;
                        if (num == 1)
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Filename is unique!");
                            text5 = string.Format("{0}", arg);
                        }
                        else if (num < 10)
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Filename will be incremented!");
                            text5 = string.Format("{0}_0{1}", arg, num);
                        }
                        else if (num > 9)
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Filename will be incremented!");
                            text5 = string.Format("{0}_{1}", arg, num);
                        }
                        else
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "This folder is empty for now");
                            arg = text2;
                            text5 = string.Format("{0}", arg);
                        }
                    }
                    else
                    {
                        this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "This folder is empty for now and Filename is unique!");
                        arg = text2;
                        text5 = string.Format("{0}", arg);
                    }
                }
            }
            else if (text != null && !Directory.Exists(text3))
            {
                if (flag2)
                {
                    if (text2 != null)
                    {
                        arg = text2;
                    }
                    else
                    {
                        arg = text4;
                    }
                    Directory.CreateDirectory(text3);
                    text5 = string.Format("{0}", arg);
                    this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Folder has been created and Filename is unique!");
                }
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, add a custom directory path address!");
                text5 = string.Format("{0}", arg);
            }
            string text6 = text5;
            DA.SetData(0, text3);
            DA.SetData(1, text6);
            base.Message = string.Format("Fn: {0}", text5);
        }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x0600009E RID: 158 RVA: 0x000055D1 File Offset: 0x000037D1
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)2;
            }
        }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x0600009F RID: 159 RVA: 0x000055D4 File Offset: 0x000037D4
        protected override Bitmap Icon
        {
            get
            {
                return Resources.Fnamer_icon1_24;
            }
        }

        // Token: 0x17000052 RID: 82
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x000055DB File Offset: 0x000037DB
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("36c91ab2-cdb3-4fd0-bef9-1f8cca9b4fb3");
            }
        }

        // Token: 0x0400002D RID: 45
        public bool myMenu_info;
    }
}
