
namespace ECS
{
    public class ECSGameObject : ECSBaseObject
    {
        static public ECSGameObject Get(string path) { return GetByPath<ECSGameObject>(path); }

        static public ECSGameObject GetAvatarRootObject() { return Get("Prefabs/Common/AvatarRootObject"); }
        static public ECSGameObject GetAvatarBodyObject() { return Get("Prefabs/Common/AvatarBodyObject"); }
        static public ECSGameObject GetHealthBarObject() { return Get("Prefabs/Common/HealthBar"); }

        private ModelComponent<HealthBar> m_HealthBar;
        public HealthBar HealthBarScript { get {
                if (m_HealthBar == null){
                    m_HealthBar = new ModelComponent<HealthBar>(transform, false);
                    m_HealthBar.Instance.Init();
                }
                return m_HealthBar.Instance;
            }
        }
    }
}
