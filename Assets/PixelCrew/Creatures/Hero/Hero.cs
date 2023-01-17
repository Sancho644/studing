using PixelCrew.Components.ColliderBased;
using PixelCrew.Components.Health;
using PixelCrew.Utils;
using PixelCrew.Model;
using PixelCrew.Components;
using PixelCrew.Model.Data;
using UnityEngine;
using UnityEditor.Animations;
using System.Collections;
using PixelCrew.Components.GoBased;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories.Items;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Model.Definitions.Player;
using PixelCrew.Effects.CameraRelated;

namespace PixelCrew.Creatures.Hero
{
    public class Hero : Creature, ICanAddInInventory
    {
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private LayerCheck _wallCheck;

        [SerializeField] private float _fallDownVelocity;
        [SerializeField] private Cooldown _throwCooldown;
        [SerializeField] private Cooldown _dashCooldown;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Header("Super throw")]
        [SerializeField] private Cooldown _superThrowCooldown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;
        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private ProbabillityDropComponent _hitDrop;
        [SerializeField] private GameObject _candle;
        [SerializeField] private ShieldComponent _shield;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        private bool _allowDoubleJump;
        private bool _superThrow;
        private bool _isOnWall;
        private bool _dashCheck;

        private Animator _animator;
        private GameSession _session;
        private HealthComponent _health;
        private CameraShakeEffect _cameraShake;
        private float _defaultGravityScale;

        private const string SwordId = "Sword";
        private int CoinsCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);

        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Trowable);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            _defaultGravityScale = Rigidbody.gravityScale;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _cameraShake = FindObjectOfType<CameraShakeEffect>();
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();

            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.StatsModel.OnUpgraded += OnHeroUpgraded;

            _health.SetHealth(_session.Data.Hp.Value);
            UpdateHeroWeapon();
        }

        private void OnHeroUpgraded(StatId statId)
        {
            switch (statId)
            {
                case StatId.Hp:
                    var health = (int)_session.StatsModel.GetValue(statId);
                    _session.Data.Hp.Value = health;
                    _health.SetHealth(health);
                    break;
            }
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == SwordId)
                UpdateHeroWeapon();
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
        }

        protected override void Update()
        {
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;

            if (_wallCheck.IsTochingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(IsOnWall, _isOnWall);
        }

        protected override float CalculateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall)
            {
                _allowDoubleJump = true;
            }
            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }
            if (_isOnWall)
            {
                return 0f;
            }

            return base.CalculateYVelocity();
        }

        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall)
            {
                _session.PerksModel.Cooldown.Reset();
                _allowDoubleJump = false;
                DoJumpVfx();
                return _jumpSpeed;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }

        public void AddInInventary(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            _cameraShake.Shake();
            if (CoinsCount > 0)
            {
                SpawnCoins();
            }
        }

        private void SpawnCoins()
        {
            var numCoinsToDispose = Mathf.Min(CoinsCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDispose);

            _hitDrop.SetCount(numCoinsToDispose);
            _hitDrop.CalculateDrop();
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.IsInLayer(_groundLayer))
            {
                var contact = other.contacts[0];
                if (contact.relativeVelocity.y >= _fallDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            var state = _animator.runtimeAnimatorController.name.Contains("_unarmed");
            if (SwordCount <= 0 || state) return;

            base.Attack();
        }

        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
        }

        public void OnDoThrow()
        {
            if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;

                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                _session.PerksModel.Cooldown.Reset();
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }

            _superThrow = false;
        }

        protected override float CalculateXVelocity()
        {
            var mod = _dashCheck ? 10 : 1;

            if (_dashCooldown.IsReady)
            {
                return base.CalculateXVelocity();
            }
            else
                return base.CalculateXVelocity() * mod;
        }

        public void OnDoDash(bool dash)
        {
            if (_session.PerksModel.IsDashSupported)
            {
                _session.PerksModel.Cooldown.Reset();
                _dashCooldown.Reset();
                _dashCheck = dash;
            }
        }

        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }

        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            _throwSpawner.SetPrebaf(throwableDef.Projectile);
            var instance = _throwSpawner.SpawnInstance();
            ApplyRangeDamageStat(instance);

            _session.Data.Inventory.Remove(throwableId, 1);
        }

        private void ApplyRangeDamageStat(GameObject projectile)
        {
            var hpModify = projectile.GetComponent<ModifyHealthComponent>();
            var damageValue = (int)_session.StatsModel.GetValue(StatId.RangeDamage);
            damageValue = ModifyDamageByCrit(damageValue);
            hpModify.SetDelta(-damageValue);
        }

        private int ModifyDamageByCrit(int damage)
        {
            var critChance = _session.StatsModel.GetValue(StatId.CriticalDamage);
            if (Random.value * 100 <= critChance)
            {
                return damage * 2;
            }

            return damage;
        }    

        public void StartThowing()
        {
            _superThrowCooldown.Reset();
        }

        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Trowable))
            {
                PerformThrowing();
            }
            else if (IsSelectedItem(ItemTag.Potion))
            {
                UsePotion();
            }
        }

        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Effect.AddHp:
                    var health = (int)potion.Value + _session.Data.Hp.Value;
                    _session.Data.Hp.Value = Mathf.Min(health, (int)_session.StatsModel.GetValue(StatId.Hp));
                    _health.SetHealth(_session.Data.Hp.Value);
                    break;
                case Effect.SpeedUp:
                    _speedUpCooldown.Value = +_speedUpCooldown.RemainingTime + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }

            _session.Data.Inventory.Remove(potion.Id, 1);
        }

        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
        }

        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;

        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }

        private void PerformThrowing()
        {
            if (!_throwCooldown.IsReady || !CanThow) return;

            if (_superThrowCooldown.IsReady) _superThrow = true;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }

        public void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }

        public void EnableCandle()
        {
            if (_candle.activeSelf)
                _candle.SetActive(false);
            else
                _candle.SetActive(true);
        }

        public void UsePerk()
        {
            if (_session.PerksModel.IsShieldSupported)
            {
                _shield.Use();
                _session.PerksModel.Cooldown.Reset();
            }
        }
    }
}