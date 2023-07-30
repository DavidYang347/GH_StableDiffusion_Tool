using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.Obsolete
{
    // Token: 0x0200000D RID: 13
    public class FileNamerComponent_OBSOLETE : GH_Component
    {
        // Token: 0x0600006F RID: 111 RVA: 0x000047CA File Offset: 0x000029CA
        public FileNamerComponent_OBSOLETE() : base("FileNamer", "LA_FileNAmer", "FileNamer can find and increment filename in a folder. This is helpful for MaskIMG and GrayGaussMask components\nby Luciano Ambrosini", "Ambrosinus", "2.Image")
        {
        }

        // Token: 0x06000070 RID: 112 RVA: 0x000047EC File Offset: 0x000029EC
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("DirPath", "DirPath", "Folder path address", 0);
            pManager.AddTextParameter("MaskName", "MaskName", "Custom filename", 0);
            pManager.AddBooleanParameter("Run", "Run", "Run component", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
        }

        // Token: 0x06000071 RID: 113 RVA: 0x00004868 File Offset: 0x00002A68
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("MaskPath", "MaskPath", "Add a 'IMGs' folder to DirPath", 0);
            pManager.AddTextParameter("Filename", "Filename", "Filename to avoid overwriting action", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x06000072 RID: 114 RVA: 0x000048BD File Offset: 0x00002ABD
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000073 RID: 115 RVA: 0x000048CC File Offset: 0x00002ACC
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
        }

        // Token: 0x06000074 RID: 116 RVA: 0x000048F4 File Offset: 0x00002AF4
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000075 RID: 117 RVA: 0x00004920 File Offset: 0x00002B20
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
            string text3 = "MyMask";
            List<string> list = new List<string>();
            List<int> list2 = new List<int>();
            bool flag2 = false;
            string arg = "";
            string text4 = "";
            bool flag3 = !flag;
            if (text != null && Directory.Exists(text) && !flag2)
            {
                if (flag3)
                {
                    foreach (string item in Directory.GetFiles(text, "*.png", SearchOption.AllDirectories))
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
                                arg = text3;
                                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "No custom name given\nso a default one has been assigned!");
                                if (a == text3)
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
                            text4 = string.Format("{0}", arg);
                        }
                        else if (num < 10)
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Filename will be incremented!");
                            text4 = string.Format("{0}_0{1}", arg, num);
                        }
                        else if (num > 9)
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Filename will be incremented!");
                            text4 = string.Format("{0}_{1}", arg, num);
                        }
                        else
                        {
                            this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "This folder is empty for now");
                            arg = text2;
                            text4 = string.Format("{0}", arg);
                        }
                    }
                }
            }
            else if (text != null && !Directory.Exists(text) && !flag2)
            {
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                    this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Folder has been created");
                }
            }
            else if (text != null && Directory.Exists(text) && flag2)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "This folder is empty for now");
                arg = text2;
                text4 = string.Format("{0}", arg);
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)10, "Please, add a custom directory path address!");
            }
            string text5 = text + "\\IMGs";
            string text6 = text4;
            DA.SetData(0, text5);
            DA.SetData(1, text6);
            base.Message = string.Format("Fn: {0}", text4);
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x06000076 RID: 118 RVA: 0x00004C05 File Offset: 0x00002E05
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)(-1);
            }
        }

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x06000077 RID: 119 RVA: 0x00004C08 File Offset: 0x00002E08
        protected override Bitmap Icon
        {
            get
            {
                return Resources.Fnamer_icon1_24;
            }
        }

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x06000078 RID: 120 RVA: 0x00004C0F File Offset: 0x00002E0F
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("188F12C5-9360-4224-A592-24831DDC3941");
            }
        }

        // Token: 0x04000029 RID: 41
        public bool myMenu_info;
    }
}
