using Enums;
using Signals;
using UnityEngine;

namespace Room
{
    public class Scatter : TriggerPerformBase
    {
        [SerializeField] private TypeInventory tryScatterInventory;
        protected override void PlayerTriggerEnter()
        {
            _signal.Fire(new ScatterHereSignal(transform, tryScatterInventory));
        }

        protected override void PlayerTriggerExit()
        {
            _signal.Fire<TargetLostSignal>();
        }
    }
}