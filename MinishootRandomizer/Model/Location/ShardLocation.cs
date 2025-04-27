using UnityEngine;

namespace MinishootRandomizer
{
    public class ShardLocation: Location
    {
        private readonly Vector3 _position;

        public Vector3 Position => _position;

        public ShardLocation(
            string identifier,
            string logicRule,
            LocationPool pool,
            Vector3 position
        ): base(identifier, logicRule, pool)
        {
            _position = position;
        }

        public override IPatchAction Accept(ILocationVisitor visitor, Item item)
        {
            return visitor.VisitShard(this, item);
        }
    }

    public class ShardLocationReplacementData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public ShardLocationReplacementData(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
