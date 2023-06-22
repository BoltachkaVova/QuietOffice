using Cysharp.Threading.Tasks;
using Room;
using Signals;

namespace Triggers
{
    public class Work : TriggerPerform
    {
        private Chair _chair;

        private void Awake()
        {
            _chair = GetComponentInChildren<Chair>();
        }

        protected override async void PlayerTriggerEnter()
        {
            _progressBar.Show(durationProgress, viewImage);
            await UniTask.WaitWhile(() => _progressBar.IsActive);
            
            if(!_progressBar.IsDone) return;
            
            _signal.Fire(new WorkStateSignal(transform, _chair.transform));
        }

        protected override void PlayerTriggerExit()
        {
            
        }
        
    }
}