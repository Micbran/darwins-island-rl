using UnityEngine;
using UnityEngine.UI;

public class FloorDisplay : MonoBehaviour
{
    [SerializeField] private Text floorValue;

    public void OnLevelGenerationComplete()
    {
        this.floorValue.text = GameManager.Instance.Floor <= 6 ? GameManager.Instance.Floor.ToString() : "Boss!";
    }
}
