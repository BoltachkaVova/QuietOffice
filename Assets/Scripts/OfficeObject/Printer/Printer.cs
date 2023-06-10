using System.Collections.Generic;
using UnityEngine;

namespace OfficeObject
{
    public class Printer : MonoBehaviour
    {
        [SerializeField] private int countSpawnFiles;
        [SerializeField] private OfficeFiles prefabFiles;


        private List<OfficeFiles> _officeFileses =  new List<OfficeFiles>(10);

    }
}