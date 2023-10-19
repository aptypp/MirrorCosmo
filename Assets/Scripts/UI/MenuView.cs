using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MomoCoop.UI
{
    public sealed class MenuView : MonoBehaviour
    {
        [SerializeField]
        private Button _hostButton;
        [SerializeField]
        private Button _joinButton;
        [SerializeField]
        private TMP_InputField _addressInputField;

        private GameServer _gameServer;
        private GameClient _gameClient;

        private void Awake()
        {
            _gameServer = FindObjectOfType<GameServer>();
            _gameClient = FindObjectOfType<GameClient>();

            _hostButton.onClick.AddListener(_gameServer.StartServer);
            _joinButton.onClick.AddListener(() => _gameClient.ConnectToServer(_addressInputField.text));
        }
    }
}