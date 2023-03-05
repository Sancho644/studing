using UnityEngine;
using PixelCrew.Model.Data;
using UnityEngine.SceneManagement;
using PixelCrew.Utils.Disposables;
using System.Collections.Generic;
using PixelCrew.Components.LevelManagement;
using System.Linq;
using PixelCrew.Model.Models;
using Assets.PixelCrew.Model.Models;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckPoint;

        public static GameSession Instance { get; private set; }

        public PlayerData Data => _data;
        private PlayerData _save;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        public QuickInventoryModel QuickInventory { get; private set; }

        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }
        public ShopItemsModel MagazineItemsModel { get; private set; }

        private readonly List<string> _checkpoints = new List<string>();

        private void Awake()
        {
            var existsSession = GetExistsSession();
            if (existsSession != null)
            {
                existsSession.StartSession(_defaultCheckPoint);
                DestroyImmediate(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(_defaultCheckPoint);
            }
        }

        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);

            LoadHud();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckPoint = _checkpoints.Last();

            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == lastCheckPoint)
                {
                    checkPoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);

            MagazineItemsModel = new ShopItemsModel(_data);
            _trash.Retain(MagazineItemsModel);

            _data.Hp.Value = (int)StatsModel.GetValue(StatId.Hp);
            var perkUsed = PerksModel.Used;
            var perkCooldown = DefsFacade.I.Perks.Get(perkUsed);
            PerksModel.Cooldown.Value = perkCooldown.Cooldown;
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }

            return null;
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();

            _trash.Dispose();
            InitModels();
        }

        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }

        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                Save();
                _checkpoints.Add(id);
            }
        }

        public void SetRemovedItems()
        {
            for (int i = 0; i < _removedItems.Count; i++)
            {
                _saveRemovedItem.Add(_removedItems[i]);
            }
        }

        private List<string> _removedItems = new List<string>();
        private List<string> _saveRemovedItem = new List<string>();

        public bool RestoreState(string itemId)
        {
            return _saveRemovedItem.Contains(itemId);
        }

        public void StoreState(string itemId)
        {
            if (!_removedItems.Contains(itemId))
                _removedItems.Add(itemId);
        }

        private void OnDestroy()
        {
            if (Instance == null)
                Instance = null;

            _trash.Dispose();
        }
    }
}