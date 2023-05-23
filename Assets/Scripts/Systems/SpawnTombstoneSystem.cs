using DOTS.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS.Systems {
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnTombstoneSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<GraveyardData>();
        }

        public void OnDestroy(ref SystemState state) {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            state.Enabled = false;
            var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardData>();
            var graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var spawnPoint = new NativeList<float3>(Allocator.Temp);
            var tombstoneOffset = new float3(0, -2f, 1f);
            
            var builder = new BlobBuilder(Allocator.Temp);
            ref var spawnPoints = ref builder.ConstructRoot<ZombieSpawnPointsBlob>();
            var arrayBuilder = builder.Allocate(ref spawnPoints.Value, graveyard.numberTombstonesToSpawn);

            for (int i = 0; i < graveyard.numberTombstonesToSpawn; i++) {
                var newTombstone = ecb.Instantiate(graveyard.tombstonePrefab);
                var newTombstoneTransform = graveyard.GetRandomTombstoneTransform();
                ecb.SetComponent(newTombstone,newTombstoneTransform);

                var newSpawnPoint = newTombstoneTransform.Position + tombstoneOffset;
                arrayBuilder[i] = newSpawnPoint;
                spawnPoint.Add(newSpawnPoint);
            }
            
            // if(!graveyard.ZombieSpawnPointInitialized()) return;
            
            var blobAsset = builder.CreateBlobAssetReference<ZombieSpawnPointsBlob>(Allocator.Persistent);
            ecb.SetComponent(graveyardEntity, new ZombieSpawnPoints{value = blobAsset});
            builder.Dispose();
            
            ecb.Playback(state.EntityManager);
        }
    }
}