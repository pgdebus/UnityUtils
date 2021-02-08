# RWStudios Unity Utils
Useful Unity C# utilities, helper classes and extension methods.

This repository contains the utility scripts and an example Unity project to demonstrate their usage. It was made using Unity version (2020.2.1f1) and should work fine with previous versions down to at least 2017.

You can either load the example project in Unity to  view example usage or simply copy the utility extension method scripts you want to use directly into your existing project.

**TL:DR**

*Step 1:* Copy the Assets/External/RWStudios scripts into your Unity project.

*Step 2:* Include the RWStudios.TransformExtensions namespace in your C# script.

*Step 3:* Start using the Transform extension methods below, they include tooltip help for your C# editor. See Features below for a list of methods.

*Step 4:* Code example to find an active deep child gameObject.transform with the tag "Circle" using depth first search.
```C#
Transform circle = transform.FindDeepChildByTag("Circle", true);
if (circle!= null)
    Debug.Log($"Found child transform with tag 'Circle' depth first find: {circle.GetGameObjectPath()}");
```


## Features
The Unity utilities package contains Transform extensions to find nested gameObject.transforms by name or by tag. Methods to find child or parent transforms by name or tag. Method parameters define options to find depth first or breadth first and if the transform must be active or not. It also supports finding multiple child objects with the given name or tag and returning a list or array.

Also includes a method  to return the full nested path name of a transform.

The find gameObject.transform methods do not do a global wide find like GameObject.Find or GameObject.FindGameObjectsWithTag, they find gameOjects that are either child transforms or parent transforms of the calling transform only.

### Transform Extension Methods

* FindDeepChildByName - finds a child transform with the given name
* FindDeepChildByTag  - finds a child transform with the given tag
* FindDeepChildrenByName - returns a list of all child transform with the given name
* FindDeepChildrenByNameArray - returns an array of all child transforms with the given name
* FindDeepChildrenByTag - returns a list of all child transforms with the given tag
* FindDeepChildrenByTagArray - returns an array of all child transforms with the given tag
* GetParentWithName - returns a parent transform with the given name
* GetParentWithTag - returns a parent transform with the given tag
* GetGameObjectPath - returns the full nested path name of a transform

## Known Performance Limitations
As with other Unity find or findWithTag methods these find methods are not
performant if called within Update or FixedUpdate loops. To avoid performance overhead, especially when finding deeply nested GameObjects use these methods in Awake or Start methods and cache results in a local field or property, or call them on demand that is not in a tight loop.


## Using the example project
Download the project then open the *StartHere* scene. The scene contains a bunch of nested empty, circle and square gameObjects. The *Children* gameObject has an example C# script component called *ChildrenController* demonstrating the use of the Transform extension methods. The *ChildrenController* script just finds some children and parent game objects and prints those found in the Unity console. Note the *Square4* gameObject is not active to demonstrate excluding inactive gameObjects.

## Credits
The code in the transform extension method GetGameObjectPath was copied from a Unity forum post.
