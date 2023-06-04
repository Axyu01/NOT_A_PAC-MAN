using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    struct dir // niech kontroler podaje tylko jedna naraz, inaczej bedzie trzeba ustalic priorytet osi
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
    }
    public void SetMoveDirection(dir dir);
    public void SetPosition(Vector2 position);
}
