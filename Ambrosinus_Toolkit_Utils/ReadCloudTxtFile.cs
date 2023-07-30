using System;
using System.Net;

namespace Ambrosinus_Toolkit.Utils
{
    // Token: 0x02000007 RID: 7
    public class ReadCloudTxtFile
    {
        // Token: 0x1700001B RID: 27
        // (get) Token: 0x0600002F RID: 47 RVA: 0x00002584 File Offset: 0x00000784
        public string CurrentVersionInfo
        {
            get
            {
                return ReadCloudTxtFile.latestVersInfo;
            }
        }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000030 RID: 48 RVA: 0x0000258B File Offset: 0x0000078B
        public string LatestVersionInfo
        {
            get
            {
                return this.LatestVers;
            }
        }

        // Token: 0x04000017 RID: 23
        private static string latestVersInfo = new WebClient().DownloadString("https://raw.githubusercontent.com/lucianoambrosini/Ambrosinus-Toolkit/main/latest_version.txt");

        // Token: 0x04000018 RID: 24
        private static char sep1 = ':';

        // Token: 0x04000019 RID: 25
        private static char sep2 = '-';

        // Token: 0x0400001A RID: 26
        private static string[] chunks = ReadCloudTxtFile.latestVersInfo.Split(new char[]
        {
            ReadCloudTxtFile.sep1,
            ReadCloudTxtFile.sep2
        }, StringSplitOptions.RemoveEmptyEntries);

        // Token: 0x0400001B RID: 27
        private string LatestVers = ReadCloudTxtFile.chunks[2].Remove(0, 1);
    }
}
