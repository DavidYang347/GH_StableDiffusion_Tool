using System;

namespace Ambrosinus_Toolkit.Utils
{
    // Token: 0x02000008 RID: 8
    internal class StoreIMG
    {
        // Token: 0x06000033 RID: 51 RVA: 0x00002605 File Offset: 0x00000805
        public StoreIMG()
        {
            this.stored = false;
            this.filepath = "Not yet run...";
            this.folderpath = " ";
            this.counter = 0;
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x06000034 RID: 52 RVA: 0x00002631 File Offset: 0x00000831
        // (set) Token: 0x06000035 RID: 53 RVA: 0x00002639 File Offset: 0x00000839
        public bool stored { get; set; }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x06000036 RID: 54 RVA: 0x00002642 File Offset: 0x00000842
        // (set) Token: 0x06000037 RID: 55 RVA: 0x0000264A File Offset: 0x0000084A
        public string filepath { get; set; }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x06000038 RID: 56 RVA: 0x00002653 File Offset: 0x00000853
        // (set) Token: 0x06000039 RID: 57 RVA: 0x0000265B File Offset: 0x0000085B
        public string folderpath { get; set; }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x0600003A RID: 58 RVA: 0x00002664 File Offset: 0x00000864
        // (set) Token: 0x0600003B RID: 59 RVA: 0x0000266C File Offset: 0x0000086C
        public int counter { get; set; }
    }
}
