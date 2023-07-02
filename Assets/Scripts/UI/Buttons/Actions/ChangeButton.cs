using Signals;

namespace UI.Buttons.Actions
{
    public class ChangeButton : BaseButton<ChangeSignal>
    {
        protected override void OnClick()
        {
            base.OnClick();
            _signalBus.Fire<ActionStateSignal>();
        }
    }
}