using NumSharp;
using UnityEngine;
using System;

public class Test
{
    // Start is called before the first frame update
    public Test()
    {
        
        NDArray test = new float[1] { 1};
        np.ceil(test);
    }
}
