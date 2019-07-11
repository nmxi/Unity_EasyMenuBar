using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace dev.kemomimi.UI.menubar
{
    public class UIEventAnimation : MonoBehaviour
    {
        [SerializeField] private Color _EnterEventColor;
        [SerializeField] private Color _ExitEventColor;

        public bool IsLockColor = false;

        public void EnterAnimation()
        {
            if(!IsLockColor)
                gameObject.GetComponent<Image>().color = _EnterEventColor;
        }

        public void ExitAnimation()
        {
            if(!IsLockColor)
                gameObject.GetComponent<Image>().color = _ExitEventColor;
        }
    }
}