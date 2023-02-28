﻿using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShowWindowComponent : MonoBehaviour
    {
        [SerializeField] private string _path;

        public void Show()
        {
            WindowUtils.CreateWindow(_path);
        }

        public void ShowIt(string path)
        {
            WindowUtils.CreateWindow(path);
        }
    }
}