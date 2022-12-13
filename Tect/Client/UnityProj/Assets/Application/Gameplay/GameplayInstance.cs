using System.Collections.Generic;
using AsunaClient.Foundation.Entity;

namespace AsunaClient.Application.Gameplay
{
    public class GameplayInitParam
    {
        public ApplicationRoot ApplicationRoot;
        public List<string> GameplayAssemblies;
    }
    
    /// <summary>
    /// gameplay 逻辑的入口，由application驱动
    /// </summary>
    public abstract partial class GameplayInstance
    {
        public virtual void Init(GameplayInitParam param)
        {
            _InitGMSystem(param);
        }

        public virtual void Release()
        {
            _ReleaseGMSystem();
        }
        
        public abstract void EntryGameplay();
        
        
    }
}