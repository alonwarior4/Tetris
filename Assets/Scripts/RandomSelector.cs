using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RandomSelector
{
    public virtual string GetNextTetromino() { return null; }
}
