using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace DOTS.Data {
    public readonly partial struct GraveyardAspect : IAspect {
        public readonly Entity entity;
        private readonly RefRO<LocalTransform> _transform;
        private readonly RefRO<GraveyardData> _graveyardData;
        private readonly RefRW<GraveyardRandom> _graveyardRandom;
        private readonly RefRW<ZombieSpawnPoints> _zombieSpawnPoints;
        private readonly RefRW<ZombieSpawnTimer> _zombieSpawnTimer;

        public int numberTombstonesToSpawn => _graveyardData.ValueRO.numberTombstoneToSpawn;
        public Entity tombstonePrefab => _graveyardData.ValueRO.tombstonePrefab;
        public Entity zombiePrefab => _graveyardData.ValueRO.zombiePrefab;
        public LocalTransform transform => _transform.ValueRO;
        
        public bool ZombieSpawnPointInitialized()
        {
            return _zombieSpawnPoints.ValueRO.value.IsCreated && ZombieSpawnPointCount > 0;
        }

        private int ZombieSpawnPointCount => _zombieSpawnPoints.ValueRO.value.Value.Value.Length;

        float3 halfDimension => new() {
            x = _graveyardData.ValueRO.fieldDimensions.x * 0.5f,
            y = 0,
            z = _graveyardData.ValueRO.fieldDimensions.y * 0.5f
        };
        
        float3 minCorner => transform.Position - halfDimension;
        float3 maxCorner => transform.Position + halfDimension;
        
        float3 GetRandomPosition() {
            float3 randomPosition = _graveyardRandom.ValueRW.value.NextFloat3(minCorner, maxCorner);
            return randomPosition;
        }

        public LocalTransform GetRandomTombstoneTransform() {
            return new LocalTransform {
                Position = GetRandomPosition(),
                Rotation = GetRandomTombstoneRotation(),
                Scale = GetRandomTombstoneScale()
            };
        }

        float GetRandomTombstoneScale() {
            return _graveyardRandom.ValueRW.value.NextFloat(0.8f, 1.2f);
        }

        quaternion GetRandomTombstoneRotation() {
            return quaternion.RotateY(_graveyardRandom.ValueRW.value.NextFloat(-0.25f, 0.25f));
        }

        public float zombieSpawnTimer {
            get => _zombieSpawnTimer.ValueRO.value;
            set => _zombieSpawnTimer.ValueRW.value = value;
        }

        public bool timeToSpawnZombie => zombieSpawnTimer < 0f;
        public float zombieSpawnRate => _graveyardData.ValueRO.zombieSpawnRate;

        public LocalTransform GetZombieSpawnPoint() {
            return new LocalTransform {
                Position = GetRandomZombieSpawnPoint(),
                Rotation = quaternion.identity,
                Scale = 1f
            };
        }

        float3 GetRandomZombieSpawnPoint() {
            return _zombieSpawnPoints.ValueRO.value.Value.Value[
                _graveyardRandom.ValueRW.value.NextInt(ZombieSpawnPointCount)];
        }
    }
}