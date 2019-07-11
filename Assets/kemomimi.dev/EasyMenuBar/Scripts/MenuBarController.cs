using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace dev.kemomimi.UI.menubar
{
    [System.Serializable]
    public class StringEvent : UnityEvent<string>
    {
    }

    public class MenuBarController : MonoBehaviour
    {
        [SerializeField] private UIManager _uIManager;
        [SerializeField] private UIResourceManager _uIResourceManager;
        private MenuBarData[] _rootMenuBarData;
        private List<GameObject> _rootMenuBarObjectsBuff = new List<GameObject>();

        private List<GameObject> _lastInstancedElementObjList = new List<GameObject>();

        [Space(15f)]public StringEvent CommandCallback;

        public void ClearMenuElementObjects()
        {
            foreach (var g in _lastInstancedElementObjList)
            {
                Destroy(g);
            }
            _lastInstancedElementObjList.Clear();
        }

        public void SetRootMenuBarData(MenuBarData[] rootMenuBarData)
        {
            this._rootMenuBarData = new MenuBarData[rootMenuBarData.Length];
            this._rootMenuBarData = rootMenuBarData;
        }

        public void SetRootMenuBarData(List<MenuBarData> rootMenuBarDataList)
        {
            this._rootMenuBarData = new MenuBarData[rootMenuBarDataList.Count];
            for (int i = 0; i < rootMenuBarDataList.Count; i++)
            {
                this._rootMenuBarData[i] = rootMenuBarDataList[i];
            }
        }

        /// <summary>
        /// メニューバーの一番上の階層用
        /// </summary>
        /// <param name="clickedElementName"></param>
        public void OnClickMenuBar(string clickedElementName)
        {
            if (_lastInstancedElementObjList.Count > 0)
                ClearMenuElementObjects();

            for (int i = 0; i < _rootMenuBarData.Length; i++)
            {
                if(_rootMenuBarData[i].ElementName == clickedElementName)
                {
                    var g = Instantiate((GameObject)Resources.Load("Prefabs/menuBarElementBg.Image"), _uIManager.MenuBarElementParentObj.transform);
                    var pos = _rootMenuBarObjectsBuff[i].transform.position;
                    g.GetComponent<RectTransform>().position = new Vector2(pos.x, pos.y - 10f);

                    float maxElementXSize = 0;
                    foreach (var e in _rootMenuBarData[i]._childElement)
                    {
                        if (maxElementXSize < e.ElementName.Length * 7 + 60)
                            maxElementXSize = e.ElementName.Length * 7 + 60;
                    }

                    float YSize = 0;
                    for (int j = 0; j < _rootMenuBarData[i]._childElement.Length; j++)
                    {
                        //Debug.Log(_rootMenuBarData[i]._childElement[j].ElementName);
                        var ig = Instantiate((GameObject)Resources.Load("Prefabs/button.ImageVariant"), g.transform);
                        ig.GetComponent<RectTransform>().position = new Vector2(pos.x, pos.y - 10f);
                        pos.y -= ig.GetComponent<RectTransform>().sizeDelta.y;
                        var text = "   " + _rootMenuBarData[i]._childElement[j].ElementName;

                        var tmps = ig.GetComponentsInChildren<TextMeshProUGUI>();
                        if (_rootMenuBarData[i]._childElement[j]._childElement.Length > 0)
                        {
                            tmps[1].text = ">";
                        }

                        var sizeDelta = new Vector2(maxElementXSize, ig.GetComponent<RectTransform>().sizeDelta.y);
                        tmps[0].text = text;
                        tmps[0].gameObject.GetComponent<RectTransform>().sizeDelta = sizeDelta;
                        ig.GetComponent<RectTransform>().sizeDelta = sizeDelta;

                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerDown;
                        entry.callback.AddListener((x) => { ig.GetComponent<MenuBarButtonController>().ButtonDownHandler(); });
                        ig.GetComponent<EventTrigger>().triggers.Add(entry);
                        ig.GetComponent<MenuBarButtonController>().MenuBarData = _rootMenuBarData[i]._childElement[j];

                        var n = _rootMenuBarData[i]._childElement[j].ElementName;

                        entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerEnter;
                        entry.callback.AddListener((x) => { ig.GetComponent<MenuBarButtonController>().ButtonHoverHandler(n); });
                        ig.GetComponent<EventTrigger>().triggers.Add(entry);
                        ig.GetComponent<MenuBarButtonController>().MenuBarData = _rootMenuBarData[i]._childElement[j];

                        entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerExit;
                        entry.callback.AddListener((x) => { ig.GetComponent<MenuBarButtonController>().ButtonHoverOutHandler(n); });
                        ig.GetComponent<EventTrigger>().triggers.Add(entry);
                        ig.GetComponent<MenuBarButtonController>().MenuBarData = _rootMenuBarData[i]._childElement[j];

                        YSize += Mathf.Abs(sizeDelta.y);
                    }

                    g.GetComponent<RectTransform>().sizeDelta = new Vector2(maxElementXSize, YSize);

                    _lastInstancedElementObjList.Add(g);
                }
            }
        }

        /// <summary>
        /// 画面上部のメニューバーの再描画
        /// </summary>
        public void ReDraw()
        {
            foreach (Transform t in _uIManager.MenuBarRootImage.transform)
            {
                Destroy(t.gameObject);
            }

            foreach (Transform t in _uIManager.MenuBarElementParentObj.transform)
            {
                Destroy(t.gameObject);
            }

            _rootMenuBarObjectsBuff.Clear();
            if(_rootMenuBarData != null)
            {
                float s_count = 5;
                for (int i = 0; i < _rootMenuBarData.Length; i++)
                {
                    var sl = _rootMenuBarData[i].ElementName.Length;
                    var b = Instantiate((GameObject)Resources.Load("Prefabs/button.Image"));
                    var sd = new Vector2(sl * 7 + 40, b.GetComponent<RectTransform>().sizeDelta.y);
                    b.GetComponent<RectTransform>().sizeDelta = sd;
                    b.transform.parent = _uIManager.MenuBarObj.transform;
                    b.transform.position = new Vector2(s_count, _uIManager.MenuBarObj.transform.position.y);

                    if (_rootMenuBarData[i]._childElement.Length > 0)
                    {
                        EventTrigger.Entry entry = new EventTrigger.Entry();
                        entry.eventID = EventTriggerType.PointerDown;
                        var elementName = _rootMenuBarData[i].ElementName;
                        entry.callback.AddListener((x) => { OnClickMenuBar(elementName); });
                        b.GetComponent<EventTrigger>().triggers.Add(entry);
                    }

                    s_count += sd.x + 5;
                    var tmp = b.GetComponentInChildren<TextMeshProUGUI>();
                    tmp.text = _rootMenuBarData[i].ElementName;
                    tmp.gameObject.GetComponent<RectTransform>().sizeDelta = sd;
                    b.SetActive(true);

                    _rootMenuBarObjectsBuff.Add(b);
                }
            }
        }
    }
}