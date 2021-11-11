using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MutationButton mutationButtonRef;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mutationButtonRef.ChangeHoverState(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mutationButtonRef.ChangeHoverState(false);
    }
}
