using PixelCrew.Model;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud
{
    public class CurrentCoinsWidjet : MonoBehaviour
    {
        [SerializeField] private Text _coinsValue;
        [SerializeField] private Text _gemsValue;

        private GameSession _session;

        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.Inventory.OnChanged += OnChangedCoinsValue;

            SetCoinsValue();
        }

        private void OnChangedCoinsValue(string id, int value)
        {
            var coins = "Coin";
            var gems = "GreenGem";
            if (id == coins)
                _coinsValue.text = value.ToString();
            if (id == gems)
                _gemsValue.text = value.ToString();
        }

        private void SetCoinsValue()
        {
            _coinsValue.text = _session.Data.Inventory.Count("Coin").ToString();
            _gemsValue.text = _session.Data.Inventory.Count("GreenGem").ToString();
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnChangedCoinsValue;
        }
    }
}