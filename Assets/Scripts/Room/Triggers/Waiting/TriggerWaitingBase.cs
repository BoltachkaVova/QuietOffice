using UnityEngine;

namespace Room
{
    public abstract class TriggerWaitingBase : MonoBehaviour
    {
        [Header("View Settings")]  
        [SerializeField] private Sprite viewImage;
        [SerializeField] private string nameTrigger = "Name";
        [SerializeField] private string textInfo= "Info";
        [SerializeField] private int durationProgress;
        
        
        [SerializeField] protected bool isActive;

        public Sprite ViewImage => viewImage;
        public string NameTrigger => nameTrigger;
        public string TextInfo => textInfo;
        public int DurationProgress => durationProgress;
        public bool IsActive => isActive;

    }
}