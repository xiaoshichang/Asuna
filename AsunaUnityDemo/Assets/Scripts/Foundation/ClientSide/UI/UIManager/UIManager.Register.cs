using System.Collections.Generic;

namespace Asuna.Foundation
{
    public class UIEntry
    {
        public string Name;
        public string AssetPath;
        public UILayer Layer;
    }
    
    
    public static partial class UIManager
    {
        private static void _RegisterUIEntries()
        {
            
        }


        private static readonly Dictionary<string, UIEntry> _Entries = new();

    }
}