using System;
using Ambrosinus_Toolkit.Properties;
using Grasshopper;
using Grasshopper.Kernel;

namespace Ambrosinus_Toolkit.Utils
{
    // Token: 0x02000009 RID: 9
    public class TabProperties : GH_AssemblyPriority
    {
        // Token: 0x0600003C RID: 60 RVA: 0x00002675 File Offset: 0x00000875
        public override GH_LoadingInstruction PriorityLoad()
        {
            GH_ComponentServer componentServer = Instances.ComponentServer;
            componentServer.AddCategoryShortName("Ambrosinus", "AL");
            componentServer.AddCategorySymbolName("Ambrosinus", 'A');
            componentServer.AddCategoryIcon("Ambrosinus", Resources.tab_icon_Ambrosinus);
            return 0;
        }
    }
}
