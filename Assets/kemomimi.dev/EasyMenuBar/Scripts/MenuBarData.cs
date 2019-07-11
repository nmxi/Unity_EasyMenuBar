using System.Collections.Generic;

namespace dev.kemomimi.UI.menubar
{
    public class MenuBarData
    {
        public string ElementName;
        public MenuBarData[] _childElement;

        public MenuBarData(string elementName)
        {
            this.ElementName = elementName;
            this._childElement = new MenuBarData[0];
        }

        public MenuBarData(string elementName, MenuBarData[] childElementData)
        {
            this.ElementName = elementName;
            this._childElement = new MenuBarData[childElementData.Length];
            for (int i = 0; i < childElementData.Length; i++)
            {
                this._childElement[i] = childElementData[i];
            }
        }

        public MenuBarData(string elementName, List<MenuBarData> childElementDataList)
        {
            this.ElementName = elementName;
            this._childElement = new MenuBarData[childElementDataList.Count];
            for (int i = 0; i < childElementDataList.Count; i++)
            {
                this._childElement[i] = childElementDataList[i];
            }
        }
    }
}