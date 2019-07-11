using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using dev.kemomimi.UI.menubar;

public class Master : MonoBehaviour {

    [SerializeField] private MenuBarController _menuBarController;

	void Start () {

        #region メニューバーの設定
        List<MenuBarData> fileMbs = new List<MenuBarData>();

        fileMbs.Add(new MenuBarData("a"));
        fileMbs.Add(new MenuBarData("b"));
        fileMbs.Add(new MenuBarData("c"));

        List<MenuBarData> fileMb = new List<MenuBarData>();

        fileMb.Add(new MenuBarData("Save"));
        fileMb.Add(new MenuBarData("Load", fileMbs));
        fileMb.Add(new MenuBarData("Quit", fileMbs));

        List<MenuBarData> fileMb2 = new List<MenuBarData>();

        fileMb2.Add(new MenuBarData("Save"));
        fileMb2.Add(new MenuBarData("Load"));
        fileMb2.Add(new MenuBarData("Quit"));
        fileMb2.Add(new MenuBarData("e1"));
        fileMb2.Add(new MenuBarData("e2"));
        fileMb2.Add(new MenuBarData("e3"));
        fileMb2.Add(new MenuBarData("e4"));
        fileMb2.Add(new MenuBarData("e5"));
        fileMb2.Add(new MenuBarData("e6", fileMb));

        List<MenuBarData> fileMb3 = new List<MenuBarData>();

        fileMb3.Add(new MenuBarData("mb3-1", fileMb));
        fileMb3.Add(new MenuBarData("mb3-2", fileMb));
        fileMb3.Add(new MenuBarData("mb3-3", fileMb));

        List<MenuBarData> menuBarDatas = new List<MenuBarData>();

        menuBarDatas.Add(new MenuBarData("File", fileMb));
        menuBarDatas.Add(new MenuBarData("Edit", fileMb));
        menuBarDatas.Add(new MenuBarData("Assets", fileMb2));
        menuBarDatas.Add(new MenuBarData("GameObject", fileMb2));
        menuBarDatas.Add(new MenuBarData("Component", fileMb3));

        _menuBarController.SetRootMenuBarData(menuBarDatas);
        _menuBarController.ReDraw();

        #endregion
    }
}