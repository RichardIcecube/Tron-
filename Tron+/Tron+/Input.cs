﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tron_
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable();
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null) return false;
            else return (bool)keyTable[key];
        }
        public static void ChangeState(Keys key, bool state) => keyTable[key] = state;
    }
}
