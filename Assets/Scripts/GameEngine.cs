using System;
using UnityEngine;
using Zenject;

namespace QuietOffice
{
    public class GameEngine : IInitializable, IDisposable
    {
        public void Initialize()
        {
            Debug.Log("GameEngine");
        }

        public void Dispose()
        {
            
        }
    }
}