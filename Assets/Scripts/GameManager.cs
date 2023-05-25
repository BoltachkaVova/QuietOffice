using System;
using UnityEngine;
using Zenject;

namespace QuietOffice
{
    public class GameManager : IInitializable, IDisposable
    {
        public void Initialize()
        {
            Debug.Log("Init GameManager");
        }

        public void Dispose()
        {
            Debug.Log("Dis GameManager");
        }
    }
}