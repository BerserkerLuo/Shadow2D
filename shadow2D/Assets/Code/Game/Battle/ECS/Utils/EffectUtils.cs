using System.Collections;
using UnityEngine;

namespace ECS
{
    public class EffectUtils 
    {
        private static IEnumerator DestroyAfterDelay(ECSBaseObject obj, float ftime)
        {
            yield return new WaitForSeconds(ftime);
            obj.Destory();
        }

        public static void ShowEffect(string effectName, Vector3 pos, float freeTime = -1, float scale = 1)
        {
            ECSEffectObject effectObj = ECSEffectObject.GetByEffectName(effectName);
            if (effectObj == null)
                return;

            pos.z = 1;
            effectObj.transform.position = pos;
            effectObj.transform.localScale = new Vector3(scale, scale, scale);
            effectObj.OnActive();

            UnityGameEntry.Instance.StartCoroutine(DestroyAfterDelay(effectObj, freeTime));
        }

    }
}