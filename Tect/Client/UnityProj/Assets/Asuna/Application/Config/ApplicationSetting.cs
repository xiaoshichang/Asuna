using System.Collections.Generic;
using Asuna.UI;
using UnityEngine;

namespace Asuna.Application
{
    [CreateAssetMenu(menuName = "Asuna/Application/ApplicationSetting", order = 1)]
    public class ApplicationSetting : ScriptableObject
    {
        public List<string> GameplayAssemblies;
    }
    
}