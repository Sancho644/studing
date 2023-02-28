using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using System;

namespace PixelCrew.Model.Models
{
    public class MagazineItemsModel : IDisposable
    {
        private readonly PlayerData _data;
        public readonly StringProperty InterfaceSelection = new StringProperty();

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public event Action OnChanged;

        public MagazineItemsModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Items.All[1].Id;

            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void BuyItem(string id, int value)
        {
            var def = DefsFacade.I.Items.Get(id);
            var isEnoughResources = _data.Inventory.IsEnough(def.Price);

            if (isEnoughResources)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Inventory.Add(id, value);
            }
        }

        public bool CanBuy(string itemId)
        {
            var def = DefsFacade.I.Items.Get(itemId);
            return _data.Inventory.IsEnough(def.Price);
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}