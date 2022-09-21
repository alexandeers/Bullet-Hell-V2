using UnityEngine;
using UnityEngine.UI;

public class UIDash : MonoBehaviour
{

    [SerializeField] Image dashMeter;
    [SerializeField] Image dashMeterBackground;

    public void SetDashUI(float charges, float maxCharges, float recharge) {
        dashMeterBackground.material.SetFloat("_SegmentCount", maxCharges);
        dashMeter.material.SetFloat("_SegmentCount", maxCharges);

        dashMeterBackground.material.SetFloat("_RemovedSegments", maxCharges - (recharge + charges));
        dashMeter.material.SetFloat("_RemovedSegments", Mathf.Floor(maxCharges - charges));
    }
}
