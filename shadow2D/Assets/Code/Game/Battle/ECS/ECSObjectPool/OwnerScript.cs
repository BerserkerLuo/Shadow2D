
using ECS;
using UnityEngine;

public class OwnerScript : MonoBehaviour
{
    private Entity m_owner = null;
    public int EId = 0;

    public Entity Owner { get { return m_owner; } }

    public void SetOwner(Entity e) {
        m_owner = e;
        EId = e.Eid;
    }
}

