using UnityEngine;
using System.Collections;

// NPCs whose interactions require custom functions (such as interactions that open doors) should
// subclass Interaction to add new functions to set as callbacks for OnStart and OnEnd
public class Interaction : MonoBehaviour {

    public InteractionComponent root;
    InteractionComponent currentComponent;
    
    // NPCs that require custom displays should override this
    public virtual IEnumerator Display() {
        yield break; // Remove after fully implementing this function

        // Display current component
        // Stay in an empty while loop until the user presses the spacebar
        // Then call LoadInteractionComponent
    }

    // Calling this function starts the interaction
    public void LoadInteractionComponent(){
        // If there's a current component, invoke its OnEnd and then turn off UI

        // Go to next component
        // If it exists, make it the current component and invoke its OnStart

        // Display the component
    }

    // For debugging only
    void Start()
    {
        root = ScriptableObject.CreateInstance<InteractionComponent>();
        root.messages = new string[1];
        root.messages[0] = "test";
    }
}
