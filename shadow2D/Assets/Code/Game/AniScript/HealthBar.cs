using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image FillGreenImage;
    Image FillRedImage;
    Image FillWhiteImage;
    
    float whiteValue = 1;
    Image FillImage;

    TextMeshProUGUI text;

    public void FixedUpdate()
    {
        if (FillWhiteImage.fillAmount - whiteValue == 0f)
            return;

        FillWhiteImage.fillAmount = Mathf.Lerp(FillWhiteImage.fillAmount,whiteValue,0.1f);
    }

    public void Init()
    {
        if (FillRedImage == null)
            FillRedImage = TryGetImage("FillRed");

        if (FillWhiteImage == null)
            FillWhiteImage = TryGetImage("FillWhite");

        if(FillGreenImage == null)
            FillGreenImage = TryGetImage("FillGreen");

        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private Image TryGetImage(string TransName) {
        Transform fillTrans = FindChildRecursive(transform, TransName);
        if (fillTrans != null)
            return fillTrans.GetComponent<Image>();
        return null;
    }

    Transform FindChildRecursive(Transform parent, string name){
        foreach (Transform child in parent){
            if (child.name == name)
                return child;

            Transform found = FindChildRecursive(child, name);
            if (found != null)
                return found;
        }
        return null;
    }

    public void SetFactionId(int factionId) {
        if (factionId == 1){
            FillImage = FillGreenImage;
            FillRedImage.gameObject.SetActive(false);
        }
        else{
            FillImage = FillRedImage;
            FillGreenImage.gameObject.SetActive(false);
        }

        FillImage.gameObject.SetActive(true);
    }

    public void SetFill(float fill) {
        FillImage.fillAmount = fill;
        CoroutineRunner.Instance.StartCoroutine(WhiteDelay(fill));
    }

    private IEnumerator WhiteDelay(float fill) {
        yield return new WaitForSeconds(0.3f);
        whiteValue = fill;
    }

    public void SetName(string Name) {
        if (text != null)
            text.text = Name;
    }
}
