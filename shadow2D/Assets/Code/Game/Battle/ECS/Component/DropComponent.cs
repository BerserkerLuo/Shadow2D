using System.Collections;
using UnityEngine;

namespace ECS
{
    public class DropComponent : Component
    {
        public int ItemId = 0;
        public bool IsInPickUp = false;
    }
}