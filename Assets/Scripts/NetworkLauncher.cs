using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkLauncher : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "MainScene";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartAsHost()
    {
        if (NetworkManager.Singleton.StartHost())
        {
            Debug.Log("Хост запущен");
            NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);
        }
        else Debug.LogError("Ошибка запуска хоста");
    }

    public void StartAsClient()
    {
        if (NetworkManager.Singleton.StartClient())
            Debug.Log("Клиент запущен");
        else
            Debug.LogError("Ошибка запуска клиента");
    }
}