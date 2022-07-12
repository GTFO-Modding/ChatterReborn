using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Data
{
    public struct FixedVector3
    {
        public FixedVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator FixedVector3(Vector3 v)
        {
            return new FixedVector3(v.x, v.y, v.z);
        }

        public static implicit operator Vector3(FixedVector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public float x;
        public float y;
        public float z;
    }
}
