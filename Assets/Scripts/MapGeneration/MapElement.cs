using System.Collections.Generic;
using UnityEngine;

namespace MomoCoop.MapGeneration
{
    public class MapElement : MonoBehaviour
    {
        public bool isCollapsed { get; set; }

        public MapModule currentModule { get; set; }

        public List<MapModule> allowedModules { get; set; }
    }
}