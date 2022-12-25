using System.Collections;
using System.Collections.Generic;
using AsunaClient.Foundation;
using AsunaClient.Foundation.UI;
using UnityEngine;
using UnityEngine.UI;

public class DemoMainPage : UIPage
{

    public override void SetupController()
    {
        _LoadSceneBtn = _Root.transform.Find("LoadSceneBtn").GetComponent<Button>();
        _LoadSceneBtn.onClick.AddListener(_OnLoadScene);
    }

    public override void OnShow(ShowPageParam param)
    {
    }

    public override void OnHide()
    {
    }

    private void _OnLoadScene()
    {
        XDebug.Info("load scene");
    }
    
    private Button _LoadSceneBtn;

}