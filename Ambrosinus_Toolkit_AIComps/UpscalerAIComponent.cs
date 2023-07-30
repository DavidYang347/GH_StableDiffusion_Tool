using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Functions;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using Grasshopper.Kernel;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Ambrosinus_Toolkit.AIComps
{
    // Token: 0x02000025 RID: 37
    public class UpscalerAIComponent : GH_Component
    {
        // Token: 0x06000177 RID: 375 RVA: 0x0000D35A File Offset: 0x0000B55A
        public UpscalerAIComponent() : base("UpsclAI_loc", "LA_UpsclAI_loc", "This component can upscale images", "Ambrosinus", "3.AI")
        {
        }


        public static class SiteContainer
        {
            public static CallSite<Func<CallSite, object, object>> p__0;
            public static CallSite<Func<CallSite, object, string>> p__1;


        }

        // Token: 0x06000178 RID: 376 RVA: 0x0000D37C File Offset: 0x0000B57C
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Mode", "Mode", "Select the Upscaling resizer mode (Resize multiplier=0, Custom Size=1", 0, 0);
            pManager.AddIntegerParameter("Timeout custom", "Timeout", "If you want increase the WebRequest timeout (default= 10 =>10mins) add an int custom value as mins", 0, 10);
            pManager.AddBooleanParameter("Check SDupscalers", "ChkUpscls", "Check all loaded Upscaler models", 0);
            pManager.AddIntegerParameter("ID Uspcales 1", "idUpscl1", "Assign an integer value related to the Upscalers 1 list as shown in the 'UpsclsList' output", 0, 0);
            pManager.AddIntegerParameter("ID Uspcales 2", "idUpscl2", "Assign an integer value related to the Upscalers 2 list as shown in the 'UpsclsList' output", 0, 0);
            pManager.AddTextParameter("Directory Path", "DirPath", "Assign the path of the folder wherein save all the images upscaled", 0);
            pManager.AddTextParameter("BaseImage Path", "BaseIMG", "Source image path to upscale", 0);
            pManager.AddNumberParameter("Resize", "Res", "It is a multiplier value to resize image works with Mode=0 (Default=2)", 0, 2.0);
            pManager.AddIntegerParameter("Width", "W", "Final image width", 0, 512);
            pManager.AddIntegerParameter("Height", "H", "Final image height", 0, 512);
            pManager.AddBooleanParameter("Start Process", "Start", "Set the Upscaler Model corresponding to the selected 'idUpscl'", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
            pManager[2].MutableNickName = false;
            pManager[3].MutableNickName = false;
            pManager[4].MutableNickName = false;
            pManager[5].MutableNickName = false;
            pManager[6].MutableNickName = false;
            pManager[7].MutableNickName = false;
            pManager[8].MutableNickName = false;
            pManager[9].MutableNickName = false;
        }

        // Token: 0x06000179 RID: 377 RVA: 0x0000D524 File Offset: 0x0000B724
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Upscalers list", "UpsclsList", "Shows all your Upscaler models", (GH_ParamAccess)1);
            pManager.AddTextParameter("imageUps Path", "ImgUps", "Full path of the upscaled image", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x0600017A RID: 378 RVA: 0x0000D579 File Offset: 0x0000B779
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x0600017B RID: 379 RVA: 0x0000D588 File Offset: 0x0000B788
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Project Info", new EventHandler(this.Menu_A_DoClick), Resources.resprj_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "About SD locally", new EventHandler(this.Menu_B_DoClick), Resources.comps_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Requirements", new EventHandler(this.Menu_C_DoClick), Resources.requirem_logo1_24);
        }

        // Token: 0x0600017C RID: 380 RVA: 0x0000D610 File Offset: 0x0000B810
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x0600017D RID: 381 RVA: 0x0000D63C File Offset: 0x0000B83C
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/atoolkit-research-project/");
            }
        }

        // Token: 0x0600017E RID: 382 RVA: 0x0000D668 File Offset: 0x0000B868
        private void Menu_B_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_B = !this.myMenu_B)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally");
            }
        }

        // Token: 0x0600017F RID: 383 RVA: 0x0000D694 File Offset: 0x0000B894
        private void Menu_C_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_C = !this.myMenu_C)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally#part1");
            }
        }

        // Token: 0x06000180 RID: 384 RVA: 0x0000D6C0 File Offset: 0x0000B8C0
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string value = "";
            int num = 0;
            if (!DA.GetData<int>(0, ref num))
            {
                return;
            }
            if (num == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Only 'Res' will be considered! (No W/H)");
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Only 'W/H' will be considered!(No Res)");
            }
            int num2 = 0;
            if (!DA.GetData<int>(1, ref num2))
            {
                return;
            }
            int timeout = 0;
            if (base.Params.Input[1].SourceCount == 0)
            {
                timeout = 600000;
            }
            else
            {
                timeout = num2 * 60 * 1000;
            }
            bool flag = false;
            if (!DA.GetData<bool>(2, ref flag))
            {
                return;
            }
            string str = "";
            if (!DA.GetData<string>(5, ref str))
            {
                return;
            }
            string text = "";
            if (base.Params.Input[6].SourceCount == 0)
            {
                base.Params.Input[6].Optional = false;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)20, "BaseIMG is required!");
                DA.GetData<string>(4, ref text);
            }
            else
            {
                DA.GetData<string>(6, ref text);
                value = CustomFX.ToBase64String(new Bitmap(text), ImageFormat.Png);
            }
            double num3 = 2.0;
            if (!DA.GetData<double>(7, ref num3))
            {
                return;
            }
            int num4 = 512;
            if (!DA.GetData<int>(8, ref num4))
            {
                return;
            }
            int num5 = 512;
            if (!DA.GetData<int>(9, ref num5))
            {
                return;
            }
            bool flag2 = false;
            if (!DA.GetData<bool>(10, ref flag2))
            {
                return;
            }
            string value2 = "";
            List<string> list = new List<string>();
            List<string> list2 = new List<string>();
            string text2 = "";
            string ipaddress = ListenIPport.IPaddress;
            string custPort = ListenIPport.CustPort;
            string str2 = string.Format("http://{0}:{1}", ipaddress, custPort);
            if (flag)
            {
                string address = string.Format(str2 + "/sdapi/v1/upscalers", Array.Empty<object>());
                using (WebClient webClient = new WebClient())
                {
                    value2 = webClient.DownloadString(address);
                }
                foreach (A11_ExtrasUPS a11_ExtrasUPS in JsonConvert.DeserializeObject<List<A11_ExtrasUPS>>(value2))
                {
                    string name = a11_ExtrasUPS.name;
                    list2.Add(name);
                }
            }
            int index = 0;
            int index2 = 0;
            string text3 = "";
            string text4 = "";
            if (base.Params.Input[3].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default Upscaler Model set is 'ESRGAN_4x'");
                text3 = "ESRGAN_4x";
            }
            else if (flag)
            {
                DA.GetData<int>(3, ref index);
                text3 = list2[index];
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)20, "Please, run 'ChkUpscls' before!");
            }
            if (base.Params.Input[4].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default second Upscaler Model set is 'None");
                text4 = "None";
            }
            else if (flag)
            {
                DA.GetData<int>(4, ref index2);
                text4 = list2[index2];
            }
            else
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)20, "Please, run 'ChkUpscls' before!");
            }
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary["resize_mode"] = num;
            dictionary["show_extras_results"] = false;
            dictionary["gfpgan_visibility"] = 0;
            dictionary["codeformer_visibility"] = 0;
            dictionary["codeformer_weight"] = 0;
            dictionary["upscaling_resize"] = num3;
            dictionary["upscaling_resize_w"] = num4;
            dictionary["upscaling_resize_h"] = num5;
            dictionary["upscaling_crop"] = true;
            dictionary["upscaler_1"] = text3;
            dictionary["upscaler_2"] = text4;
            dictionary["extras_upscaler_2_visibility"] = 0;
            dictionary["upscale_first"] = false;
            dictionary["image"] = value;
            Dictionary<string, object> value3 = dictionary;
            if (flag2)
            {
                string value4 = JsonConvert.SerializeObject(value3);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(str2 + "/sdapi/v1/extra-single-image");
                httpWebRequest.Timeout = timeout;
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(value4);
                }
                using (StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()))
                {
                    object arg = JsonConvert.DeserializeObject<object>(streamReader.ReadToEnd());
                    if (UpscalerAIComponent.SiteContainer. p__0 == null)
                    {
                        UpscalerAIComponent.SiteContainer. p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "image", typeof(UpscalerAIComponent), new CSharpArgumentInfo[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                        }));
                    }
                    object arg2 = UpscalerAIComponent.SiteContainer. p__0.Target(UpscalerAIComponent.SiteContainer. p__0, arg);
                    if (UpscalerAIComponent.SiteContainer. p__1 == null)
                    {
                        UpscalerAIComponent.SiteContainer. p__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(UpscalerAIComponent)));
                    }
                    Bitmap bitmap = CustomFX.Base64StringToBitmap(UpscalerAIComponent.SiteContainer. p__1.Target(UpscalerAIComponent.SiteContainer. p__1, arg2));
                    string text5 = CustomFX.UnixTimeStampToDateTime((double)DateTimeOffset.Now.ToUnixTimeSeconds()).ToString();
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
                    string str3;
                    string text6;
                    if (num == 0)
                    {
                        if (text4 != "None")
                        {
                            str3 = string.Format("{0}-ups_{1}+{2}_x{3}", new object[]
                            {
                                fileNameWithoutExtension,
                                text3,
                                text4,
                                num3
                            });
                            text6 = string.Format(string.Concat(new string[]
                            {
                                "Datetime: ",
                                text5,
                                "\nUpscale 1: ",
                                text3,
                                "\nUpscale 2: ",
                                text4,
                                "\n",
                                string.Format("Width: {0}\n", bitmap.Width),
                                string.Format("Height: {0}", bitmap.Height)
                            }), Array.Empty<object>());
                        }
                        else
                        {
                            str3 = string.Format("{0}-ups_{1}_x{2}", fileNameWithoutExtension, text3, num3);
                            text6 = string.Format(string.Concat(new string[]
                            {
                                "Datetime: ",
                                text5,
                                "\nUpscale 1: ",
                                text3,
                                "\n",
                                string.Format("Width: {0}\n", bitmap.Width),
                                string.Format("Height: {0}", bitmap.Height)
                            }), Array.Empty<object>());
                        }
                    }
                    else if (text4 != "None")
                    {
                        str3 = string.Format("{0}-ups_{1}+{2}_size", fileNameWithoutExtension, text3, text4);
                        text6 = string.Format(string.Concat(new string[]
                        {
                            "Datetime: ",
                            text5,
                            "\nUpscale 1: ",
                            text3,
                            "\nUpscale 2: ",
                            text4,
                            "\n",
                            string.Format("Width: {0}\n", bitmap.Width),
                            string.Format("Height: {0}", bitmap.Height)
                        }), Array.Empty<object>());
                    }
                    else
                    {
                        str3 = string.Format("{0}-ups_{1}_size", fileNameWithoutExtension, text3);
                        text6 = string.Format(string.Concat(new string[]
                        {
                            "Datetime: ",
                            text5,
                            "\nUpscale 1: ",
                            text3,
                            "\n",
                            string.Format("Width: {0}\n", bitmap.Width),
                            string.Format("Height: {0}", bitmap.Height)
                        }), Array.Empty<object>());
                    }
                    PropertyItem propertyItem = bitmap.PropertyItems[0];
                    propertyItem.Id = 270;
                    propertyItem.Type = 2;
                    string str4 = text6;
                    byte[] bytes = Encoding.ASCII.GetBytes(str4 + "\0");
                    propertyItem.Value = bytes;
                    propertyItem.Len = bytes.Length;
                    bitmap.SetPropertyItem(propertyItem);
                    text2 = string.Format(str + str3 + ".png", Array.Empty<object>());
                    bitmap.Save(text2);
                }
            }
            list = list2;
            string text7 = text2;
            DA.SetDataList(0, list);
            DA.SetData(1, text7);
        }

        // Token: 0x17000089 RID: 137
        // (get) Token: 0x06000181 RID: 385 RVA: 0x0000DF60 File Offset: 0x0000C160
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)8;
            }
        }

        // Token: 0x1700008A RID: 138
        // (get) Token: 0x06000182 RID: 386 RVA: 0x0000DF63 File Offset: 0x0000C163
        protected override Bitmap Icon
        {
            get
            {
                return Resources.Upscaler1_icon1_24;
            }
        }

        // Token: 0x1700008B RID: 139
        // (get) Token: 0x06000183 RID: 387 RVA: 0x0000DF6A File Offset: 0x0000C16A
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("308E14C9-9095-41AA-96A0-69E35DEA7206");
            }
        }

        // Token: 0x04000058 RID: 88
        public bool myMenu_info;

        // Token: 0x04000059 RID: 89
        public bool myMenu_A;

        // Token: 0x0400005A RID: 90
        public bool myMenu_B;

        // Token: 0x0400005B RID: 91
        public bool myMenu_C;
    }
}
