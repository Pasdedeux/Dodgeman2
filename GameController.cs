/**************************************************************** 
 * 作    者：Derek Liu 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/3/7 17:39:39 
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

namespace Assets.Scripts
{
    class GameController:Singleton<GameController>
    {
        //游戏初始化
        public bool GameIsInit { get; set; }

        public void EnterMainMenu()
        {
            SceneLoadManager.Instance.StartFade( 0 );
        }


        public void EnterLevel( int levelID )
        {
            SceneLoadManager.Instance.StartFade( levelID );
        }




    }
}
