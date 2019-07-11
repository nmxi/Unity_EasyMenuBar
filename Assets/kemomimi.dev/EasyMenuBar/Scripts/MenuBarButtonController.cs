using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace dev.kemomimi.UI.menubar
{
    public class MenuBarButtonController : MonoBehaviour
    {
        public MenuBarController MenuBarController;
        public MenuBarData MenuBarData;
        public int ShowElementTabDelay = 5; //*0.1s

        //private Coroutine _htc; //HoverTimeCounterCoroutine

        private GameObject _lastInstancedObject = null;    //最後にインスタンスを作成したオブジェクト

        void OnEnable()
        {
            MenuBarController = GameObject.FindGameObjectsWithTag("Master")[0].GetComponent<MenuBarController>();
        }

        public void ButtonDownHandler()
        {
            //Debug.Log("Element clicked");
            if(MenuBarData._childElement.Length > 0)
            {
                InstanceChildElement();
            }
            else if(MenuBarData._childElement.Length == 0)
            {
                //Debug.Log("command : " + MenuBarData.ElementName);
                MenuBarController.CommandCallback.Invoke(MenuBarData.ElementName);

                MenuBarController.ReDraw();
            }
        }

        public void ButtonHoverHandler(string elementName)
        {
            //Vector2 pos = gameObject.GetComponent<RectTransform>().position;
            //Vector2 hs = gameObject.GetComponent<RectTransform>().sizeDelta / 2;
            //Debug.Log(Input.mousePosition + " " + pos);

            //Vector2 m_pos = Input.mousePosition;
            //if(m_pos.x < pos.x + hs.x && pos.x - hs.x < m_pos.x && m_pos.y < pos.y + hs.y && pos.y - hs.y < m_pos.y)
            //{
            //    if (MenuBarData._childElement.Length > 0)
            //    {
            //        _htc = StartCoroutine(HoverTimeCounter());
            //    }
            //}
        }

        public void ButtonHoverOutHandler(string elementName)
        {
            //if (MenuBarData._childElement.Length > 0 && _htc != null)
            //    StopCoroutine(_htc);

            //if (_lastInstancedObject != null && _lastInstancedObject.name != elementName)
            //{
            //    gameObject.GetComponent<UIEventAnimation>().ExitAnimation();
            //}

            //_htc = null;
        }

        //private IEnumerator HoverTimeCounter()
        //{
        //    int count = 0;
        //    while (count < ShowElementTabDelay)
        //    {
        //        count++;
        //        yield return new WaitForSeconds(0.1f);
        //    }
        //    InstanceChildElement();
        //    Debug.Log("show");
        //    yield break;
        //}

        private void InstanceChildElement()
        {
            gameObject.GetComponent<UIEventAnimation>().IsLockColor = true;

            foreach (Transform t in gameObject.transform.parent.transform)
            {
                if (t.gameObject.GetComponent<UIEventAnimation>() && t.gameObject != gameObject)
                {
                    t.gameObject.GetComponent<UIEventAnimation>().IsLockColor = false;
                    t.gameObject.GetComponent<UIEventAnimation>().ExitAnimation();
                }

                foreach (Transform ct in t)
                {
                    if (ct.gameObject.tag == "MenuBar")
                    {
                        Destroy(ct.gameObject);
                    }
                }
            }

            if (this.MenuBarData._childElement.Length > 0)
            {
                var g = Instantiate((GameObject)Resources.Load("Prefabs/menuBarElementBg.Image"), gameObject.transform);
                var pos = gameObject.transform.position;
                var rect = gameObject.GetComponent<RectTransform>();
                pos.x += rect.sizeDelta.x;
                g.GetComponent<RectTransform>().position = pos;

                float maxElementXSize = 0;
                foreach (var e in this.MenuBarData._childElement)
                {
                    if (maxElementXSize < e.ElementName.Length * 7 + 60)
                        maxElementXSize = e.ElementName.Length * 7 + 60;
                }

                float YSize = 0;
                for (int i = 0; i < this.MenuBarData._childElement.Length; i++)
                {
                    //Debug.Log(this.MenuBarData._childElement[i].ElementName);
                    var e = Instantiate((GameObject)Resources.Load("Prefabs/button.ImageVariant"), g.transform);
                    e.GetComponent<RectTransform>().position = pos;
                    pos.y -= e.GetComponent<RectTransform>().sizeDelta.y;
                    var text = "   " + this.MenuBarData._childElement[i].ElementName;

                    var tmps = e.GetComponentsInChildren<TextMeshProUGUI>();

                    if (this.MenuBarData._childElement[i]._childElement.Length > 0)
                    {
                        tmps[1].text = ">";
                    }

                    var sizeDelta = new Vector2(maxElementXSize, e.GetComponent<RectTransform>().sizeDelta.y);
                    tmps[0].text = text;
                    tmps[0].gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
                    e.GetComponent<RectTransform>().sizeDelta = sizeDelta;

                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerDown;
                    entry.callback.AddListener((x) => { e.GetComponent<MenuBarButtonController>().ButtonDownHandler(); });
                    e.GetComponent<EventTrigger>().triggers.Add(entry);
                    e.GetComponent<MenuBarButtonController>().MenuBarData = this.MenuBarData._childElement[i];

                    var n = this.MenuBarData._childElement[i].ElementName;

                    entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerEnter;
                    entry.callback.AddListener((x) => { e.GetComponent<MenuBarButtonController>().ButtonHoverHandler(n); });
                    e.GetComponent<EventTrigger>().triggers.Add(entry);
                    e.GetComponent<MenuBarButtonController>().MenuBarData = this.MenuBarData._childElement[i];

                    entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerExit;
                    entry.callback.AddListener((x) => { e.GetComponent<MenuBarButtonController>().ButtonHoverOutHandler(n); });
                    e.GetComponent<EventTrigger>().triggers.Add(entry);
                    e.GetComponent<MenuBarButtonController>().MenuBarData = this.MenuBarData._childElement[i];

                    YSize += Mathf.Abs(sizeDelta.y);
                }

                g.GetComponent<RectTransform>().sizeDelta = new Vector2(maxElementXSize, YSize);

                _lastInstancedObject = g;
            }
        }
    }
}