using Unity.Entities;
using Unity.Mathematics;

namespace DOTS.Data {
    public struct GraveyardRandom: IComponentData {
        public Random value;
    }
}