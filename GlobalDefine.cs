/****************************************************************
 * 作    者：Derek Liu
 * CLR 版本：4.0.30319.42000
 * 创建时间：2018/3/9 13:25:42
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
using LTool;

class GlobalDefine
{
    public static class UINames
    {
        public const string Loading = "UI_Loading";
        public const string MainMenu = "UI_StartMenu";
        public const string Level = "UI_Level";
        public const string LevelChoose = "UI_LevelChoose";
        public const string TipsConfirm = "UI_TipsConfirm";

        //滚动列表子元素
        public const string Child_LevelBrick = "Panel_Child_LevelBrick";
        public const string Child_LevelLine = "Panel_Child_LevelLine";
    }


    public static class PrefabNames
    {
        public const string Floor = "Floor";
        public const string Man = "Man";
        public const string Terminal = "Terminus";
        public const string Enemy = "Enemy";
    }
}
