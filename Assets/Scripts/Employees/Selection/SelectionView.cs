﻿using UnityEngine;

namespace Employees.Selection
{
    public class SelectionView : MonoBehaviour
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