using DOTS.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace DOTS.Systems {
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<ZombieSpawnTimer>();
        }
        
        public void OnDestroy(ref SystemState state) {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            new SpawnZombieJob {
                deltaTime =  deltaTime,
                ecb =  ecb.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }
    
    [BurstCompile]
    public  partial struct SpawnZombieJob : IJobEntity {
        public float deltaTime;
        public EntityCommandBuffer ecb;
        
        [BurstCompile]
        void Execute(GraveyardAspect graveyard) {
            graveyard.zombieSpawnTimer -= deltaTime;
            
            if(!graveyard.timeToSpawnZombie) return;

            graveyard.zombieSpawnTimer = graveyard.zombieSpawnRate;
            var newZombie = ecb.Instantiate(graveyard.zombiePrefab);
            ecb.SetComponent(newZombie,graveyard.GetRandomTombstoneTransform());
        }
    }
}