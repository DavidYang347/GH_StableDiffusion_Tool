using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Ambrosinus_Toolkit.Functions;
using Ambrosinus_Toolkit.Properties;
using Ambrosinus_Toolkit.Utils;
using Grasshopper.Kernel;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace Ambrosinus_Toolkit.Obsolete
{
    // Token: 0x0200000C RID: 12
    public class AIengineComponent_OBSOLETE : GH_Component
    {
        // Token: 0x06000062 RID: 98 RVA: 0x00002A71 File Offset: 0x00000C71
        public AIengineComponent_OBSOLETE() : base("AIeNG_loc", "LA_AIeNG_loc", "This component takes advantage of the Automatic1111 project for running Stable Diffusion locally on your machine as an AI local engine for  AI generative art creations or even as render engine. It requires a previous installation of the Stable Diffusion and ControlNET projects. Right click on this component for more info.\nby Luciano Ambrosini", "Ambrosinus", "3.AI")
        {
        }


        public static class SiteContainer
        {
            public static CallSite<Func<CallSite, object, object>> p__0;
            public static CallSite<Func<CallSite, object, object>> p__1;
            public static CallSite<Func<CallSite, Type, object, object>> p__2;
            public static CallSite<Func<CallSite, object, object>> p__3;
            public static CallSite<Func<CallSite, Type, object, object>> p__4;
            public static CallSite<Func<CallSite, Type, object, object>> p__5;
            public static CallSite<Func<CallSite, object, string>> p__6;
            public static CallSite<Func<CallSite, object, object>> p__7;
            public static CallSite<Func<CallSite, int, object, object>> p__8;
            public static CallSite<Func<CallSite, object, bool>> p__9;
            public static CallSite<Func<CallSite, object, int, object>> p__10;
            public static CallSite<Func<CallSite, object, string>> p__11;
        }


        // Token: 0x06000063 RID: 99 RVA: 0x00002A94 File Offset: 0x00000C94
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("CN version", "CNvers", "Select the ControlNET version. Remember that 'LaunchSDlocal' component should point at different Stable Diffusion folder according to the CN vesion selected and installed on your machine", 0, 0);
            pManager.AddIntegerParameter("Mode Pretrained", "Mode", "Select the inference mode to run", 0, 0);
            pManager.AddIntegerParameter("Timeout custom", "Timeout", "If you want increase the WebRequest timeout (default= 10 =>10mins) add an int custom value as mins", 0, 10);
            pManager.AddTextParameter("Directory Path", "DirPath", "Assign the path of the folder wherein save all the images generated", 0);
            pManager.AddTextParameter("BaseImage Path", "BaseIMG", "Source image path for all the ControlNET text2img processes", 0, "");
            pManager.AddTextParameter("Prompt", "Prompt", "Input Text prompt", 0);
            pManager.AddTextParameter("Negative Prompt", "nPrompt", "Text prompt as negative input (whatever to avoid as output result) it is optional", 0, "");
            pManager.AddIntegerParameter("Steps", "Steps", "Number of steps for removing noise in the 'diffusion' process", 0, 10);
            pManager.AddIntegerParameter("Cfg scale", "Cfg", "Cfg scale parameter that controls how much the image generation process follows the text prompt", 0, 7);
            pManager.AddNumberParameter("Weight", "Weight", "Parameter that controls how much the image generation process follows the given prompt (ignored with 'T2I Basic' Mode)", 0, 1.0);
            pManager.AddNumberParameter("Guidance Start", "Gstart", "Parameter that controls how much the image generation process follows the source image", 0, 0.0);
            pManager.AddNumberParameter("Guidance End", "Gend", "Parameter that controls how much the image generation process follows at the end of the prompt", 0, 1.0);
            pManager.AddIntegerParameter("Width", "W", "Final image width", 0, 512);
            pManager.AddIntegerParameter("Height", "H", "Final image height", 0, 512);
            pManager.AddIntegerParameter("Resize", "Resize", "Type of resizing modes", 0);
            pManager.AddIntegerParameter("Number imgs", "N", "Number of images to generate", 0, 1);
            pManager.AddIntegerParameter("Sampler model", "Sampler", "Type of inference sampler model", 0);
            pManager.AddIntegerParameter("Seed", "Seed", "Seed value to discretize the process settings", 0, 0);
            pManager.AddBooleanParameter("Start", "Start", "Run this component", 0);
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
            pManager[10].MutableNickName = false;
            pManager[11].MutableNickName = false;
            pManager[12].MutableNickName = false;
            pManager[13].MutableNickName = false;
            pManager[14].MutableNickName = false;
            pManager[15].MutableNickName = false;
            pManager[16].MutableNickName = false;
            pManager[17].MutableNickName = false;
            pManager[18].MutableNickName = false;
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00002D90 File Offset: 0x00000F90
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Output", "Output", "Output list of the generated images", (GH_ParamAccess)1);
            pManager.AddTextParameter("Info settings", "Info", "Info parameters embedded to the image generated", 0);
            pManager[0].MutableNickName = false;
            pManager[1].MutableNickName = false;
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00002DE5 File Offset: 0x00000FE5
        protected override void BeforeSolveInstance()
        {
            base.BeforeSolveInstance();
            base.MutableNickName = false;
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00002DF4 File Offset: 0x00000FF4
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            GH_DocumentObject.Menu_AppendItem(menu, "AmbrosinusDEV Log", new EventHandler(this.Menu_info_DoClick), Resources.Ambrosinus_DEV_logo_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Project Info", new EventHandler(this.Menu_A_DoClick), Resources.resprj_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "About SD locally", new EventHandler(this.Menu_B_DoClick), Resources.comps_logo1_24);
            GH_DocumentObject.Menu_AppendItem(menu, "Requirements", new EventHandler(this.Menu_C_DoClick), Resources.requirem_logo1_24);
        }

        // Token: 0x06000067 RID: 103 RVA: 0x00002E7C File Offset: 0x0000107C
        private void Menu_info_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_info = !this.myMenu_info)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ambrosinus-toolkit/");
            }
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00002EA8 File Offset: 0x000010A8
        private void Menu_A_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_A = !this.myMenu_A)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/atoolkit-research-project/");
            }
        }

        // Token: 0x06000069 RID: 105 RVA: 0x00002ED4 File Offset: 0x000010D4
        private void Menu_B_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_B = !this.myMenu_B)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally");
            }
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00002F00 File Offset: 0x00001100
        private void Menu_C_DoClick(object sender, EventArgs e)
        {
            if (this.myMenu_C = !this.myMenu_C)
            {
                Process.Start("https://ambrosinus.altervista.org/blog/ai-as-rendering-eng-sd-controlnet-locally#part1");
            }
        }

        // Token: 0x0600006B RID: 107 RVA: 0x00002F2C File Offset: 0x0000112C
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int num = 0;
            if (base.Params.Input[0].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default CNvers set up on 'v1.0'");
                DA.GetData<int>(0, ref num);
            }
            else
            {
                DA.GetData<int>(0, ref num);
                if (num != 0)
                {
                    MessageBox.Show("The ControlNET v1.1.x has been implemented but does not have full API support! Please use ControlNET v1.0 OR go on just for testing it.\n\nIn this case, be sure to have selected in the 'LaunchSD_loc' component the DirPath that points to the folder where you have installed A1111 with ControlNET v1.1.x and, finally, launched it according to the AmbrosinusToolkit procedure.", "ControlNET info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            int num2 = 1;
            if (base.Params.Input[1].SourceCount == 0)
            {
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Default Mode set up on 'T2I Basic'");
                DA.GetData<int>(1, ref num2);
            }
            else
            {
                DA.GetData<int>(1, ref num2);
            }
            int num3 = 0;
            if (!DA.GetData<int>(2, ref num3))
            {
                return;
            }
            int timeout;
            if (base.Params.Input[2].SourceCount == 0)
            {
                timeout = 600000;
            }
            else
            {
                timeout = num3 * 60 * 1000;
            }
            string text = "";
            if (!DA.GetData<string>(3, ref text))
            {
                return;
            }
            string filename = "";
            if (num2 == 0 && base.Params.Input[4].SourceCount != 0)
            {
                base.Params.Input[4].Optional = true;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "BaseIMG is not required");
                DA.GetData<string>(4, ref filename);
            }
            else if (num2 == 0 && base.Params.Input[4].SourceCount == 0)
            {
                base.Params.Input[4].Optional = true;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "T2I Basic selected.\nNo BaseIMG required");
                DA.GetData<string>(4, ref filename);
            }
            else if (num2 != 0 && base.Params.Input[4].SourceCount == 0)
            {
                base.Params.Input[4].Optional = false;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)20, "BaseIMG is required!");
                DA.GetData<string>(4, ref filename);
            }
            else
            {
                DA.GetData<string>(4, ref filename);
            }
            string text2 = "";
            if (!DA.GetData<string>(5, ref text2))
            {
                return;
            }
            string text3 = "";
            if (!DA.GetData<string>(6, ref text3))
            {
                return;
            }
            int num4 = 10;
            if (!DA.GetData<int>(7, ref num4))
            {
                return;
            }
            int num5 = 7;
            if (!DA.GetData<int>(8, ref num5))
            {
                return;
            }
            double num6 = 1.0;
            if (num2 == 0 && base.Params.Input[9].SourceCount != 0)
            {
                base.Params.Input[9].Optional = true;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Weight is not required");
                DA.GetData<double>(9, ref num6);
            }
            else if (num2 == 0 && base.Params.Input[9].SourceCount == 0)
            {
                base.Params.Input[9].Optional = true;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "T2I Basic selected.\nNo Weight required");
                DA.GetData<double>(9, ref num6);
            }
            else if (num2 != 0 && base.Params.Input[9].SourceCount == 0)
            {
                base.Params.Input[9].Optional = false;
                this.AddRuntimeMessage((GH_RuntimeMessageLevel)255, "Weight could be required.\nDefault=1");
                DA.GetData<double>(9, ref num6);
            }
            else
            {
                DA.GetData<double>(9, ref num6);
            }
            double num7 = 0.0;
            if (!DA.GetData<double>(10, ref num7))
            {
                return;
            }
            double num8 = 1.0;
            if (!DA.GetData<double>(11, ref num8))
            {
                return;
            }
            int num9 = 512;
            if (!DA.GetData<int>(12, ref num9))
            {
                return;
            }
            int num10 = 512;
            if (!DA.GetData<int>(13, ref num10))
            {
                return;
            }
            int index = 0;
            if (!DA.GetData<int>(14, ref index))
            {
                return;
            }
            int num11 = 1;
            if (!DA.GetData<int>(15, ref num11))
            {
                return;
            }
            int index2 = 0;
            if (!DA.GetData<int>(16, ref index2))
            {
                return;
            }
            int num12 = 0;
            if (!DA.GetData<int>(17, ref num12))
            {
                return;
            }
            double num13;
            if (base.Params.Input[17].SourceCount == 0)
            {
                num13 = -1.0;
            }
            else
            {
                num13 = (double)num12;
            }
            bool flag = false;
            if (!DA.GetData<bool>(18, ref flag))
            {
                return;
            }
            List<string> list = new List<string>();
            string text4 = "Not yet run...";
            bool flag2 = num != 0;
            List<string> list2 = new List<string>();
            List<string> list3 = new List<string>();
            List<string> list4 = new List<string>();
            if (!flag2)
            {
                base.Message = "ControlNET v1.0";
                list2 = new List<string>
                {
                    "None",
                    "control_sd15_canny [fef5e48e]",
                    "control_sd15_depth [fef5e48e]",
                    "control_sd15_depth [fef5e48e]",
                    "control_sd15_hed [fef5e48e]",
                    "control_sd15_mlsd [fef5e48e]",
                    "control_sd15_normal [fef5e48e]",
                    "control_sd15_openpose [fef5e48e]",
                    "control_sd15_openpose [fef5e48e]",
                    "control_sd15_scribble [fef5e48e]",
                    "control_sd15_scribble [fef5e48e]",
                    "control_sd15_seg [fef5e48e]"
                };
                list3 = new List<string>
                {
                    "none",
                    "canny",
                    "depth",
                    "depth_leres",
                    "hed",
                    "mlsd",
                    "normal_map",
                    "openpose",
                    "openpose_hand",
                    "scribble",
                    "fake_scribble",
                    "segmentation"
                };
                list4 = new List<string>
                {
                    "bsc",
                    "cny",
                    "dpt",
                    "dptL",
                    "hed",
                    "mlsd",
                    "nrm",
                    "pse",
                    "pseH",
                    "scb",
                    "Fscb",
                    "seg"
                };
            }
            else
            {
                base.Message = "ControlNET v1.1";
                list2 = new List<string>
                {
                    "None",
                    "control_v11p_sd15_canny [d14c016b]",
                    "control_v11f1p_sd15_depth [cfd03158]",
                    "control_v11f1p_sd15_depth [cfd03158]",
                    "control_v11f1p_sd15_depth [cfd03158]",
                    "control_v11p_sd15s2_lineart_anime [3825e83e]",
                    "control_v11p_sd15_lineart [43d4be0d]",
                    "control_v11p_sd15_lineart [43d4be0d]",
                    "control_v11p_sd15_lineart [43d4be0d]",
                    "control_v11p_sd15_mlsd [aca30ff0]",
                    "control_v11p_sd15_normalbae [316696f1]",
                    "control_v11p_sd15_normalbae [316696f1]",
                    "control_v11p_sd15_openpose [cab727d4]",
                    "control_v11p_sd15_openpose [cab727d4]",
                    "control_v11p_sd15_openpose [cab727d4]",
                    "control_v11p_sd15_openpose [cab727d4]",
                    "control_v11p_sd15_openpose [cab727d4]",
                    "control_v11p_sd15_scribble [d4ba51ff]",
                    "control_v11p_sd15_scribble [d4ba51ff]",
                    "control_v11p_sd15_scribble [d4ba51ff]",
                    "control_v11p_sd15_seg [e1f51eb9]",
                    "control_v11p_sd15_seg [e1f51eb9]",
                    "control_v11p_sd15_seg [e1f51eb9]",
                    "control_v11e_sd15_shuffle [526bfdae]",
                    "control_v11p_sd15_softedge [a8575a2a]",
                    "control_v11p_sd15_softedge [a8575a2a]",
                    "control_v11p_sd15_softedge [a8575a2a]",
                    "control_v11p_sd15_softedge [a8575a2a]",
                    "control_v11u_sd15_tile [1f041471]"
                };
                list3 = new List<string>
                {
                    "none",
                    "canny",
                    "depth_leres",
                    "depth_midas",
                    "depth_zoe",
                    "lineart_anime",
                    "lineart_coarse",
                    "lineart_realistic",
                    "lineart_standard (from white bg & black line)",
                    "mlsd",
                    "normal_bae",
                    "normal_midas",
                    "openpose",
                    "openpose_face",
                    "openpose_faceonly",
                    "openpose_full",
                    "openpose_hand",
                    "scribble_hed",
                    "scribble_pidinet",
                    "scribble_xdog",
                    "seg_ofade20k",
                    "seg_ofcoco",
                    "seg_ufade20k",
                    "shuffle",
                    "softedge_hed",
                    "softedge_hedsafe",
                    "softedge_pidinet",
                    "softedge_pidisafe",
                    "tile_gaussian"
                };
                list4 = new List<string>
                {
                    "bsc",
                    "cny",
                    "dptL",
                    "dptM",
                    "dptZ",
                    "lartA",
                    "lartC",
                    "lartR",
                    "lartS",
                    "mlsd",
                    "nrmB",
                    "nrmM",
                    "pse",
                    "pseF",
                    "pseFO",
                    "pseFU",
                    "pseH",
                    "scbH",
                    "scbP",
                    "scbX",
                    "segOA",
                    "segOC",
                    "segUA",
                    "shff",
                    "softH",
                    "softHS",
                    "softPN",
                    "softPS",
                    "tilG"
                };
            }
            string text5 = "";
            if (num2 != 20)
            {
                text5 = "T2I";
            }
            List<string> list5 = new List<string>
            {
                "Just Resize",
                "Scale to Fit (Inner Fit)",
                "Envelope (Outer Fit)"
            };
            List<string> list6 = new List<string>
            {
                "Euler a",
                "Euler",
                "LMS",
                "Heun",
                "DPM2",
                "DPM2 a",
                "DPM++ 2S a",
                "DPM++ 2M",
                "DPM++ SDE",
                "DPM fast",
                "DPM adaptive",
                "LMS Karras",
                "DPM2 Karras",
                "DPM2 a Karras",
                "DPM++ 2S a Karras",
                "DPM++ 2M Karras",
                "DPM++ SDE Karras",
                "DDIM",
                "PLMS",
                "UniPC"
            };
            string ipaddress = ListenIPport.IPaddress;
            string custPort = ListenIPport.CustPort;
            string arg = string.Format("http://{0}:{1}", ipaddress, custPort);
            string requestUriString = "";
            if (num2 != 0)
            {
                requestUriString = string.Format("{0}/sdapi/v1/txt2img", arg);
            }
            else if (num2 == 0)
            {
                requestUriString = string.Format("{0}/sdapi/v1/txt2img", arg);
            }
            if (flag)
            {
                string value2;
                if (num2 != 0)
                {
                    Bitmap bitmap = new Bitmap(filename);
                    Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
                    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                    if (num == 0)
                    {
                        string value = CustomFX.ToBase64String(bitmap, ImageFormat.Png);
                        dictionary2.Add("args", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                {
                                    "input_image",
                                    value
                                },
                                {
                                    "mask",
                                    ""
                                },
                                {
                                    "module",
                                    list3[num2]
                                },
                                {
                                    "model",
                                    list2[num2]
                                },
                                {
                                    "weight",
                                    num6
                                },
                                {
                                    "resize_mode",
                                    list5[index]
                                },
                                {
                                    "lowvram",
                                    true
                                },
                                {
                                    "processor_res",
                                    64
                                },
                                {
                                    "threshold_a",
                                    64
                                },
                                {
                                    "threshold_b",
                                    64
                                },
                                {
                                    "guidance",
                                    1
                                },
                                {
                                    "guidance_start",
                                    num7
                                },
                                {
                                    "guidance_end",
                                    num8
                                },
                                {
                                    "guessmode",
                                    false
                                }
                            }
                        });
                    }
                    else
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        bitmap.Save(memoryStream, ImageFormat.Png);
                        string value = Convert.ToBase64String(memoryStream.ToArray());
                        dictionary2.Add("args", new List<Dictionary<string, object>>
                        {
                            new Dictionary<string, object>
                            {
                                {
                                    "input_image",
                                    value
                                },
                                {
                                    "module",
                                    list3[num2]
                                },
                                {
                                    "model",
                                    list2[num2]
                                },
                                {
                                    "weight",
                                    num6
                                },
                                {
                                    "resize_mode",
                                    list5[index]
                                },
                                {
                                    "lowvram",
                                    true
                                },
                                {
                                    "processor_res",
                                    64
                                },
                                {
                                    "threshold_a",
                                    64
                                },
                                {
                                    "threshold_b",
                                    64
                                },
                                {
                                    "guidance",
                                    1
                                },
                                {
                                    "guidance_start",
                                    num7
                                },
                                {
                                    "guidance_end",
                                    num8
                                }
                            }
                        });
                    }
                    dictionary.Add("controlnet", dictionary2);
                    Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
                    dictionary3["prompt"] = text2;
                    dictionary3["negative_prompt"] = text3;
                    dictionary3["seed"] = num13;
                    dictionary3["subseed"] = -1;
                    dictionary3["subseed_strength"] = 0;
                    dictionary3["batch_size"] = 1;
                    dictionary3["n_iter"] = num11;
                    dictionary3["steps"] = num4;
                    dictionary3["cfg_scale"] = num5;
                    dictionary3["width"] = num9;
                    dictionary3["height"] = num10;
                    dictionary3["restore_faces"] = true;
                    dictionary3["eta"] = 0;
                    dictionary3["sampler_index"] = list6[index2];
                    dictionary3["alwayson_scripts"] = dictionary;
                    value2 = JsonConvert.SerializeObject(dictionary3);
                }
                else
                {
                    value2 = JsonConvert.SerializeObject(new
                    {
                        prompt = text2,
                        negative_prompt = text3,
                        steps = num4,
                        cfg_scale = num5,
                        width = num9,
                        height = num10,
                        n_iter = num11,
                        seed = num13,
                        sampler_index = list6[index2]
                    });
                }
                JsonConvert.ToString(value2);
                byte[] bytes = Encoding.ASCII.GetBytes(Convert.ToString(value2));
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Timeout = timeout;
                httpWebRequest.KeepAlive = true;
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.ContentLength = (long)bytes.Length;
                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                object arg2 = JsonConvert.DeserializeObject<object>(new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd());
                if (AIengineComponent_OBSOLETE.SiteContainer. p__0 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "images", typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    }));
                }
                object arg3 = AIengineComponent_OBSOLETE.SiteContainer. p__0.Target(AIengineComponent_OBSOLETE.SiteContainer. p__0, arg2);
                if (AIengineComponent_OBSOLETE.SiteContainer. p__1 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "parameters", typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    }));
                }
                object arg4 = AIengineComponent_OBSOLETE.SiteContainer. p__1.Target(AIengineComponent_OBSOLETE.SiteContainer. p__1, arg2);
                if (AIengineComponent_OBSOLETE.SiteContainer. p__2 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__2 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    }));
                }
                AIengineComponent_OBSOLETE.SiteContainer. p__2.Target(AIengineComponent_OBSOLETE.SiteContainer. p__2, typeof(JsonConvert), arg4);
                if (AIengineComponent_OBSOLETE.SiteContainer. p__3 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "info", typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    }));
                }
                object arg5 = AIengineComponent_OBSOLETE.SiteContainer. p__3.Target(AIengineComponent_OBSOLETE.SiteContainer. p__3, arg2);
                if (AIengineComponent_OBSOLETE.SiteContainer. p__4 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__4 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", null, typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    }));
                }
                object arg6 = AIengineComponent_OBSOLETE.SiteContainer. p__4.Target(AIengineComponent_OBSOLETE.SiteContainer. p__4, typeof(JsonConvert), arg5);
                if (AIengineComponent_OBSOLETE.SiteContainer. p__6 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__6 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(AIengineComponent_OBSOLETE)));
                }
                Func<CallSite, object, string> target = AIengineComponent_OBSOLETE.SiteContainer. p__6.Target;
                CallSite p__ = AIengineComponent_OBSOLETE.SiteContainer. p__6;
                if (AIengineComponent_OBSOLETE.SiteContainer. p__5 == null)
                {
                    AIengineComponent_OBSOLETE.SiteContainer. p__5 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "DeserializeObject", null, typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                    }));
                }
                JToken jtoken = JToken.Parse(target( p__, AIengineComponent_OBSOLETE.SiteContainer. p__5.Target(AIengineComponent_OBSOLETE.SiteContainer. p__5, typeof(JsonConvert), arg6)));
                List<string> stringsToRemove = new List<string>
                {
                    "\"prompt\"",
                    "\"all_prompts\"",
                    "\"negative_prompt\"",
                    "\"all_negative_prompts\"",
                    "\"seed\"",
                    "\"all_seeds\"",
                    "\"subseed\"",
                    "\"all_subseeds\"",
                    "\"subseed_strength\"",
                    "\"width\"",
                    "\"height\"",
                    "\"sampler_name\"",
                    "\"cfg_scale\"",
                    "\"steps\"",
                    "\"batch_size\"",
                    "\"restore_faces\"",
                    "\"face_restoration_model\"",
                    "\"sd_model_hash\"",
                    "\"Model\"",
                    "\"seed_resize_from_w\"",
                    "\"seed_resize_from_h\"",
                    "\"denoising_strength\"",
                    "\"extra_generation_params\"",
                    "\"index_of_first_image\"",
                    "\"styles\"",
                    "\"job_timestamp\"",
                    "\"clip_skip\"",
                    "\"is_using_inpainting_conditioning\""
                };
                string[] value3 = (from c in jtoken.Children()
                                   select c.ToString() into s
                                   where !stringsToRemove.Any((string r) => s.StartsWith(r))
                                   select s).ToArray<string>();
                string input = string.Join(" ", value3);
                List<string> list7 = new List<string>();
                string pattern = "\\n\\n";
                string replacement = "\\";
                string[] array = Regex.Replace(input, pattern, replacement).Split(new char[]
                {
                    '\\'
                });
                string item = string.Format("Prompt: {0}\n", text2);
                string text6 = array[1].Split(new char[]
                {
                    ':'
                })[1];
                string item2 = string.Format("Negative Prompt: {0}\n", text3);
                List<string> list8 = array[2].Split(new char[]
                {
                    ','
                }).ToList<string>();
                for (int i = 0; i < list8.Count; i++)
                {
                    string text7 = list8[i].Split(new char[]
                    {
                        ':'
                    })[0];
                    text6 = list8[i].Split(new char[]
                    {
                        ':'
                    })[1];
                    list7.Add(string.Format("{0}: {1}", text7.Trim(), text6.Trim()));
                }
                int count = list7.Count;
                string value4 = list7[count - 1].Split(new char[]
                {
                    '"'
                })[0];
                string value5 = list7[0].Remove(0, 1);
                list7[0] = value5;
                list7[count - 1] = value4;
                List<string> list9 = list7.ToList<string>();
                long num14 = DateTimeOffset.Now.ToUnixTimeSeconds();
                string arg7 = CustomFX.UnixTimeStampToDateTime((double)num14).ToString();
                string item3 = string.Format("IP address: {0}:{1},\n", ipaddress, custPort);
                string item4 = string.Format("Datetime: {0},", arg7);
                List<string> list10 = new List<string>
                {
                    item4,
                    item3,
                    item,
                    item2
                };
                for (int j = 0; j < 4; j++)
                {
                    list9.Insert(j, list10[j]);
                }
                string text8 = string.Join("\n", list9);
                List<char> list11 = new List<char>();
                list11.Add('{');
                list11.Add('}');
                list11.Add('"');
                string text9 = "";
                foreach (char c2 in list11)
                {
                    text9 = text8.Replace(c2.ToString(), string.Empty);
                }
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                int num15 = 0;
                for (; ; )
                {
                    if (AIengineComponent_OBSOLETE.SiteContainer. p__9 == null)
                    {
                        AIengineComponent_OBSOLETE.SiteContainer. p__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                        }));
                    }
                    Func<CallSite, object, bool> target2 = AIengineComponent_OBSOLETE.SiteContainer. p__9.Target;
                    CallSite p__2 = AIengineComponent_OBSOLETE.SiteContainer. p__9;
                    if (AIengineComponent_OBSOLETE.SiteContainer. p__8 == null)
                    {
                        AIengineComponent_OBSOLETE.SiteContainer. p__8 = CallSite<Func<CallSite, int, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.LessThan, typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                        }));
                    }
                    Func<CallSite, int, object, object> target3 = AIengineComponent_OBSOLETE.SiteContainer. p__8.Target;
                    CallSite p__3 = AIengineComponent_OBSOLETE.SiteContainer. p__8;
                    int arg8 = num15;
                    if (AIengineComponent_OBSOLETE.SiteContainer. p__7 == null)
                    {
                        AIengineComponent_OBSOLETE.SiteContainer. p__7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Count", typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
                        }));
                    }
                    if (!target2( p__2, target3( p__3, arg8, AIengineComponent_OBSOLETE.SiteContainer. p__7.Target(AIengineComponent_OBSOLETE.SiteContainer. p__7, arg3))))
                    {
                        break;
                    }
                    if (AIengineComponent_OBSOLETE.SiteContainer. p__11 == null)
                    {
                        AIengineComponent_OBSOLETE.SiteContainer. p__11 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(AIengineComponent_OBSOLETE)));
                    }
                    Func<CallSite, object, string> target4 = AIengineComponent_OBSOLETE.SiteContainer. p__11.Target;
                    CallSite p__4 = AIengineComponent_OBSOLETE.SiteContainer. p__11;
                    if (AIengineComponent_OBSOLETE.SiteContainer. p__10 == null)
                    {
                        AIengineComponent_OBSOLETE.SiteContainer. p__10 = CallSite<Func<CallSite, object, int, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof(AIengineComponent_OBSOLETE), new CSharpArgumentInfo[]
                        {
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                        }));
                    }
                    Bitmap bitmap2 = CustomFX.Base64StringToBitmap(target4( p__4, AIengineComponent_OBSOLETE.SiteContainer. p__10.Target(AIengineComponent_OBSOLETE.SiteContainer. p__10, arg3, num15)));
                    PropertyItem propertyItem = bitmap2.PropertyItems[0];
                    propertyItem.Id = 270;
                    propertyItem.Type = 2;
                    string str = text9;
                    byte[] bytes2 = Encoding.ASCII.GetBytes(str + "\0");
                    propertyItem.Value = bytes2;
                    propertyItem.Len = bytes2.Length;
                    bitmap2.SetPropertyItem(propertyItem);
                    string str2 = string.Format("{0}-{1}_img_{2}_{3}.png", new object[]
                    {
                        text5,
                        list4[num2],
                        num14,
                        num15 + 1
                    });
                    string filename2 = text + "\\" + str2;
                    bitmap2.Save(filename2);
                    num15++;
                }
                list = Directory.GetFiles(text, "*.png").ToList<string>();
                httpWebResponse.Close();
                httpWebRequest.Abort();
                text4 = text9;
            }
            else
            {
                list = Directory.GetFiles(text, "*.png").ToList<string>();
            }
            DA.SetDataList(0, list);
            DA.SetData(1, text4);
        }

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x0600006C RID: 108 RVA: 0x000047B4 File Offset: 0x000029B4
        public override GH_Exposure Exposure
        {
            get
            {
                return (GH_Exposure)(-1);
            }
        }

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x0600006D RID: 109 RVA: 0x000047B7 File Offset: 0x000029B7
        protected override Bitmap Icon
        {
            get
            {
                return Resources.AIrend_icon3_24;
            }
        }

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x0600006E RID: 110 RVA: 0x000047BE File Offset: 0x000029BE
        public override Guid ComponentGuid
        {
            get
            {
                return new Guid("148d7b78-27ad-48f1-b834-b2cb0758a90a");
            }
        }

        // Token: 0x04000025 RID: 37
        public bool myMenu_info;

        // Token: 0x04000026 RID: 38
        public bool myMenu_A;

        // Token: 0x04000027 RID: 39
        public bool myMenu_B;

        // Token: 0x04000028 RID: 40
        public bool myMenu_C;
    }
}
