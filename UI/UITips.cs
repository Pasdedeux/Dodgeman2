/****************************************************************
 * 作    者：Derek Liu
 * CLR 版本：4.0.30319.42000
 * 创建时间：2018/3/22 13:25:42
 * 当前版本：1.0.0.1
 * 
 * 描述说明：
 *
 * 修改历史：
 *
*****************************************************************
 * Copyright @ Derek 2018 All rights reserved
*****************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class UITips:BaseUI
    {
        Button _okBtn, _cancelBtn;

        private void Awake()
        {
            CurrentUIType.uiType = UITypeEnum.PopUp;
            CurrentUIType.uiShowMode = UIShowModeEnum.PopUp;
            CurrentUIType.uiTransparent = UITransparentEnum.NoPenetratingLow;
        }

        private void OnEnable()
        {
            _cancelBtn.onClick.AddListener( () => 
            {
                UIManager.Instance.Close( GlobalDefine.UINames.TipsConfirm );
            } );
            _okBtn.onClick.AddListener( () => 
            {
                UnityAds.Instance.unityAdsRewardFinished = () => 
                {
                    DataModel.Instance.TipsNum += GameManager.Instance.unityAds_Tips;
                    UIManager.Instance.Close( GlobalDefine.UINames.TipsConfirm );
                    try
                    { UIManager.Instance.GetUIByName( GlobalDefine.UINames.Level ).Update(); }
                    catch
                    {
                        UnityEngine.Debug.LogError( "UI " + GlobalDefine.UINames.Level + "未加载" );
                    }
                    
                };
                UnityAds.Instance.ShowRewardAds();
            } );
        }


        private void OnDisable()
        {
            _okBtn.onClick.RemoveAllListeners();
            _cancelBtn.onClick.RemoveAllListeners();
        }


    }
}
