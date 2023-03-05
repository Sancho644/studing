using UnityEngine;
using System;

namespace PixelCrew.Components.GoBased
{
    public class SpawnListComponent : MonoBehaviour
    {
        [SerializeField] private SpawnData[] _spawners;

        public void SpawnAll()
        {
            foreach (var spawnData in _spawners)
            {
                spawnData.Component.Spawn();
            }
        }

        public void Spawn(string id)
        {
            foreach (var element in _spawners)
            {
                if (element.Id == id)
                {
                    element.Component.Spawn();
                }
            }
        }

        [Serializable]
        public class SpawnData
        {
            public string Id;
            public SpawnComponent Component;
        }
    }
}