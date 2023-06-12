using UnityEngine;

namespace Room
{
    public class TriggerBase : MonoBehaviour
    {
        [Header("View Settings")]
        [SerializeField] protected Sprite viewImage;
        [SerializeField] protected string nameInventory = "Inventory";
        [SerializeField] protected string textInfo= "Info";
        [SerializeField] protected int durationProgress;

        public string NameInventory => nameInventory;
        public string TextInfo => textInfo;
        public Sprite ViewImage => viewImage;
        public int DurationProgress => durationProgress;
    }
}