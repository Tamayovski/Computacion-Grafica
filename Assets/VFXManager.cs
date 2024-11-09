using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.VFX;
using System.Collections;

public class VFXManager : MonoBehaviour
{
    public GameObject[] vfxPrefabs;          // Array of VFX prefabs
    public TMP_Dropdown vfxDropdown;         // Dropdown to select VFX
    public Button playButton;                // Button to start VFX and animation
    public float effectDuration = 5f;        // Duration for VFX
    public Animator Controlador;             // Reference to the character's Animator called "Controlador"

    private GameObject currentVFX;           // Reference to the current VFX instance
    private VisualEffect currentVFXGraph;    // Reference to the VisualEffect component

    void Start()
    {
        // Populate dropdown with VFX names
        vfxDropdown.options.Clear();
        foreach (GameObject vfx in vfxPrefabs)
        {
            vfxDropdown.options.Add(new TMP_Dropdown.OptionData(vfx.name));
        }

        // Add listener for play button
        playButton.onClick.AddListener(DisplaySelectedVFX);
    }

    void DisplaySelectedVFX()
    {
        int selectedIndex = vfxDropdown.value;

        if (selectedIndex >= 0 && selectedIndex < vfxPrefabs.Length)
        {
            // Destroy any existing VFX instance before creating a new one
            if (currentVFX != null)
            {
                Destroy(currentVFX);
            }

            // Instantiate the selected VFX as inactive
            currentVFX = Instantiate(vfxPrefabs[selectedIndex], Vector3.zero, Quaternion.identity);
            currentVFX.SetActive(false); // Set inactive initially
            currentVFXGraph = currentVFX.GetComponent<VisualEffect>(); // Get the VisualEffect component
            Debug.Log("VFX instantiated but inactive: " + vfxPrefabs[selectedIndex].name);

            // Start the VFX and corresponding animation when Play is pressed
            PlayVFX(selectedIndex);
        }
        else
        {
            Debug.LogWarning("No VFX selected or invalid index!");
        }
    }

    void PlayVFX(int selectedIndex)
    {
        if (currentVFXGraph == null) return;

        Debug.Log("Starting VFX and Animation...");
        currentVFX.SetActive(true); // Activate the VFX GameObject
        currentVFXGraph.SetBool("isPlaying", true); // Start VFX emission

        // Play the corresponding animation based on the selected VFX
        if (Controlador != null)
        {
            // Trigger "RAYO" if the first VFX is selected, and "AURA" if the second one is
            if (selectedIndex == 0)  // Assuming index 0 is the "RAYO" effect
            {
                Controlador.SetTrigger("RAYO"); // Trigger the "RAYO" animation
            }
            else if (selectedIndex == 1)  // Assuming index 1 is the "AURA" effect
            {
                Controlador.SetTrigger("AURA"); // Trigger the "AURA" animation
            }
              else if (selectedIndex == 2)  // Assuming index 1 is the "AURA" effect
            {
                Controlador.SetTrigger("HIELO"); // Trigger the "AURA" animation
            }      
        }

        // Start coroutine to deactivate VFX after the effect's duration
        StartCoroutine(DeactivateAfterDuration(effectDuration));
    }

    private IEnumerator DeactivateAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (currentVFXGraph != null)
        {
            currentVFXGraph.SetBool("isPlaying", false); // Stop emission
            currentVFX.SetActive(false);                  // Deactivate the VFX GameObject
            Debug.Log("VFX finished and deactivated.");
        }

        // Optionally, reset animation trigger states if needed
        if (Controlador != null)
        {
            Controlador.ResetTrigger("RAYO");
            Controlador.ResetTrigger("AURA");
            Controlador.ResetTrigger("HIELO");
            Controlador.SetTrigger("Idle"); // Trigger an idle animation or another fallback state if needed
        }
    }
}
