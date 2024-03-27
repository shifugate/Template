using System.Collections.Generic;
using UnityEngine;
using Assets._Scripts.Manager.Keyboard.Component.Keys;

namespace Assets._Scripts.Manager.Keyboard.Component.Level
{
    class KeyboardLine
    {
        public List<Key> keys = new List<Key>();
        public float width = 0;
        public Canvas canvas;
    }
}
