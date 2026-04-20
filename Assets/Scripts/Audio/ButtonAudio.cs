using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Audio
{
    public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        Selectable selectable;

        void Awake()
        {
            selectable = GetComponent<Selectable>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Don't play hover sound if not interactable
            if (selectable != null && !selectable.interactable)
                return;

            SFXManager.PlaySound(GlobalSFX.ButtonHover);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (selectable != null && !selectable.interactable)
                SFXManager.PlaySound(GlobalSFX.ButtonHover);

            else
                SFXManager.PlaySound(GlobalSFX.ButtonClick);
        }
    }
}