using Enums;
using Signals;
using UnityEngine;

namespace UI
{
    public class SelectInventoryButton : BaseButton<SelectTargetSignal>
    {
        [SerializeField] private TypeInventory typeInventory;
        protected override void OnClick()
        {
            _signalBus.Fire(new SelectTargetSignal(null, typeInventory));
            _signalBus.Fire<ThrowStateSignal>();
        }
    }
}