using System.Collections;
using System.Collections.Generic;
using Project.UI.Windows;
using Project.UI.Windows.StartGameWindow;
using UnityEngine;

namespace Project
{
    public class MainScript : MonoBehaviour
    {
        private void Start()
        {
            WindowsManager.CreateWindow<StartGameWindow>();
        }
    }
}
