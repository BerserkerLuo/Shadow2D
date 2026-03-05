
using UnityEngine;
namespace ECS
{
    public class ECSUIObject : ECSBaseObject
    {
        static public ECSUIObject Get(string path) { return GetByPath<ECSUIObject>(path); }

        public RectTransform RectTransform;
        public override void AfterInit(){
            RectTransform = gameObject.GetComponent<RectTransform>();
        }
    }
}
