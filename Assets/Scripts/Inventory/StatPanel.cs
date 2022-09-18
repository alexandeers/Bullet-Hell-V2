using UnityEngine;

public class StatPanel : MonoBehaviour
{

    [SerializeField] StatDisplay[] statDisplays;
    [SerializeField] string[] statNames;

    CharacterStat[] stats;

    void OnValidate() {
        statDisplays = GetComponentsInChildren<StatDisplay>();
    }

    public void SetStats(params CharacterStat[] characterStats) {
        stats = characterStats;

        if(stats.Length > statDisplays.Length) {
            Debug.LogError("Not enough Stat Displays");
            return;
        } 

        for (int i = 0; i < statDisplays.Length; i++) {
            statDisplays[i].gameObject.SetActive(i < stats.Length);
        }
    }

    public void UpdateStatValues() {
        for (int i = 0; i < stats.Length; i++)
        {
            statDisplays[i].valueText.text = stats[i].Value.ToString();
        }
    }

}
