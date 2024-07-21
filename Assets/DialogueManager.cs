using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private DialogueBox dialogueBox;
    [SerializeField]
    private float dialogueSoundInterval;
    [SerializeField]
    private AudioSource[] dialogueSounds;
    private int currSourceIdx;
    private AudioSource currAudioSource;

    public static DialogueManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        dialogueSounds = GetComponentsInChildren<AudioSource>();
        CycleAudioSource();
        // StartCoroutine(RepeatSound(dialogueSoundInterval, 10f));
    }

    private void PlaySound() {
        float pitch = Random.Range(0.8f, 1.2f);
        currAudioSource.pitch = pitch;
        currAudioSource.Play();
    }

    private void CycleAudioSource() {
        currSourceIdx = (currSourceIdx + 1) % dialogueSounds.Length;
        currAudioSource = dialogueSounds[currSourceIdx];
    }

    IEnumerator OutputString(string input, float letterInterval) {
        dialogueBox.gameObject.SetActive(true);
        for (int i=0;i<input.Length;i++) {
            dialogueBox.AddLetter(input[i]);

            // Dont make sound for spaces
            if (input[i] == ' ') {
                continue;
            }

            PlaySound();
            CycleAudioSource();

            yield return new WaitForSeconds(letterInterval);
        }
        yield return new WaitForSeconds(1f);
        dialogueBox.EraseText();
        dialogueBox.gameObject.SetActive(false);
    }

    public void SayString(string input) {
        StartCoroutine(OutputString(input, dialogueSoundInterval));
    }
}
