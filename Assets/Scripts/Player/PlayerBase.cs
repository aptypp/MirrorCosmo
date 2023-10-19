using UnityEngine;

namespace Game.Game.Player
{
    [CreateAssetMenu(fileName = nameof(PlayerBase), menuName = "Game/Player/" + nameof(PlayerBase))]
    public class PlayerBase : ScriptableObject
    {
        public float speed => _speed;
        public float runSpeed => _runSpeed;
        public float sensitivity => _sensitivity;

        [SerializeField]
        private float _speed;
        [SerializeField]
        private float _runSpeed;
        [SerializeField]
        private float _sensitivity;
    }
}