using DG.Tweening;
using TMPro;
using UnityEngine;

namespace QuietOffice
{
    public class FPS : MonoBehaviour
    {
        private TextMeshProUGUI _fpsText;
        private int _fpsValue;

        private void Awake()
        {
            _fpsText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            var value = Mathf.RoundToInt(1f / Time.deltaTime);
            
            DOTween.To(() => _fpsValue, x => _fpsValue = x, value, 2f)
                .OnUpdate(() => _fpsText.text = _fpsValue.ToString());
        }
    }
}