using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MomoCoop.UI
{
    public sealed class RoomView : MonoBehaviour
    {
        [SerializeField]
        private Button _disconnect;
        [SerializeField]
        private Toggle _readyToggle;
        [SerializeField]
        private TMP_InputField _nicknameInputField;

        private GameClient _gameClient;

        private void Awake()
        {
            _gameClient = FindObjectOfType<GameClient>();

            _disconnect.onClick.AddListener(_gameClient.Disconnect);
            _readyToggle.onValueChanged.AddListener(_gameClient.SetIsReady);
            _nicknameInputField.onValueChanged.AddListener(OnChangedNickname);

            _nicknameInputField.text = LocalSettings.nickname;
        }

        private static void OnChangedNickname(string newNickName) => LocalSettings.nickname = newNickName;
    }
}