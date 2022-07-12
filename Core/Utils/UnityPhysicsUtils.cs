using Agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterReborn.Utils
{
    public static class UnityPhysicsUtils
    {
        public static bool IsPostionVisible(Vector3 from, Vector3 to, out RaycastHit raycastHit)
        {
            Ray ray = default(Ray);
            ray.origin = from;
            Vector3 direction = to - from;
            float magnitude = direction.magnitude;
            direction.Normalize();
            ray.direction = direction;
            return !Physics.Raycast(ray, out raycastHit, magnitude, LayerManager.MASK_WORLD);
        }

        public static bool CanSee(this Agent from, GameObject targetObject)
        {
            RaycastHit raycastHit1;
            return from.EyePosition != null && (IsPostionVisible(from.EyePosition, targetObject.transform.position, out raycastHit1) || raycastHit1.transform.gameObject == targetObject || raycastHit1.transform.IsChildOf(targetObject.transform));
        }

    }
}
