using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.Windows.Localization
{
    public class LocalizationWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private LocaleItemWidjet _prefab;

        private DataGroup<LocaleInfo, LocaleItemWidjet> _dataGroup;

        private string[] _supportedLocales = new[] { "en", "ru" };

        protected override void Start()
        {
            base.Start();
            _dataGroup = new DataGroup<LocaleInfo, LocaleItemWidjet>(_prefab, _container);
            _dataGroup.SetData(ComposeData());
        }

        private List<LocaleInfo> ComposeData()
        {
            var data = new List<LocaleInfo>();
            foreach (var locale in _supportedLocales)
            {
                data.Add(new LocaleInfo { LocaleId = locale });
            }

            return data;
        }

        public void OnSelected(string selectedLocale)
        {
            LocalizationManager.I.SetLocale(selectedLocale);
        }
    }
}