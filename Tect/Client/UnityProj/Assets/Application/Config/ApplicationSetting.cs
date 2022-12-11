using System.Collections.Generic;
using AsunaClient.Foundation.UI;
using UnityEngine;

namespace AsunaClient.Application.Config
{
    [CreateAssetMenu(menuName = "Asuna/Application/ApplicationSetting", order = 1)]
    public class ApplicationSetting : ScriptableObject
    {
        public List<UIPageRegisterItem> UIPageRegisterItems;
    }
    
}