using UnityEngine;
using UnityEngine.UI;

public class UIDash : MonoBehaviour
{

    [SerializeField] Image dashMeter;
    [SerializeField] Image dashMeterBackground;
    [SerializeField] GameObject dashRemainder;

    public void SetDashUI(float charges, float maxCharges, float recharge) {
        dashMeterBackground.material.SetFloat("_SegmentCount", maxCharges);
        dashMeter.material.SetFloat("_SegmentCount", maxCharges);

        dashMeterBackground.material.SetFloat("_RemovedSegments", maxCharges - (recharge + charges));
        dashMeter.material.SetFloat("_RemovedSegments", Mathf.Floor(maxCharges - charges));

        dashMeter.color = Color.Lerp(dashMeter.color, Color.white, Time.deltaTime);
    }

    public void FlashUI() {
        dashMeter.color = new Color(0.2f, 0.35f, 0.88f, 1f);
    }

    public void UseDash(float charges, float maxCharges) {
        var remainder = Instantiate(dashRemainder, dashRemainder.transform.parent);
        remainder.GetComponent<RectTransform>().localScale = Vector3.one;

        Image dashRemainderImage = remainder.GetComponent<Image>();

        var zRotation = 180f + (charges * (360/maxCharges) ) - (360/maxCharges);
        remainder.GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, zRotation);
        dashRemainderImage.material.SetFloat("_RemovedSegments", maxCharges - 1f);
        dashRemainderImage.material.SetFloat("_SegmentCount", maxCharges);
        remainder.SetActive(true);
    }
}