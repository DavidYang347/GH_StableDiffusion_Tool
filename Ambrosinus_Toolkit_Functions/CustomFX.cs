using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ambrosinus_Toolkit.Functions
{
    // Token: 0x02000016 RID: 22
    public class CustomFX
    {
        // Token: 0x060000C9 RID: 201 RVA: 0x00006DB4 File Offset: 0x00004FB4
        public static PointF[] PolylineToPoints(Polyline curve)
        {
            PointF[] array = new PointF[curve.Count];
            for (int i = 0; i < curve.Count; i++)
            {
                float x = (float)curve[i].X;
                float y = (float)curve[i].Y;
                array[i] = new PointF(x, y);
            }
            return array;
        }

        // Token: 0x060000CA RID: 202 RVA: 0x00006E14 File Offset: 0x00005014
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            result = result.AddSeconds(unixTimeStamp).ToLocalTime();
            return result;
        }

        // Token: 0x060000CB RID: 203 RVA: 0x00006E48 File Offset: 0x00005048
        public static IGH_DocumentObject FindObj(GH_Document ghDocument, string name, string guid)
        {
            for (int i = 0; i < ghDocument.ObjectCount; i++)
            {
                IGH_DocumentObject igh_DocumentObject = ghDocument.Objects[i];
                if (name != null && igh_DocumentObject.NickName.Equals(name, StringComparison.Ordinal) && igh_DocumentObject != null)
                {
                    return igh_DocumentObject;
                }
                if (guid != null && igh_DocumentObject.ComponentGuid.Equals(guid) && igh_DocumentObject != null)
                {
                    return igh_DocumentObject;
                }
            }
            return null;
        }

        // Token: 0x060000CC RID: 204 RVA: 0x00006EAC File Offset: 0x000050AC
        public static string GetLine(string fileName, int line)
        {
            string result;
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                for (int i = 1; i < line; i++)
                {
                    streamReader.ReadLine();
                }
                result = streamReader.ReadLine();
            }
            return result;
        }

        // Token: 0x060000CD RID: 205 RVA: 0x00006EF8 File Offset: 0x000050F8
        public static void RunProcessWebUI(string DirPath)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                WorkingDirectory = DirPath,
                FileName = "cmd.exe",
                Arguments = "/k title AmbrosinusToolkit",
                RedirectStandardInput = true,
                CreateNoWindow = false,
                RedirectStandardOutput = false,
                UseShellExecute = false,
                Verb = "runas"
            };
            process.Start();
            StreamWriter standardInput = process.StandardInput;
            if (standardInput.BaseStream.CanWrite)
            {
                standardInput.WriteLine(string.Format("webui-user.bat", DirPath));
            }
        }

        // Token: 0x060000CE RID: 206 RVA: 0x00006F86 File Offset: 0x00005186
        public static void RunCMDipconfig()
        {
            Process.Start(new ProcessStartInfo("cmd.exe")
            {
                Arguments = "/k title AmbrosinusToolkit - IPconfig Check && ipconfig",
                WindowStyle = ProcessWindowStyle.Normal
            });
        }

        // Token: 0x060000CF RID: 207 RVA: 0x00006FAC File Offset: 0x000051AC
        public static bool IsBusy(int portNumber)
        {
            IPEndPoint[] activeTcpListeners = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
            if (activeTcpListeners == null || activeTcpListeners.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < activeTcpListeners.Length; i++)
            {
                if (activeTcpListeners[i].Port == portNumber)
                {
                    return true;
                }
            }
            return false;
        }

        // Token: 0x060000D0 RID: 208 RVA: 0x00006FEC File Offset: 0x000051EC
        public static Bitmap Base64StringToBitmap(string base64String)
        {
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64String));
            memoryStream.Position = 0L;
            Bitmap result = (Bitmap)Image.FromStream(memoryStream);
            memoryStream.Close();
            return result;
        }

        // Token: 0x060000D1 RID: 209 RVA: 0x00007020 File Offset: 0x00005220
        public static string ToBase64String(Bitmap bmp, ImageFormat imageFormat)
        {
            string empty = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, imageFormat);
            memoryStream.Position = 0L;
            byte[] inArray = memoryStream.ToArray();
            memoryStream.Close();
            return Convert.ToBase64String(inArray);
        }
    }
}
