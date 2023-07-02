using Signals;

namespace UI.Buttons.Actions
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