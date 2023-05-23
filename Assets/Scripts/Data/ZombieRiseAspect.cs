using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTS.Data {
    public readonly partial struct ZombieRiseAspect : IAspect {
        public readonly Entity entity;
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<ZombieRiseRate> _zombieRiseRate;
        
        public void Rise(float delTime) {
            _transform.ValueRW.Position += math.up() * _zombieRiseRate.ValueRO.value * delTime;
        }
        
        public bool IsAboveGround => _transform.ValueRO.Position.y >= 0f;
        
    }
}