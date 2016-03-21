using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class InteractionComponent : ScriptableObject {

    public UnityEvent OnStart;

    public string[] messages; // Multiple elements for making choices
    // Add a field for an image file here once pictures in messages are implemented

    public UnityEvent OnEnd;

    public InteractionComponent[] children; // Multiple elements for branching conversations

    // If only one child, don't indent the children of the array in the property drawer
}
