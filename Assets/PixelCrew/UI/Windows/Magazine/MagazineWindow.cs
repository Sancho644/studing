using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Windows.Magazine
{
    public class MagazineWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private ItemWidjet _price;
        [SerializeField] private Text _infoText;
        [SerializeField] private Transform _magazineContainer;

        private PredefinedDataGroup<ItemDef, MagazineItemWidjet> _dataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new PredefinedDataGroup<ItemDef, MagazineItemWidjet>(_magazineContainer);
            _session = GameSession.Instance;

            _trash.Retain(_session.MagazineItemsModel.Subscribe(OnMagazineChanged));

            OnMagazineChanged();
        }

        private void OnMagazineChanged()
        {
            _dataGroup.SetData(DefsFacade.I.Items.GetAll(ItemTag.Magazine));

            var selected = _session.MagazineItemsModel.InterfaceSelection.Value;
            _buyButton.interactable = _session.MagazineItemsModel.CanBuy(selected);

            var def = DefsFacade.I.Items.Get(selected);
            _price.SetData(def.Price);

            _infoText.text = LocalizationManager.I.Localize(def.Info);
        }

        public void OnBuy()
        {
            var selected = _session.MagazineItemsModel.InterfaceSelection.Value;
            _session.MagazineItemsModel.BuyItem(selected, 1);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
} 