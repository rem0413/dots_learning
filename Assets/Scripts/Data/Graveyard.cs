using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace DOTS.Data {
    public struct GraveyardData : IComponentData {
        public float2 fieldDimensions;
        public int numberTombstoneToSpawn;
        public Entity tombstonePrefab;
        public Entity zombiePrefab;
        public float zombieSpawnRate;
    }
    
    public struct ZombieSpawnTimer : IComponentData {
        public float value;
    }

    public class Graveyard : MonoBehaviour {
        public float2 fieldDimensions;
        public int numberTombstoneToSpawn;
        public GameObject tombstonePrefab;
        public GameObject zombiePrefab;
        public int zombieSpawnRate;
        public uint randomSeed;
    }

    public class GraveyardBaker : Baker<Graveyard> {
        public override void Bake(Graveyard authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent<GraveyardData>(entity, new GraveyardData {
                fieldDimensions = authoring.fieldDimensions,
                numberTombstoneToSpawn = authoring.numberTombstoneToSpawn,
                tombstonePrefab = GetEntity(authoring.tombstonePrefab,TransformUsageFlags.Dynamic),
                zombiePrefab = GetEntity(authoring.zombiePrefab,TransformUsageFlags.Dynamic),
                zombieSpawnRate = authoring.zombieSpawnRate
            });
            
            AddComponent<GraveyardRandom>(entity, new GraveyardRandom {
                value =  Random.CreateFromIndex(authoring.randomSeed)
            });

            AddComponent<ZombieSpawnPoints>(entity);
            AddComponent<ZombieSpawnTimer>(entity);
        }
    }
}