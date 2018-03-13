/****************************************************************
 * 作    者：Derek Liu
 * CLR 版本：4.0.30319.42000
 * 创建时间：2018/3/7 23:03:09
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
using UnityEngine;

namespace Assets.Scripts
{
    class AppStart:MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad( this );
        }

        private void Start()
        {
            GameController.Instance.EnterMainMenu();
        }
    }
}
