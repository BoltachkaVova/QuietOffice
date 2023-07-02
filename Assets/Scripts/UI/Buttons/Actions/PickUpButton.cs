using Signals;

namespace UI.Buttons.Actions
{
    public class PickUpButton : BaseButton<PickUpSignal>
    {
        protected override void OnClick()
        {
            base.OnClick();
            _signalBus.Fire<ActionStateSignal>();
        }
    }
}