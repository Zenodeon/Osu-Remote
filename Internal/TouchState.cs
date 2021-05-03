using System.Collections;
using System.Collections.Generic;

public class TouchState 
{
    public int touchIndex { get; set; }

    public TouchPhase phase { get; set; }

}

public enum TouchPhase
{
    Began,
    Moved,
    Stationary,
    Ended,
    Canceled
}
