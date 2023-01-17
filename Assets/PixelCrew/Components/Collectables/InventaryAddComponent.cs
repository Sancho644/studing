using UnityEngine;
using PixelCrew.Model.Data;
using PixelCrew.Utils;
using PixelCrew.Model.Definitions.Repositories.Items;

namespace PixelCrew.Components.Collectables
{
    public class InventaryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetInterface<ICanAddInInventory>();
            hero?.AddInInventary(_id, _count);
        }
    }
}
