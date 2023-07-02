using Enums;
using Signals;
using TMPro;
using UnityEngine;

namespace UI.Buttons.Actions
{
    public class SelectInventoryButton : BaseButton<SelectTargetSignal>
    {
        [SerializeField] private TypeInventory typeButtonInventory;
        [SerializeField] private TextMeshProUGUI textCount;

        public TypeInventory TypeButtonInventory => typeButtonInventory;

        public void SetCount(int value)
        {
            textCount.text = value.ToString();
        }
        
        protected override void OnClick()
        {
            _signalBus.Fire(new ThrowStateSignal(typeButtonInventory));
        }
    }
}