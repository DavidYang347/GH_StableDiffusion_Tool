using System;

namespace Ambrosinus_Toolkit.Utils
{
    // Token: 0x0200000A RID: 10
    public class ToolkitVersionProperties
    {
        // Token: 0x17000021 RID: 33
        // (get) Token: 0x0600003E RID: 62 RVA: 0x000026B4 File Offset: 0x000008B4
        public string Version
        {
            get
            {
                return string.Concat(new string[]
                {
                    ToolkitVersionProperties.MAJOR_VERSION.ToString(),
                    ".",
                    ToolkitVersionProperties.MINOR_VERSION.ToString(),
                    ".",
                    ToolkitVersionProperties.PATCH_VERSION.ToString()
                });
            }
        }

        // Token: 0x04000020 RID: 32
        public static int MAJOR_VERSION = 1;

        // Token: 0x04000021 RID: 33
        public static int MINOR_VERSION = 2;

        // Token: 0x04000022 RID: 34
        public static int PATCH_VERSION = 3;
    }
}
