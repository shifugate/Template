using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.Manager.Keyboard.Component.Level
{
    class KeyboardLevel
    {
        public List<KeyboardLine> lines = new List<KeyboardLine>();
        public string level;
        public float height = 0;
        public Canvas canvas;
    }
}