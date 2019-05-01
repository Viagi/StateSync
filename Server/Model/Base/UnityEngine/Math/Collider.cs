using System;
using System.Collections.Generic;


// A base class of all colliders.
namespace UnityEngine
{

    public class Collider
    {
        //public bool enabled
        //{
        //	get {
        //		return GetEnabled();
        //	}
        //	set {
        //		SetEnabled(value);
        //	}
        //}

        //public Collider() {}
        //      public Collider(System.Object o)
        //      {
        //          var c = o as UnityEngine.Config.Collider;
        //          //m_Enabled = c.m_Enabled == 1;
        //      }

        //internal override void AwakeFromLoad (AwakeFromLoadMode awakeMode)
        //{
        //	base.AwakeFromLoad(awakeMode);
        //	UpdateColliderVisible(ShouldBeInScene());
        //}

        //internal override void Deactivate()
        //{
        //	UpdateColliderVisible(false);
        //	base.Deactivate ();
        //}

        //bool m_Enabled = true;
        //protected bool inScene;

        //bool ShouldBeInScene()
        //{
        //	return m_Enabled && IsActive();
        //}

        //internal void SetEnabled(bool v)
        //{
        //	m_Enabled = v;
        //	bool shouldBeInScene = ShouldBeInScene();
        //	if (shouldBeInScene == inScene) return;
        //	UpdateColliderVisible(shouldBeInScene);
        //}

        //internal bool GetEnabled()
        //{
        //	return m_Enabled;
        //}

        //void UpdateColliderVisible(bool visible)
        //{
        //	if( visible )
        //	{
        //		if (!inScene)
        //		{
        //			Application.GetCollisionWorld().Add(this);
        //			inScene = true;
        //		}
        //	}
        //	else
        //	{
        //		if (inScene)
        //		{
        //			Application.GetCollisionWorld().Remove(this);
        //			inScene = false;
        //		}
        //	}
        //}

        public bool isTrigger;
        /*
        // The rigidbody the collider is attached to.
        AUTO_PTR_PROP Rigidbody attachedRigidbody GetRigidbody

        // Is the collider a trigger?
        AUTO_PROP bool isTrigger GetIsTrigger SetIsTrigger

        // The material used by the collider.
        CUSTOM_PROP PhysicMaterial material
        {
            PhysicMaterial* material = self->GetMaterial ();
            PhysicMaterial* instance = &PhysicMaterial::GetInstantiatedMaterial (material, *self);
            if (instance != material)
                self->SetMaterial (instance);
            return Scripting::ScriptingWrapperFor (instance);
        }
        {
            self->SetMaterial (value);
        }

        // The closest point to the bounding box of the attached collider.
        CUSTOM Vector3 ClosestPointOnBounds (Vector3 position)
        {
            float dist; Vector3f outpos;
            self->ClosestPointOnBounds(position, outpos, dist);
            return outpos;
        }

        // The shared physic material of this collider.
        CUSTOM_PROP PhysicMaterial sharedMaterial { return Scripting::ScriptingWrapperFor (self->GetMaterial ()); } { self->SetMaterial (value); }

        // The world space bounding volume of the collider.
        AUTO_PROP Bounds bounds GetBounds
        */

        // Casts a [[Ray]] that ignores all Colliders except this one.

        public virtual bool Raycast(Ray ray, out RaycastHit hitInfo, float distance)
        {
            bool didHit = DoRayCast(ray, out hitInfo, distance);
            if (didHit)
            {
                hitInfo.collider = this;
            }
            return didHit;
        }

        protected virtual bool DoRayCast(Ray ray, out RaycastHit hitInfo, float distance)
        {
            hitInfo = new RaycastHit();
            return false;
        }

        /*
            // OnTriggerEnter is called when the [[Collider]] /other/ enters the [[class-BoxCollider|trigger]].
            CSNONE void OnTriggerEnter (Collider other);

            // OnTriggerExit is called when the [[Collider]] /other/ has stopped touching the [[class-BoxCollider|trigger]].
            CSNONE void OnTriggerExit (Collider other);

            // OnTriggerStay is called ''almost'' all the frames for every [[Collider]] __other__ that is touching the [[class-BoxCollider|trigger]].
            CSNONE void OnTriggerStay (Collider other);

            // OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.

            CSNONE void OnCollisionEnter (Collision collisionInfo);

            // OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
            CSNONE void OnCollisionExit (Collision collisionInfo);

            // OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
            CSNONE void OnCollisionStay (Collision collisionInfo);
        */
    }
}
