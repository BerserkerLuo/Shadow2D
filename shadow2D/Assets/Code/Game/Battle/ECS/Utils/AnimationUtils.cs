
using ECS;
using UnityEngine;
using DG.Tweening;

namespace ECS
{
    class AnimationUtil
    {
        public static RoleController GetRoleController(Entity e)
        {
            if (e == null)
                return null;

            ECSModelObject obj = AvatarDataUtil.GetEntityMainObj(e);
            if (obj == null)
                return null;

            return obj.RoleController;
        }

        public static WeaponController GetWeaponController(Entity e)
        {
            if (e == null)
                return null;

            ECSModelObject obj = AvatarDataUtil.GetWeaponObj(e);
            if (obj == null)
                return null;

            return obj.WeaponController;
        }

        public static void OnBron(Entity e) {
            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                roleController.OnBron();
        }

        public static void Attack(Entity e,Vector2 dire)
        {
            WeaponController weaponController = GetWeaponController(e);
            if (weaponController != null)
            {
                weaponController.OnAtk();
                return;
            }

            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                roleController.OnShowAtkAnimation(dire);
        }

        public static void StopMove(Entity e){
            Walk(e, 0);
            SetWeaponMoveFlag(e,false);
        }
        public static void Walk(Entity e,float speed = 3.0f)
        {
            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                roleController.OnShowMoveAnimation(speed);
            SetWeaponMoveFlag(e, true);
        }

        public static void OnDead(Entity e) {
            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                roleController.OnShowDeadAnimation();

        }

        public static void SetWeaponMoveFlag(Entity e,bool flag) {
            ECSModelObject weapon = AvatarDataUtil.GetWeaponObj(e);
            if (weapon != null && weapon.WeaponController != null)
                weapon.WeaponController.SetIsMove(flag);
        }


        public static void SetTargetDire(Entity e,Vector2 dire){
            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                    roleController.SetTargetDire(dire);
        }

        public static void SetWeaponTarget(Entity e,Entity target) {
            ECSModelObject weapon = AvatarDataUtil.GetWeaponObj(e);
            if (weapon != null && weapon.WeaponController != null)
                weapon.WeaponController.SetTarget(target);
        }

        public static void SetMoveDire(Entity e,Vector2 dire)
        {
            //Debug.Log("SetMoveDire Eid" + e.Eid  + " "+ dire);

            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                roleController.SetMoveDire(dire);

            ECSModelObject weapon = AvatarDataUtil.GetWeaponObj(e);
            if (weapon != null && weapon.WeaponController != null)
                weapon.WeaponController.SetMoveDire(dire);
        }

        public static void OnHit(Entity e) {
            RoleController roleController = GetRoleController(e);
            if (roleController != null)
                roleController.OnHit();
        }
    }
}