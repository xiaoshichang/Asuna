using Asuna.Input;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private void _InitInputManager()
        {
            PlayerInputManager = new PlayerInputManager();
            PlayerInputManager.Init(null);
        }

        private void _ReleaseInputManager()
        {
            PlayerInputManager.Release();
            PlayerInputManager = null;
        }

        public PlayerInputManager PlayerInputManager;
    }
}