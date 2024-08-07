﻿using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace IKhom.ExtensionsLibrary.Runtime
{
    /// <summary>
    /// Provides extension methods for the <see cref="GameObject"/> class.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns the object itself if it exists, null otherwise.
        /// </summary>
        /// <remarks>
        /// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
        /// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method uses
        /// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
        /// aiding in correctly chaining operations and preventing NullReferenceExceptions.
        /// </remarks>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="obj">The object being checked.</param>
        /// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
        [PublicAPI]
        public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

        /// <summary>
        /// Gets the specified component if it exists on the GameObject; otherwise, adds it.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="go">The GameObject to get or add the component to.</param>
        /// <returns>The existing or newly added component.</returns>
        [PublicAPI]
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            var component = go.GetComponent<T>();
            if (!component) component = go.AddComponent<T>();

            return component;
        }

        /// <summary>
        /// Destroys all child GameObjects of the specified GameObject.
        /// </summary>
        /// <param name="go">The GameObject whose children should be destroyed.</param>
        [PublicAPI]
        public static void DestroyChildren(this GameObject go)
        {
            go.transform.DestroyChildren();
        }

        /// <summary>
        /// Enables all child GameObjects of the specified GameObject.
        /// </summary>
        /// <param name="go">The GameObject whose children should be enabled.</param>
        [PublicAPI]
        public static void EnableChildren(this GameObject go)
        {
            go.transform.EnableChildren();
        }

        /// <summary>
        /// Disables all child GameObjects of the specified GameObject.
        /// </summary>
        /// <param name="go">The GameObject whose children should be disabled.</param>
        [PublicAPI]
        public static void DisableChildren(this GameObject go)
        {
            go.transform.DisableChildren();
        }

        /// <summary>
        /// Resets the GameObject's transform's position, rotation, and scale to their default values.
        /// </summary>
        /// <param name="gameObject">The GameObject whose transformation is to be reset.</param>
        [PublicAPI]
        public static void ResetTransformation(this GameObject gameObject)
        {
            gameObject.transform.Reset();
        }

        /// <summary>
        /// Returns the hierarchical path in the Unity scene hierarchy for this GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to get the path for.</param>
        /// <returns>A string representing the full hierarchical path of this GameObject in the Unity scene.
        /// This is a '/'-separated string where each part is the name of a parent, starting from the root parent and ending
        /// with the name of the specified GameObject's parent.</returns>
        [PublicAPI]
        public static string Path(this GameObject gameObject)
        {
            return "/" + string.Join("/",
                gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());
        }

        /// <summary>
        /// Returns the full hierarchical path in the Unity scene hierarchy for this GameObject.
        /// </summary>
        /// <param name="gameObject">The GameObject to get the path for.</param>
        /// <returns>A string representing the full hierarchical path of this GameObject in the Unity scene.
        /// This is a '/'-separated string where each part is the name of a parent, starting from the root parent and ending
        /// with the name of the specified GameObject itself.</returns>
        [PublicAPI]
        public static string PathFull(this GameObject gameObject)
        {
            return gameObject.Path() + "/" + gameObject.name;
        }

        /// <summary>
        /// Recursively sets the provided layer for this GameObject and all of its descendants in the Unity scene hierarchy.
        /// </summary>
        /// <param name="gameObject">The GameObject to set layers for.</param>
        /// <param name="layer">The layer number to set for GameObject and all of its descendants.</param>
        [PublicAPI]
        public static void SetLayersRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
            gameObject.transform.ForEveryChild(child => child.gameObject.SetLayersRecursively(layer));
        }

        /// <summary>
        /// Hides the GameObject in the Hierarchy view.
        /// </summary>
        /// <param name="gameObject">The GameObject to hide.</param>
        [PublicAPI]
        public static void HideInHierarchy(this GameObject gameObject)
        {
            gameObject.hideFlags = HideFlags.HideInHierarchy;
        }


        /// <summary>
        /// Moves the specified GameObject in the Unity scene hierarchy.
        /// </summary>
        /// <param name="target">The GameObject to move.</param>
        /// <param name="x">The new X position of the GameObject, or null to leave it unchanged.</param>
        /// <param name="y">The new Y position of the GameObject, or null to leave it unchanged.</param>
        /// <param name="z">The new Z position of the GameObject, or null to leave it unchanged.</param>
        /// <param name="relativeTo">The space in which the new position is specified. Default is Self, meaning the position is relative to the GameObject's own coordinate system.</param>
        [PublicAPI]
        public static void Move(this GameObject target,
            float? x = null,
            float? y = null,
            float? z = null,
            Space relativeTo = Space.Self)
        {
            target.transform.Move(x, y, z, relativeTo);
        }
    }
}