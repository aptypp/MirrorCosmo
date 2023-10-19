using UnityEngine;

namespace MomoCoop.MapGeneration
{
    public class MapModule : MonoBehaviour
    {
        [field: SerializeField, HideInInspector] public int instanceId { get; private set; }

        [field: SerializeField]
        public MapModule[] excludedModulesLeft { get; private set; }
        [field: SerializeField]
        public MapModule[] excludedModulesRight { get; private set; }
        [field: SerializeField]
        public MapModule[] excludedModulesUp { get; private set; }
        [field: SerializeField]
        public MapModule[] excludedModulesDown { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying) return;

            instanceId = GetInstanceID();
        }
#endif
    }
}