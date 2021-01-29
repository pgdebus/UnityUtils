using System.Collections.Generic;
using UnityEngine;


namespace RWStudios.TransformExtensions
{
    /// <summary>
    /// FindDeepChild is a UnityEngine.Transform extension method class. It provides method extensions to find child GameObjects
    /// of the calling Transform only, only interrogating the child tree of the calling Transform.
    /// The methods do not do a global wide find like GameObject.Find, GameObject.FindGameObjectsWithTag 
    /// or GameObject.FindWithTag methods.
    /// 
    /// Note: These extension methods are not very performant so avoid calling them in Update or FixedUpdate loops
    /// to avoid performance overhead, especially when finding deeply nested GameObjetcs.
    /// Best practice use is to call them in Awake or Start methods and cache results in a local field or property.
    /// 
    /// Internally these extension methods use Transform.Find, Transform.GetChild(i) and Transform.CompareTag(string) with a recursive walk down the
    /// Transform's child hierarchy.
    /// See: 
    ///     https://docs.unity3d.com/ScriptReference/Transform.Find.html
    ///     https://docs.unity3d.com/ScriptReference/Transform-childCount.html
    ///     https://docs.unity3d.com/ScriptReference/Transform.GetChild.html
    ///     https://docs.unity3d.com/ScriptReference/Component.CompareTag.html
    /// </summary>
    public static class FindDeepChild
    {
        /// <summary>
        /// Recursively finds a Transform's child GameObject by Transform.name given by <paramref name="name"/>. 
        /// The bool <paramref name="mustBeActive"/> can be used to control whether the child object must be active or not
        /// and bool <paramref name="breadthFirst"/> to do a breadth first if true or depth first search if false. Returns the child Transfrom 
        /// otherwise null if not found. The parent object is not included in the search.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">string name of child object to find</param>
        /// <param name="mustBeActive">bool true means child must be active, false means child may or may not be active</param>
        /// <param name="breadthFirst">bool true does a breadth first search, false (default) does a depth first search</param>
        /// <returns>UnityEngine.Transform</returns>
        public static Transform FindDeepChildByName(this Transform parent, string name, bool mustBeActive, bool breadthFirst = false)
        {
            if (breadthFirst)
            {
                Transform target = parent.Find(name);
                if ((target != null) && (! mustBeActive || ( target.gameObject.activeSelf)))
                    return target;
            }
            else
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    if ((parent.GetChild(i).name.Equals(name)) && (! mustBeActive || (parent.GetChild(i).gameObject.activeSelf)))
                        return parent.GetChild(i);
                    Transform grandchild = FindDeepChildByName(parent.GetChild(i), name, mustBeActive, breadthFirst);
                    if (grandchild != null)
                        return grandchild;
                }
            }
            if (breadthFirst)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform grandchild = FindDeepChildByName(parent.GetChild(i), name, breadthFirst);
                    if (grandchild != null)
                        return grandchild;
                }
            }
            return null;
        }

        /// <summary>
        /// Recursively finds a Transform's child GameObject by <paramref name="tag"/>. The tag must exist in the projects tag list.
        /// The bool <paramref name="mustBeActive"/> can be used to control whether the child object must be active or not
        /// and the bool <paramref name="breadthFirst"/> is used do a breadth first search if true or depth first search if false. 
        /// Returns the child Transfrom otherwise null if not found. The parent object is not included in the search.
        /// The tag must be defined, otherwise the standard Unity tag is not defined exception will be thrown.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag">string tag of child object to find</param>
        /// <param name="mustBeActive">bool true means child must be active, false means child may or may not be active</param>
        /// <param name="breadthFirst">bool true does a breadth first search, false (default) does a depth first search</param>
        /// <returns>UnityEngine.Transform</returns>
        public static Transform FindDeepChildByTag(this Transform parent, string tag, bool mustBeActive, bool breadthFirst = false)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if ((parent.GetChild(i).CompareTag(tag)) && (!mustBeActive || (parent.GetChild(i).gameObject.activeSelf)))
                {
                    return parent.GetChild(i);
                }
                if (!breadthFirst)
                {
                    Transform grandchild = FindDeepChildByTag(parent.GetChild(i), tag, breadthFirst);
                    if ((grandchild != null) && (!mustBeActive || (grandchild.gameObject.activeSelf)))
                        return grandchild;
                }
            }
            if (breadthFirst)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform grandchild = FindDeepChildByTag(parent.GetChild(i), tag, breadthFirst);
                    if ((grandchild != null) && (!mustBeActive || (grandchild.gameObject.activeSelf)))
                        return grandchild;
                }
            }
            return null;
            }

        /// <summary>
        /// Recursively finds all child GameObjects by Transform.name given <paramref name="name"/>.
        /// The bool <paramref name="mustBeActive"/> can be used to control whether the child objects returned must be active or not.
        /// Returns a List{UnityEngine.Transform} of child Transforms found, may be an empty List.
        /// The parent object is not included in the search.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">string tag of child object to find</param>
        /// <param name="mustBeActive">bool true means child must be active, false means child may or may not be active</param>
        /// <returns>List{UnityEngine.Transform}</returns>
        public static List<Transform> FindDeepChildrenByName(this Transform parent, string name, bool mustBeActive)
        {
            //Transform target = parent.Find(name);
            //if (target != null)
            //    return target;
            // foreach (Transform child in parent)
            List<Transform> found = new List<Transform>();
            for (int i = 0; i < parent.childCount; i++)
            {
                if ((parent.GetChild(i).name.Equals(name)) && (!mustBeActive || (parent.GetChild(i).gameObject.activeSelf)))
                {
                    found.Add(parent.GetChild(i));
                }
                found.AddRange(parent.GetChild(i).FindDeepChildrenByTag(name, mustBeActive));
            }
            return found;
        }

        /// <summary>
        /// Recursively finds all child GameObjects by Transform.name given <paramref name="name"/>. 
        /// Internally this just calls FindDeepChildrenByName and converts the List to an array.
        /// The bool <paramref name="mustBeActive"/> can be used to control whether the child objects returned must be active or not.
        /// Returns UnityEngine.Transform[] array of child Transforms found, may be zero length array.
        /// The parent object is not included in the search.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name">string tag of child object to find</param>
        /// <param name="mustBeActive">bool true means child must be active, false means child may or may not be active</param>
        /// <returns>UnityEngine.Transform[]</returns>
        public static Transform[] FindDeepChildrenByNameArray(this Transform parent, string name, bool mustBeActive)
        {
            return parent.FindDeepChildrenByName(name, mustBeActive).ToArray();
        }

        /// <summary>
        /// Recursively finds all child GameObjects by <paramref name="tag"/>.
        /// The bool <paramref name="mustBeActive"/> can be used to control whether the child objects returned must be active or not.
        /// Returns a List{UnityEngine.Transform} of child Transforms found, may be an empty List.
        /// The parent object is not included in the search.
        /// The tag must be defined, otherwise the standard Unity tag is not defined exception will be thrown.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag">string tag of child object to find</param>
        /// <param name="mustBeActive">bool true means child must be active, false means child may or may not be active</param>
        /// <returns>List{UnityEngine.Transform}</returns>
        public static List<Transform> FindDeepChildrenByTag(this Transform parent, string tag, bool mustBeActive)
        {
            List<Transform> found = new List<Transform>();

            for (int i = 0; i < parent.childCount; i++)
            {
                if ((parent.GetChild(i).CompareTag(tag)) && (!mustBeActive || (parent.GetChild(i).gameObject.activeSelf)))
                {
                    found.Add(parent.GetChild(i));
                }
                found.AddRange(parent.GetChild(i).FindDeepChildrenByTag(tag, mustBeActive));
            }
            return found;
        }

        /// <summary>
        /// Recursively finds all child GameObjects by <paramref name="tag"/>. 
        /// Internally this just calls FindDeepChildrenByTag and converts the List to an array.
        /// The bool <paramref name="mustBeActive"/> can be used to control whether the child objects returned must be active or not.
        /// Returns UnityEngine.Transform[] array of child Transforms found, may be zero length array.
        /// The parent object is not included in the search.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag">string tag of child object to find</param>
        /// <param name="mustBeActive">bool true means child must be active, false means child may or may not be active</param>
        /// <returns>UnityEngine.Transform[]</returns>
        public static Transform[] FindDeepChildrenByTagArray(this Transform parent, string tag, bool mustBeActive)
        {
            return parent.FindDeepChildrenByTag(tag, mustBeActive).ToArray();
        }
    }

    /// <summary>
    /// ParentFind is a UnityEngine.Transform extension method class. It provides method extensions to find parent GameObjects
    /// of the calling Transform only, only interrogating the parent hierarchy of the calling Transform.
    /// 
    /// Note: These extension methods are not very performant so avoid calling them in Update or FixedUpdate otherwise
    /// they will have significant performance overhead especially if the calling Transfrom is deep in the hierarchy.
    /// Best practice use is to call them in Awake or Start methods and cache results in a local field or property.
    /// 
    /// Internally these extension methods use Transform.Parent and Transform.CompareTag with a recursive walk up the Transform's hierarchy.
    /// See:
    ///     https://docs.unity3d.com/ScriptReference/Transform-parent.html
    ///     https://docs.unity3d.com/ScriptReference/Component.CompareTag.html
    /// </summary>
    public static class ParentFind
    {
        /// <summary>
        /// Recursively finds a Transform's parent GameObject by <paramref name="name"/>. 
        /// Returns the parent Transfrom otherwise null if not found. The calling object is not included in the search.
        /// </summary>
        /// <param name="child"></param>
        /// <param name="name">string name of child object to find</param>
        /// <returns>UnityEngine.Transform</returns>
        public static Transform GetParentWithName(this Transform child, string name)
        {
            if (child.transform.parent != null)
            {
                if (child.transform.parent.name.Equals(name))
                {
                    return child.transform.parent;
                }
                return GetParentWithName(child.transform.parent, name);
            }
            return null;
        }

        /// <summary>
        /// Recursively finds a Transform's parent GameObject by <paramref name="tag"/>. 
        /// Returns the parent Transfrom otherwise null if not found. The calling object is not included in the search.
        /// The tag must be defined, otherwise the standard Unity tag is not defined exception will be thrown.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag">string tag of child object to find</param>
        /// <returns>UnityEngine.Transform</returns>

        public static Transform GetParentWithTag(this Transform child, string tag)
        {
            if (child.transform.parent != null)
            {
                if (child.transform.parent.CompareTag(tag))
                {
                    return child.transform.parent;
                }
                return GetParentWithTag(child.transform.parent, tag);
            }
            return null;
        }
    }

    /// <summary>
    /// TransformFullPath is a UnityEngine.Transform extension method class. It provides method extensions to return GameObject
    /// full path names capturing their position in the hierarchy. 
    /// 
    /// Note: These extension methods are not very performant so avoid calling them in Update or FixedUpdate otherwise
    /// they will have significant performance overhead especially if the calling Transfrom is deep in the hierarchy.
    /// Best practice use is to call them in Awake or Start methods and cache results in a local field or property.
    /// 
    /// Internally these extension methods use Transform.Parent with a while loop walk up the Transform's hierarchy.
    /// See:
    ///     https://docs.unity3d.com/ScriptReference/Transform-parent.html
    /// </summary>
    public static class TransformFullPath
    {
        /// <summary>
        /// Returns the full hierarchy GameObject path of the calling Transform. 
        /// Example: A 4 level nested GameObject path may be /a/b/c/d
        /// </summary>
        /// <returns>string</returns>
        public static string GetGameObjectPath(this Transform targetTransform)
        {
            string path = targetTransform.name;
            while (targetTransform.parent != null)
            {
                targetTransform = targetTransform.parent;
                path = targetTransform.name + "/" + path;
            }
            return "/" + path;
        }
    }

    
}
