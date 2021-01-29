using UnityEngine;
using RWStudios.TransformExtensions;

public class ChildrenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Find parent gameObject with name ParentOfShapes
        Transform parentOfShapes = transform.GetParentWithName("ParentOfShapes");
        if (parentOfShapes != null)
            Debug.Log($"Found parent gameObject named 'ParentOfShapes': {parentOfShapes.GetGameObjectPath()}");

        // Find parent gameObject with tag Circle
        Transform parentCircle = transform.GetParentWithTag("Circle");
        if (parentCircle != null)
            Debug.Log($"Found parent gameObject with tag 'Circle': {parentCircle.GetGameObjectPath()}");

        // Find child transform with name Square5 using depth first find
        Transform square5= transform.FindDeepChildByName("Square5", true);
        if (square5 != null)
            Debug.Log($"Found child gameObject named 'Square5' depth first find: {square5.GetGameObjectPath()}");
        square5 = null;
        // Find child transform with name Square5 using breadth first find
        square5 = transform.FindDeepChildByName("Square5", true, true);
        if (square5 != null)
            Debug.Log($"Found child gameObject named 'Square5' breadth first find: {square5.GetGameObjectPath()}");
        // Find first child transform with Circle tag using depth first find
        Transform circle = transform.FindDeepChildByTag("Circle", true);
        if (circle!= null)
            Debug.Log($"Found child gameObject with tag 'Circle' depth first find: {circle.GetGameObjectPath()}");
        // Find first child transform with Circle tag using breadth first find
        circle = null;
        circle = transform.FindDeepChildByTag("Circle", true, true);
        if (circle != null)
            Debug.Log($"Found child gameObject with tag 'Circle' breadth first find: {circle.GetGameObjectPath()}");
        // Find all active gameObjects with Square tag
        Transform[] allSquares = transform.FindDeepChildrenByTagArray("Square", true);
        foreach (Transform square in allSquares)
        {
            Debug.Log($"Found active child gameObjects with tag 'Square': {square.GetGameObjectPath()}");
        }

    }

}
