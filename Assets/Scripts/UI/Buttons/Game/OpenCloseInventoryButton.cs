using Signals;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OpenCloseInventoryButton : BaseButton<OpenCloseInventorySignal>
    {
        [SerializeField] private Sprite openImage;
        [SerializeField] private Sprite closeImage;
        
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.sprite = openImage;
        }

        protected override void OnClick()
        {
            base.OnClick();
            _image.sprite = closeImage;
        }
    }
}