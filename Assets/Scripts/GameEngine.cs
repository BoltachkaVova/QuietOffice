using System;
using DG.Tweening;
using Zenject;

namespace QuietOffice
{
    public class GameEngine : IInitializable, IDisposable
    {
        public void Initialize()
        {
            DOTween.SetTweensCapacity(500, 50);
        }

        public void Dispose()
        {
            
        }
    }
}