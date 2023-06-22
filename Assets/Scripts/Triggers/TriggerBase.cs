using UnityEngine;

namespace Triggers
{
    public class TriggerBase : MonoBehaviour
    {
        [Header("View Settings")]  
        [SerializeField] protected Sprite viewImage;
        [SerializeField] protected string nameTrigger = "Name";
        [SerializeField] protected string textInfo= "Info";
        [SerializeField] protected int durationProgress;
        
        [SerializeField] protected bool isActiveTrigger;
        
        public string NameTrigger => nameTrigger;
        public bool IsActiveTrigger => isActiveTrigger;
        
    }
}