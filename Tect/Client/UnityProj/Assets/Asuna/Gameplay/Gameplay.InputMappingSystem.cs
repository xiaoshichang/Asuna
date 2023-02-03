namespace Asuna.Gameplay
{
    public partial class GameplayInstance
    {
        private void _InitInputMappingSystem()
        {
            InputMappingSystem = new InputMappingSystem();
            InputMappingSystem.Init(null);
        }

        private void _ReleaseInputMappingSystem()
        {
            InputMappingSystem.Release();
            InputMappingSystem = null;
        }
        
        public InputMappingSystem InputMappingSystem;
    }
}