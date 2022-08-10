using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public Texture2D cursorArrow;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(cursorArrow, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        DontDestroyOnLoad(this);
    }
}
