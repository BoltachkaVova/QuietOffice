using UnityEngine;

namespace Minions
{
    public class MinionBase : MonoBehaviour
    {
        private Selection _selection;

        private void Awake()
        {
            _selection = GetComponentInChildren<Selection>();
        }

        public void ThisTarget(bool isOn)
        {
            _selection.IsOn(isOn);
        }
    }
}