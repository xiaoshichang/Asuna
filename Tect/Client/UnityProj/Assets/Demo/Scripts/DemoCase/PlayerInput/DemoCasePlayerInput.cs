
using Asuna.Application;
using Asuna.Input;

namespace Demo.LoadScene
{
    public class DemoCasePlayerInput : DemoCaseLoadScene
    {
        public override void InitDemo()
        {
            base.InitDemo();
            _InitMainPlayerInput();
        }

        public override void ReleaseDemo()
        {
            _ReleaseMainPlayerInput();
            base.ReleaseDemo();
        }

        public override string GetDemoName()
        {
            return "Player Input";
        }
        
        
        public override void Tick(float dt)
        {
        }
        
        protected override void _OnPlayerLoaded()
        {
            // bind player1 input to this avatar
            G.GameplayInstance.InputMappingSystem.BindPlayerInputToEntity(PlayerType.Player1, _MainPlayer);
        }
    }
}