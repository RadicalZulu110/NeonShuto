using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ZoomStrategy
{
    void ZoomIn(Camera cam, float delta, float nearZoomLimit);
    void ZoomOut(Camera cam, float delta, float farZoomLimit);
}
