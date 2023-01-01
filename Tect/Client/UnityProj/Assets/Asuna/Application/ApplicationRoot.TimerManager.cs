using Asuna.Timer;

namespace Asuna.Application
{
    public partial class ApplicationRoot
    {
        private void _InitTimerManager()
        {
            TimerManager = new TimerMgr();
        }

        private void _ReleaseTimerManager()
        {
            TimerManager = null;
        }
        
        
        public TimerMgr TimerManager;
    }
}