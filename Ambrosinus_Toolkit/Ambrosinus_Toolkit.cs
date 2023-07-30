using System;
using System.Drawing;
using Ambrosinus_Toolkit.Properties;
using Grasshopper.Kernel;


namespace Ambrosinus_Toolkit
{
    // Token: 0x02000003 RID: 3
    public class AmbrosinusToolkitInfo : GH_AssemblyInfo
    {
        // Token: 0x1700000A RID: 10
        // (get) Token: 0x0600000E RID: 14 RVA: 0x00002464 File Offset: 0x00000664
        public override string Name
        {
            get
            {
                return "Ambrosinus Toolkit";
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x0600000F RID: 15 RVA: 0x0000246B File Offset: 0x0000066B
        public override Bitmap Icon
        {
            get
            {
                return Resources.tab_icon_Ambrosinus;
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000010 RID: 16 RVA: 0x00002472 File Offset: 0x00000672
        public override string Description
        {
            get
            {
                return "Ambrosinus Toolkit for Grasshopper | useful components for your design projects";
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000011 RID: 17 RVA: 0x00002479 File Offset: 0x00000679
        public override Guid Id
        {
            get
            {
                return new Guid("81E4839A-77CA-4FD4-A2A1-83140C29AA27");
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000012 RID: 18 RVA: 0x00002485 File Offset: 0x00000685
        public override string AuthorName
        {
            get
            {
                return "Luciano Ambrosini";
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000013 RID: 19 RVA: 0x0000248C File Offset: 0x0000068C
        public override string AuthorContact
        {
            get
            {
                return "email: luciano.ambrosini@outlook.com\nweb: lucianoambrosini.it\n\nSupport Forum: https://bit.ly/AToolkit-SupportForum \n\nGitHub page: https://github.com/lucianoambrosini";
            }
        }
    }
}
