using PixelCrew.Model.Definitions.Repositories.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repositories
{
    public class DefRepository<TDefType> : ScriptableObject where TDefType : IHaveId
    {
        [SerializeField] protected TDefType[] _collection;

        public TDefType Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return default;

            foreach (var itemDef in _collection)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }

            return default;
        }

        public TDefType[] All => new List<TDefType>(_collection).ToArray();

        public TDefType[] GetAll(params ItemTag[] tags)
        {
            var retValue = new List<TDefType>();
            foreach (var item in _collection)
            {
                var itemDef = DefsFacade.I.Items.Get(item.Id);
                var isAllRequirementsMet = tags.All(x => itemDef.HasTag(x));

                if (isAllRequirementsMet)
                {
                    retValue.Add(item);
                }
            }

            return retValue.ToArray();
        }
    }
}