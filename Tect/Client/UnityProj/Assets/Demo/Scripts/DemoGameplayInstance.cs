using AF.Application;

namespace Demo.Scripts
{
    public class DemoGameplayInstance : GameplayInstance
    {
        public override void EntryGameplay()
        {
            G.UIManager.ShowPage("DemoMainPage", null);
        }

    }
}