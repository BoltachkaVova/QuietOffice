using System;
using UnityEngine;
using UnityEngine.UI;

namespace Minions
{
    public class Selection : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void IsOn(bool isOn)
        {
            _canvasGroup.alpha = isOn ? 1 : 0;
        }
    }
}