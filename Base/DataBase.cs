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

using UnityEngine;
namespace Data
{

    public static class DataBase
    {
        public static int CurLevel
        {
            get { return PlayerPrefs.GetInt( "Player_CurLevel" , 1 ); }
            set { PlayerPrefs.SetInt( "Player_CurLevel" , value ); }
        }


        public static int UnLockedMaxLevel
        {
            get { return PlayerPrefs.GetInt( "Player_UnLockedMaxLevel" , 1 ); }
            set { PlayerPrefs.SetInt( "Player_UnLockedMaxLevel" , value ); }
        }


        public static bool SoundOpen
        {
            get { return PlayerPrefs.GetInt( "GameSound" , 1 ) == 1; }
            set { PlayerPrefs.SetInt( "GameSound" , value ? 1 : 0 ); }
        }


        public static int TipsNum
        {
            get { return PlayerPrefs.GetInt( "Player_TipsNum" , 9 ); }
            set { PlayerPrefs.SetInt( "Player_TipsNum" , value ); }
        }
    }
}
