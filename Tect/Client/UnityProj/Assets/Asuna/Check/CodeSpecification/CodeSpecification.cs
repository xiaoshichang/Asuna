using Asuna.Utils;
using UnityEditor;

namespace Asuna.Check
{
    public static class CodeSpecification
    {
        [MenuItem("Asuna/Check/CodeSpecification")]
        public static void StartCodeSpecificationCheckPipeline()
        {
            var pipeline = new CodeSpecificationCheckPipeline();
            pipeline.Run();
        }
    }
}