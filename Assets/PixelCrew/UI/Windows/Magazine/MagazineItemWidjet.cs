using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Magazine
{
    public class MagazineItemWidjet : MonoBehaviour, IItemRenderer<ItemDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isSelected;

        private GameSession _session;
        private ItemDef _data;

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
      
            UpdateView();
        }

        public void SetData(ItemDef data, int index)
        {
            _data = data;

            if (_session != null)
                UpdateView();
        }

        private void UpdateView()
        {
            _icon.sprite = _data.Icon;

            _isSelected.SetActive(_session.MagazineItemsModel.InterfaceSelection.Value == _data.Id);
        }

        public void OnSelect()
        {
            _session.MagazineItemsModel.InterfaceSelection.Value = _data.Id;
        }
    }
}