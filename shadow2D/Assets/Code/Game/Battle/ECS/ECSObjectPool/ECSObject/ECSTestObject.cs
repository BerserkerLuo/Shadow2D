
using UnityEngine;

namespace ECS
{
    public class ECSTestObject : ECSBaseObject
    {
        static public ECSTestObject Get(string path) { return GetByPath<ECSTestObject>(path); }

        LineRenderer line = null;
        bool IsGetLineRenderer = false;
        public LineRenderer LineRenderer {
            get {
                if (IsGetLineRenderer)
                    return line;
                line = transform.GetComponent<LineRenderer>();
                return line;
            }
        }
    }
}
