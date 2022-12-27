using Asuna.Application;

namespace Demo
{
    public class DemoGameplayInstance : GameplayInstance
    {
        public override void EntryGameplay()
        {
            G.UIManager.ShowPage(nameof(DemoMainPage), null);
        }

    }
}