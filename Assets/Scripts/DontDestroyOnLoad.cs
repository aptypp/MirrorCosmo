using UnityEngine;

namespace MomoCoop
{
    public sealed class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(this);
    }
}