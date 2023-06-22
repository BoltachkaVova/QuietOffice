using Signals;

namespace UI
{
    public class BreakButton : BaseButton<BreakSignal>
    {
        protected override void OnClick()
        {
            base.OnClick();
            _signalBus.Fire<ActionStateSignal>();
        }
    }
}