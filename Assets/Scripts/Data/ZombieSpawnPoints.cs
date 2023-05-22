using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace DOTS.Data {
    public struct ZombieSpawnPointsBlob
    {
        public BlobArray<float3> Value;
    }
    
    public struct ZombieSpawnPoints : IComponentData {
        public BlobAssetReference<ZombieSpawnPointsBlob> value;
    }
 
}