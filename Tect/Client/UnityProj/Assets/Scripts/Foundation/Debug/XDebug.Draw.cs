using UnityEngine;

namespace AsunaClient.Foundation
{
    public static partial class XDebug
    {

        #region Draw Line
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            Debug.DrawLine(start, end, color);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            Debug.DrawLine(start, end, color, duration);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            Debug.DrawLine(start, end, color, duration, depthTest);
        }
        #endregion

    }
}