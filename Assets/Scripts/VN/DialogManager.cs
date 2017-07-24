using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogManager : MonoBehaviour {
	
	[Header("Choices")]
	[SerializeField]
	GameObject choiceContainer;
	[SerializeField]
	GameObject choicePrefab;

	[Header("Dialog")]
	[SerializeField]
	TextMeshProUGUI dialogText;
	[SerializeField]
	TextAsset inkFile;

	Story inkStory;
	bool skip_display = false;
	bool next_dialog = false;
	bool text_running = false;
	bool dialog_active = false;

	void Start() {
		StartCoroutine(Show_Ink(inkFile));
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.E) && dialog_active) {
			if (text_running) {
				skip_display = true;
			}
			else {
				next_dialog = true;
			}
		}
	}

	IEnumerator Show_Ink(TextAsset file) {
		dialog_active = true;
		inkStory = new Story(inkFile.text);		

		while (true) {
			if (!inkStory.canContinue) {
				if (inkStory.currentChoices.Count == 0) {
					break;
				}
				else {
					yield return Display_Choices(inkStory.currentChoices);
					continue;
				}
			}

			var verse = inkStory.Continue();
			
			yield return Display_String(verse, 2);

			yield return new WaitUntil(() => next_dialog);

			next_dialog = false;
		}

		dialog_active = false;
	}

	IEnumerator Display_String(string text, int speed) {
		int current_character = 0;
		text_running = true;

		while (true) {
			if (current_character == text.Length ||
				skip_display) {
				break;
			}

			dialogText.text = text.Substring(0, current_character++);
			yield return HushPuppy.WaitForEndOfFrames(speed);
		}

		text_running = false;
		dialogText.text = text;
	}

	IEnumerator Display_Choices(List<Choice> choices) {
		yield break;
	}

	public void Select_Choice(int index) {

	}
}
