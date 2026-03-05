
using System.Numerics;

namespace ECS
{
    public static class AvatarDataUtil
    {

        public static ECSBaseObject GetAvatarBody(Entity e) {
            AvatarComponent component = e.GetComponentData<AvatarComponent>();
            if (component == null)
                return null;

            return component.AvatarBody;
        }
        public static void AddModelToEntity(Entity e, string name, ECSBaseObject obj, bool isBindRoot = false)
        {
            if (obj == null)
                return;

            AvatarComponent component = e.GetComponentData<AvatarComponent>();
            if (component == null)
                return;

            if (isBindRoot)
                obj.transform.parent = component.AvatarRoot.transform;
            else
                obj.transform.parent = component.AvatarBody.transform;

            obj.OnActive(); 
            obj.transform.localPosition = UnityEngine.Vector3.zero;

            if (!component.EcsObjs.ContainsKey(name))
                component.EcsObjs.Add(name, obj);
        }

        public static void RemoveEntityEcsObj(Entity e, string name) {
            AvatarComponent component = e.GetComponentData<AvatarComponent>();
            if (component == null)
                return;

            ECSBaseObject retObj = null;
            if (!component.EcsObjs.TryGetValue(name, out retObj))
                return;

            retObj.Destory();
            component.EcsObjs.Remove(name);
        }

        public static ECSBaseObject GetEntityEcsObj(Entity e, string name)
        {
            AvatarComponent component = e.GetComponentData<AvatarComponent>();
            if (component == null)
                return null;

            ECSBaseObject retObj = null;
            if (!component.EcsObjs.TryGetValue(name, out retObj))
                return null;
            return retObj;
        }


        public static void AddMainModelToEntity(Entity e, ECSModelObject obj, bool isBindRoot = false) {
            if (obj == null)
                return;
             
            AddModelToEntity(e, "_Main_", obj, isBindRoot);

            if (obj.OwnerScript != null) obj.OwnerScript.SetOwner(e);
            else DebugUtils.LogError("OwnerScript Is Null !");
        }

        public static ECSModelObject GetEntityMainObj(Entity e){
            return  GetEntityEcsObj(e, "_Main_") as ECSModelObject;
        }

        public static void AddHealthBarToEntity(Entity e, int FactionId) {
            ECSGameObject HealthBarObj = ECSGameObject.GetHealthBarObject();
            HealthBarObj.HealthBarScript.SetFactionId(FactionId);
            HealthBarObj.HealthBarScript.SetFill(1);
            AddModelToEntity(e, "HealthBar", HealthBarObj, true);
        }

        public static ECSBaseObject GetHealthBar(Entity e) {
            return GetEntityEcsObj(e, "HealthBar");
        }

        public static void AddHealthBarToEntity(Entity e, ECSModelObject obj)
        {
            AddModelToEntity(e, "HealthBar", ECSGameObject.GetHealthBarObject(), true);
        }

        public static void AddWeaponToEntity(Entity e,string weaponName) {
            ECSModelObject weapon = ECSModelObject.GetByModelName(weaponName);
            if (weapon == null)
            {
                DebugUtils.Log("AddWeaponToEntity weapon == null");
                return;
            }

            DebugUtils.Log("AddWeaponToEntity 1");

            if (weapon.WeaponController != null)
            {
                DebugUtils.Log("AddWeaponToEntity SetOwner");
                weapon.WeaponController.SetOwner(e);
            }

            DebugUtils.Log("AddWeaponToEntity 2");

            AddModelToEntity(e, "Weapon", weapon, true);
        }

        public static ECSModelObject GetWeaponObj(Entity e)
        {
            return (ECSModelObject)GetEntityEcsObj(e, "Weapon");
        }

        public static void RemoveEntityWeapon(Entity e){
            RemoveEntityEcsObj(e, "Weapon");
        }

        public static void AddStatusEffectToEntity(Entity e,int statusId) { 

        }

        public static void RemoveEntityStatusEffect(Entity e, int statusId){

        }

    }
}
