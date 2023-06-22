using Signals;

namespace UI
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