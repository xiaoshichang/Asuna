using UnityEngine;

namespace Asuna.Utils
{
    public static partial class ADebug
    {

        #region Draw Line
        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            UnityEngine.Debug.DrawLine(start, end, color);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration);
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
        }
        #endregion

    }
}