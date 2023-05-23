using DOTS.Data;
using Unity.Burst;
using Unity.Entities;

namespace DOTS.Systems {
    [BurstCompile]
    [UpdateAfter(typeof(SpawnZombieSystem))]
    public partial struct ZombieRiseSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            
        }

        public void OnDestroy(ref SystemState state) {
            
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            var deltaTime = SystemAPI.Time.DeltaTime;
            new ZombieRiseJob {
                deltaTime = deltaTime
            }.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public  partial struct ZombieRiseJob : IJobEntity {
        public float deltaTime;
        [BurstCompile]
        void Execute(ZombieRiseAspect zombie) {
            zombie.Rise(deltaTime);
        }
    }
}