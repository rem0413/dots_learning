using Unity.Entities;
using UnityEngine;

namespace DOTS.Data {
    public struct ZombieRiseRate : IComponentData {
        public float value;
    }

    public class Zombie : MonoBehaviour {
        public float zombieRiseRate;
    }

    public class ZombieBaker : Baker<Zombie> {
        public override void Bake(Zombie authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<ZombieRiseRate>(entity, new ZombieRiseRate {
                value =  authoring.zombieRiseRate
            });
        }
    }
}