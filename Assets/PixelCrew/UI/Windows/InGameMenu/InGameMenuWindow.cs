using PixelCrew.Components.LevelManagement;
using PixelCrew.Model;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private ReloadLevelComponent _reloadScene;

        protected override void Start()
        {
            base.Start();

            _reloadScene = GetComponent<ReloadLevelComponent>();
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }

        public void OnRestart()
        {
            _reloadScene.Reload();
            Close();
        }

        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
            Close();
        }

        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
            Close();
        }

        public void OnControls()
        {
            WindowUtils.CreateWindow("UI/HandlingsPC");
        }

        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");

            var session = GameSession.Instance;
            Destroy(session.gameObject);
        }

        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}