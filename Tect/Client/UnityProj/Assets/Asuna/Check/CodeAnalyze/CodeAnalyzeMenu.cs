using Asuna.Utils;
using UnityEditor;

namespace Asuna.Check
{
    public static class CodeAnalyzeMenu
    {
        [MenuItem("Asuna/Check/CodeAnalyze")]
        public static void StartCodeSpecificationCheckPipeline()
        {
            var pipeline = new CodeSpecificationCheckPipeline();
            pipeline.Run();
        }
    }
}