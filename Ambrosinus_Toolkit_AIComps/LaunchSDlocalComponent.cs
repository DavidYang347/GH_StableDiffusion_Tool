using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Functions;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace Ambrosinus_Toolkit.AIComps
{
    // Token: 0x02000021 RID: 33
    public class LaunchSDlocalComponent : GH_Component
    {
        // Token: 0x06000142 RID: 322 RVA: 0x0000AF5C File Offset: 0x0000915C
        public LaunchSDlocalComponent() : base("LaunchSD_loc", "LA_LaunchSD_loc", "This component launches the Webui process from the AUTOMATIC1111 projectin order to run Stable Diffusion locally on your machine. It requires a previous installation of the SD. Right click on this component for more info.\nby Luciano Ambrosini", "Ambrosinus", "3.AI")
        {
            this.m_values = new List<GH_ValueListItem>
            {
                new GH_ValueListItem("API", "1"),
                new GH_ValueListItem("lowvram", "2"),
                new GH_ValueListItem("xformers", "3"),
                new GH_ValueListItem("autolaunch", "4"),
                new GH_ValueListItem("theme dark", "5"),
                new GH_ValueListItem("share", "6"),
                new GH_ValueListItem("listen", "7"),
                new GH_ValueListItem("reinstall xformers", "8"),
                new GH_ValueListItem("reinstall torch", "9"),
                new GH_ValueListItem("skip vers check", "10"),
                new GH_ValueListItem("precision full", "11"),
                new GH_ValueListItem("no half", "12"),
                new GH_ValueListItem("no half vae", "13"),
                new GH_ValueListItem("disable NaN check", "14"),
                new GH_ValueListItem("upcast sampling", "15")
            };
        }

        // Token: 0x06000143 RID: 323 RVA: 0x0000B0D0 File Offset: 0x000092D0
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Directory Path", "DirPath", "Assign the path of the folder wherein SD project is installed", 0);
            pManager.AddIntegerParameter("Cmd Arguments", "CMDargs", "List of arguments to use for launching the process", (GH_ParamAccess)1, 0);
            pManager.AddIntegerParameter("IP port", "IPport", "Set a custom IP adress port value after ticked the 'listen' argument (Default is: 7860) ", 0, 7860);
            pManager.AddBooleanParameter("Gen Webui", "GenWebUI", "Generate the 'webui-user.bat' file according to the cmd arguments", 0);
            pManager.AddBooleanParameter("Start Process", "Start", "Start the Webui process", 0);
            pManager.AddBooleanParameter("Close Process", "Close", "Close the Webui process", 0);
            pManager.AddBooleanParameter("Check Process", "Check", "Check status", 0);
            pManager.AddBooleanParameter("Check IPconfig", "CheckIP", "Check the IP value at 'IPv4' string", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
            pManager[4].MutableNickName = false;
            pManager[5].MutableNickName = false;
            pManager[6].MutableNickName = false;
            pManager[7].MutableNickName = false;
        }

        // Token: 0x06000144 RID: 324 RVA: 0x0000B203 File Offset: 0x00009403
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Read status", "Read", "Shows localhost port check status", 0);
            pManager[0].MutableNickName = false;
        }

        // Token: 0x06000145 RID: 325 RVA: 0x0000B229 File Offset: 0x00009429
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000146 RID: 326 RVA: 0x0000B238 File Offset: 0x00009438
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Project Info", new EventHandler(this.Menu_A_DoClick), Resources.resprj_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "About SD locally", new EventHandler(this.Menu_B_DoClick), Resources.comps_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Requirements", new EventHandler(this.Menu_C_DoClick), Resources.requirem_logo1_24);
        }

        // Token: 0x06000147 RID: 327 RVA: 0x0000B2C0 File Offset: 0x000094C0
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000148 RID: 328 RVA: 0x0000B2EC File Offset: 0x000094EC
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/atoolkit-research-project/");
            }
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000B318 File Offset: 0x00009518
        private void Menu_B_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_B = !this.myMenu_B)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally");
            }
        }

        // Token: 0x0600014A RID: 330 RVA: 0x0000B344 File Offset: 0x00009544
        private void Menu_C_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_C = !this.myMenu_C)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally#part1");
            }
        }

        // Token: 0x0600014B RID: 331 RVA: 0x0000B370 File Offset: 0x00009570
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string text = "";
            if (!DA.GetData<string>(0, ref text))
            {
                return;
            }
            List<int> list = new List<int>();
            if (!DA.GetDataList<int>(1, list))
            {
                return;
            }
            int num = 7860;
            if (base.Params.Input[2].SourceCount == 0)
            {
                num = 7860;
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "If you have added a custom IP port number, please be sure to have ticked the 'listen' argument!");
                DA.GetData<int>(2, ref num);
            }
            bool flag = false;
            if (!DA.GetData<bool>(3, ref flag))
            {
                return;
            }
            bool flag2 = false;
            if (!DA.GetData<bool>(4, ref flag2))
            {
                return;
            }
            bool flag3 = false;
            if (!DA.GetData<bool>(5, ref flag3))
            {
                return;
            }
            bool flag4 = false;
            if (!DA.GetData<bool>(6, ref flag4))
            {
                return;
            }
            bool flag5 = false;
            if (!DA.GetData<bool>(7, ref flag5))
            {
                return;
            }
            int num2 = num;
            ListenIPport.CustPort = Convert.ToString(num);
            bool flag6 = CustomFX.IsBusy(num2);
            if (flag2)
            {
                CustomFX.RunProcessWebUI(text);
            }
            if (flag3)
            {
                foreach (Process process in Process.GetProcessesByName("cmd"))
                {
                    process.CloseMainWindow();
                    process.Kill();
                    process.Close();
                }
            }
            string arg;
            if (flag6)
            {
                arg = "available";
            }
            else
            {
                arg = "not available";
            }
            if (flag4)
            {
                if (flag6)
                {
                    arg = "available";
                }
                else
                {
                    arg = "not available";
                }
            }
            string text2 = string.Format("Check completed!{0}Port {1} is {2}", Environment.NewLine, num2, arg);
            if (flag5)
            {
                CustomFX.RunCMDipconfig();
            }
            string text3 = text;
            string text4 = "webui-user.bat";
            string path = Path.Combine(text3, text4);
            string str = "";
            List<string> list2 = new List<string>();
            if (flag)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] == 1)
                    {
                        list2.Add("--api");
                    }
                    else if (list[j] == 2)
                    {
                        list2.Add("--lowvram");
                    }
                    else if (list[j] == 3)
                    {
                        list2.Add("--xformers");
                    }
                    else if (list[j] == 4)
                    {
                        list2.Add("--autolaunch");
                    }
                    else if (list[j] == 5)
                    {
                        list2.Add("--theme dark");
                    }
                    else if (list[j] == 6)
                    {
                        list2.Add("--share");
                    }
                    else if (list[j] == 7)
                    {
                        string str2 = string.Format("--port {0}", ListenIPport.CustPort);
                        list2.Add("--listen " + str2);
                    }
                    else if (list[j] == 8)
                    {
                        list2.Add("--reinstall-xformers");
                    }
                    else if (list[j] == 9)
                    {
                        list2.Add("--reinstall-torch");
                    }
                    else if (list[j] == 10)
                    {
                        list2.Add("--skip-version-check");
                    }
                    else if (list[j] == 11)
                    {
                        list2.Add("--precision full");
                    }
                    else if (list[j] == 12)
                    {
                        list2.Add("--no-half");
                    }
                    else if (list[j] == 13)
                    {
                        list2.Add("--no-half-vae");
                    }
                    else if (list[j] == 14)
                    {
                        list2.Add("--disable-nan-check");
                    }
                    else if (list[j] == 15)
                    {
                        list2.Add("--upcast-sampling");
                    }
                    string arg2 = string.Join(" ", list2);
                    str = string.Format("set COMMANDLINE_ARGS={0}\n\n", arg2);
                }
                string contents = "@echo off\n\nset PYTHON=\nset GIT=\nset VENV_DIR=\n" + str + "git pull\ncall webui.bat";
                File.WriteAllText(path, contents);
                MessageBox.Show("The file " + text4 + " has been set up in " + text3, "Webui-user file set up correctly", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, string.Format("Set 'API' always-on to run these components!", Array.Empty<object>()));
            }
            string text5 = text2;
            DA.SetData(0, text5);
        }

        // Token: 0x0600014C RID: 332 RVA: 0x0000B75C File Offset: 0x0000995C
        public override bool Write(GH_IWriter writer)
        {
            writer.SetInt32("ValueCount", this.m_values.Count);
            for (int i = 0; i < this.m_values.Count; i++)
            {
                writer.SetString("Name" + i.ToString(), this.m_values[i].Name);
                writer.SetString("Expression" + i.ToString(), this.m_values[i].Expression);
            }
            return base.Write(writer);
        }

        // Token: 0x0600014D RID: 333 RVA: 0x0000B7EC File Offset: 0x000099EC
        public override bool Read(GH_IReader reader)
        {
            this.m_values.Clear();
            int @int = reader.GetInt32("ValueCount");
            for (int i = 0; i < @int; i++)
            {
                string @string = reader.GetString("Name" + i.ToString());
                string string2 = reader.GetString("Expression" + i.ToString());
                this.m_values.Add(new GH_ValueListItem(@string, string2));
            }
            return base.Read(reader);
        }

        // Token: 0x0600014E RID: 334 RVA: 0x0000B868 File Offset: 0x00009A68
        public override void AddedToDocument(GH_Document document)
        {
            base.AddedToDocument(document);
            if (base.Params.Input[1].SourceCount > 0)
            {
                return;
            }
            base.Attributes.PerformLayout();
            int num = (int)base.Params.Input[0].Attributes.Pivot.X - 250;
            int num2 = (int)base.Params.Input[0].Attributes.Pivot.Y - 10;
            GH_ValueList gh_ValueList = new GH_ValueList();
            gh_ValueList.CreateAttributes();
            gh_ValueList.Attributes.Pivot = new PointF((float)num, (float)num2);
            gh_ValueList.Attributes.ExpireLayout();
            gh_ValueList.ListMode = 0;
            gh_ValueList.ListItems.Clear();
            gh_ValueList.ListItems.AddRange(this.m_values);
            gh_ValueList.Name = "CMD Arguments";
            gh_ValueList.NickName = "CMDargs";
            gh_ValueList.Description = "CMD arguments list";
            document.AddObject(gh_ValueList, false, int.MaxValue);
            base.Params.Input[1].AddSource(gh_ValueList);
        }

        // Token: 0x1700007D RID: 125
        // (get) Token: 0x0600014F RID: 335 RVA: 0x0000B988 File Offset: 0x00009B88
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)8;
            }
        }

        // Token: 0x1700007E RID: 126
        // (get) Token: 0x06000150 RID: 336 RVA: 0x0000B98B File Offset: 0x00009B8B
        protected override Bitmap Icon
        {
            get
            {
                return Resources.portCmd_icon1_24;
            }
        }

        // Token: 0x1700007F RID: 127
        // (get) Token: 0x06000151 RID: 337 RVA: 0x0000B992 File Offset: 0x00009B92
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("422E7965-A51C-4C2D-B978-02DCCC0F3620");
            }
        }

        // Token: 0x04000049 RID: 73
        private List<GH_ValueListItem> m_values;

        // Token: 0x0400004A RID: 74
        public bool myMenu_info;

        // Token: 0x0400004B RID: 75
        public bool myMenu_A;

        // Token: 0x0400004C RID: 76
        public bool myMenu_B;

        // Token: 0x0400004D RID: 77
        public bool myMenu_C;
    }
}
