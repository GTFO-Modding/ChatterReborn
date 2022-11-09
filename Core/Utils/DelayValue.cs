namespace ChatterReborn.Utils
{
    public struct DelayValue
    {

        public static implicit operator DelayValue(MinMaxTimer val)
        {
            return new DelayValue(val);
        }

        public static implicit operator DelayValue(float val)
        {
            return new DelayValue(val);
        }

        public static implicit operator float(DelayValue val)
        {
            return val.totalTimer;
        }


        public DelayValue(float timer)
        {
            totalTimer = timer;
        }

        public DelayValue(MinMaxTimer minMaxTimer)
        {
            totalTimer = UnityEngine.Random.Range(minMaxTimer.min, minMaxTimer.max);
        }

        public float totalTimer;
    }
}
