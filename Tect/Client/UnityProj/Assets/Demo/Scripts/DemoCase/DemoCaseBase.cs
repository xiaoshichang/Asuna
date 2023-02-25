using Asuna.Application;
using Asuna.Input;
using Asuna.Utils;
using InControl;

namespace Demo
{
    public abstract class DemoCaseBase
    {
        public abstract void InitDemo();
        public abstract void ReleaseDemo();
        public abstract void Tick(float dt); 
        public abstract string GetDemoName();
        
        protected void _InitMainPlayerInput()
        {
            var device = G.PlayerInputManager.GetAvailableDevice();
            if (device == null)
            {
                ADebug.Warning("no available device found!");
                return;
            }
            var actionSet = new DefaultPlayerActionSet();
            var mapping = new PlayerInputMapping(device, actionSet);
            G.PlayerInputManager.SetupPlayerInputMapping(mapping);
        }

        protected void _ReleaseMainPlayerInput()
        {
            G.PlayerInputManager.ClearPlayerInputMapping();
        }
    }
}