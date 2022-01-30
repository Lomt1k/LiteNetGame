using Networking;
using Project.UI.Windows;
using Project.UI.Windows.StartGameWindow;
using UnityEngine;

namespace Project
{
    public class MainScript : MonoBehaviour
    {
        private void Start()
        {
            #if SERVER_ONLY
            NetStarter.TryStartServer(5555, 1000);
            #else
            WindowsManager.CreateWindow<StartGameWindow>();
            #endif
        }
    }
}
