using System.Collections;
using GLTFast.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class InitGame : MonoBehaviour
    {
        public Button start;
        public Button end;

        private void Start()
        {
            if (start)
            {
                start.onClick.AddListener(OnClickStart);
            }

            if (end)
            {
                end.onClick.AddListener(OnClickEnd);
            }
        }
        
        private void OnClickStart()
        {
            StartCoroutine(LoadGameScene());
        }

        private static void OnClickEnd()
        {
            Application.Quit();
        }

        private static IEnumerator LoadGameScene()
        {
            yield return  SceneManager.LoadSceneAsync("procedural");
        }
        
    }
}