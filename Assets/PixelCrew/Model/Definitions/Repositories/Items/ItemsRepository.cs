using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories.Items
{
    [CreateAssetMenu(menuName = "Defs/Repositories/Items", fileName = "Items")]
    public class ItemsRepository : DefRepository<ItemDef>
    {     

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _collection;
#endif
    }


    [Serializable]
    public struct ItemDef : IHaveId
    {
        [SerializeField] private string _id;
        [SerializeField] private string _info;
        [SerializeField] private Sprite _icon;
        [SerializeField] private ItemTag[] _tags;
        [SerializeField] ItemWithCount _price;
        public string Id => _id;

        public bool IsVoid => string.IsNullOrEmpty(_id);
        public Sprite Icon => _icon;
        public string Info => _info;
        public ItemWithCount Price => _price;
        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }      
    }
}
