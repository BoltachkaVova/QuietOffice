using Signals;

namespace UI.Buttons.Actions
{
    public class StopWorkButton : BaseButton<ActiveStateSignal>
    {
        protected override void OnClick()
        {
            base.OnClick();
            _signalBus.Fire<LostTargetSignal>();
        }
    }
}