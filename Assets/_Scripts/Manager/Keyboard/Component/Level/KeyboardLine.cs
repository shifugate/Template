using System.Collections.Generic;
using UnityEngine;
using Project.Manager.Keyboard.Component.Keys;

namespace Project.Manager.Keyboard.Component.Level
{
    class KeyboardLine
    {
        public List<Key> keys = new List<Key>();
        public float width = 0;
        public Canvas canvas;
    }
}
