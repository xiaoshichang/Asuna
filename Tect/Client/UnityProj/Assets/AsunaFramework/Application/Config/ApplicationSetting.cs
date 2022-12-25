using System.Collections.Generic;
using AF.UI;
using UnityEngine;

namespace AF.Application
{
    [CreateAssetMenu(menuName = "Asuna/Application/ApplicationSetting", order = 1)]
    public class ApplicationSetting : ScriptableObject
    {
        public List<string> GameplayAssemblies;
        public List<UIPageRegisterItem> UIPageRegisterItems;
    }
    
}