namespace ChatterReborn.Utils
{
    public struct WeightValue<V>
    {
        public V Value;

        public float Weight;

        public WeightValue(V value, float weight)
        {
            this.Weight = weight;
            this.Value = value;
        }
    }
}
