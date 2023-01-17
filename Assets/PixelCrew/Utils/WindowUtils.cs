using UnityEngine;

namespace PixelCrew.Utils
{
    public static class WindowUtils
    {
        public static void CreateWindow(string resouecePath)
        {
            var window = Resources.Load<GameObject>(resouecePath);
            var canvas = GameObject.FindWithTag("MainUICanvas").GetComponent<Canvas>();
            Object.Instantiate(window, canvas.transform);
        }       
    }
}