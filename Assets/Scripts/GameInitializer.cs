using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MomoCoop
{
    public sealed class GameInitializer : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            yield return SceneManager.UnloadSceneAsync(0);
        }
    }
}