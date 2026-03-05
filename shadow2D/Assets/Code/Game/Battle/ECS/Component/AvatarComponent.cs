
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class AvatarComponent : Component
    {
        private ECSGameObject m_avatarRoot = null;
        private ECSGameObject m_avatarBody = null;

        private Vector3 m_forward;
        private Vector3 m_position;

        private Dictionary<string, ECSBaseObject> m_DictEcsObjs = new Dictionary<string, ECSBaseObject>();

        public override void Init(Entity e) {
            base.Init(e);

            m_avatarRoot = ECSGameObject.GetAvatarRootObject();
            m_avatarBody = ECSGameObject.GetAvatarBodyObject();

            m_avatarBody.transform.parent = m_avatarRoot.transform;
             
            m_avatarRoot.OnActive();
            m_avatarBody.OnActive(); 

            m_DictEcsObjs.Add("Root", m_avatarRoot);
            m_DictEcsObjs.Add("Body", m_avatarBody);
        }

        public override void UnInit() {
            base.UnInit();

            foreach (var it in m_DictEcsObjs)
                it.Value.Destory();

            m_DictEcsObjs.Clear();
        }

        public ECSGameObject AvatarRoot { get { return m_avatarRoot; } }
        public ECSGameObject AvatarBody { get { return m_avatarBody; } }

        public Vector3 Forward{
            set { m_forward = value; AvatarBody.transform.forward = m_forward; }
            get { return m_forward; }
        }

        public Vector3 Position{
            set { m_position = value; AvatarRoot.transform.position = m_position; }
            get { return m_position; }
        }

        public Quaternion Rotation {
            set { AvatarBody.transform.rotation = value; }
        }
        public Vector3 eulerAngles{
            set { AvatarBody.transform.eulerAngles = value; }
        }

        public Dictionary<string, ECSBaseObject> EcsObjs { get { return m_DictEcsObjs; } }
    }
}
