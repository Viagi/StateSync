using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class BoxCollider : Collider
    {
        public Vector3 center;
        public Vector3 size;

        //public BoxCollider() {}

        //public BoxCollider(System.Object o)
        //:base(o)
        //{
        //	var c = o as UnityEngine.Config.BoxCollider;
        //	center = new Vector3(c.m_Center.x, c.m_Center.y, c.m_Center.z);
        //	size = new Vector3(c.m_Size.x, c.m_Size.y, c.m_Size.z);
        //}

        protected override bool DoRayCast(Ray ray, out RaycastHit hitInfo, float distance)
        {
            hitInfo = new RaycastHit();

            Vector3 dir = ray.direction;
            Vector3 org = ray.origin;

            Vector3 aabbHalfExtent = size * 0.5f;
            Vector3 aabbCenter = center;
            Vector3 rayFrom = org;
            if (distance > 100000)
                distance = 100000;
            Vector3 rayTo = org + dir * distance;
            rayFrom = new Vector3(org.x, org.y, org.z);
            rayTo = new Vector3(rayTo.x, rayTo.y, rayTo.z);
            distance = (rayTo - rayFrom).magnitude;

            Vector3 source = rayFrom - aabbCenter;
            Vector3 target = rayTo - aabbCenter;
            float param = 100;
            int sourceOutcode = Outcode(source, aabbHalfExtent);
            int targetOutcode = Outcode(target, aabbHalfExtent);

            if ((sourceOutcode & targetOutcode) == 0x0)
            {
                float lambda_enter = 0.0f;
                float lambda_exit = param;
                Vector3 r = target - source;
                int i;
                float normSign = 1;
                Vector3 hitNormal = Vector3.zero;
                int bit = 1;

                for (int j = 0; j < 2; j++)
                {
                    for (i = 0; i != 3; ++i)
                    {
                        if ((sourceOutcode & bit) != 0)
                        {
                            float lambda = (-source[i] - aabbHalfExtent[i] * normSign) / r[i];
                            if (lambda_enter <= lambda)
                            {
                                lambda_enter = lambda;
                                hitNormal.Set(0, 0, 0);
                                hitNormal[i] = normSign;
                            }
                        }
                        else if ((targetOutcode & bit) != 0)
                        {
                            float lambda = (-source[i] - aabbHalfExtent[i] * normSign) / r[i];
                            if (lambda < lambda_exit)
                            {
                                lambda_exit = lambda;
                            }
                        }
                        bit <<= 1;
                    }
                    normSign = -1.0f;
                }
                if (lambda_enter <= lambda_exit)
                {
                    param = lambda_enter;
                    hitInfo = new RaycastHit();
                    hitInfo.collider = this;
                    hitInfo.distance = param * distance;
                    hitInfo.point = rayFrom + (rayTo - rayFrom) * param;
                    hitInfo.point = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                    return true;
                }
            }
            return false;
        }

        int Outcode(Vector3 p, Vector3 halfExtent)
        {
            return (p.x < -halfExtent.x ? 0x01 : 0x0) |
                   (p.x > halfExtent.x ? 0x08 : 0x0) |
                   (p.y < -halfExtent.y ? 0x02 : 0x0) |
                   (p.y > halfExtent.y ? 0x10 : 0x0) |
                   (p.z < -halfExtent.z ? 0x4 : 0x0) |
                   (p.z > halfExtent.z ? 0x20 : 0x0);
        }

    }
}

