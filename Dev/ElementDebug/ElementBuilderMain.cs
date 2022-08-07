using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChatterRebornDev.ElementDebug
{
    public static class ElementBuilderMain
    {

        public static void OnGUI()
        {
            GUI.Box(new Rect(20, 20, 100, 100), "Menu");

            float y = 50f;
            for (int i = 0; i < 10; i++)
            {
                GUI.Button(new Rect(50f, y, 100, 20), "Button" + (i + 1));
                y += 35f;
            }
        }
    }
}
