using Unity.Burst;
using Unity.Entities;

namespace DOTS.Systems {
    [BurstCompile]
    public partial struct SpawnZombieSystem : ISystem {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {
            
        }
        
        public void OnDestroy(ref SystemState state) {
            
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            
        }
    }
    
    public  partial struct SpawnZombieJob : IJobEntity {
        
    }
}