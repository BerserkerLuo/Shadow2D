using Pathfinding;
using Table;
using Unity.VisualScripting;
using UnityEngine;

namespace ECS
{
    public class ECSModelObject : ECSBaseObject
    {
        static public ECSModelObject GetByModelName(string modelName) { return GetByPath<ECSModelObject>(GetModelPath(modelName)); }

        static public string GetModelPath(string modelName) {

            ProfebsModelCfg modelCfg = TableMgr.Singleton.GetProfebsModelCfg(modelName);
            if (modelCfg == null)
                return modelName;

            string modelPath = modelCfg.Path;
            return modelPath;
        }

        private ModelComponent<Seeker> m_seeker;
        private ModelComponent<RoleController> m_RoleController;
        private ModelComponent<OwnerScript> m_OwnerScript;
        private ModelComponent<WeaponController> m_WeaponController;

        public Seeker Seeker { get { return m_seeker.Instance; } }
        public RoleController RoleController { get { return m_RoleController.Instance; } }
        public OwnerScript OwnerScript { get { return m_OwnerScript.Instance; } }

        public WeaponController WeaponController { get { return m_WeaponController.Instance; } }

        private Vector3 _bulletPos = Vector3.zero;
        public Vector3 BulletPos { get { return _bulletPos; } }

        private Transform _Body;
        public Transform Body { get { return _Body; } }

        public override void AfterInit() {

            _Body = transform.Find("Body");
            if (_Body == null)
                _Body = transform;

            m_RoleController = new ModelComponent<RoleController>(_Body, false);
            m_seeker = new ModelComponent<Seeker>(_Body, false);
            m_OwnerScript = new ModelComponent<OwnerScript>(_Body);
            m_WeaponController = new ModelComponent<WeaponController>(transform, false);

            Transform bulletPos = transform.Find("BulletPos");
            if(bulletPos != null) _bulletPos = bulletPos.localPosition;
        }
    }
}
