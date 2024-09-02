using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public interface Iraycatable
    {
        bool HandleRaycast(PlayerController controller);
        CursorType GetCursorType();
    }
}