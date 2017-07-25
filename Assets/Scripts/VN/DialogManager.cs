using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

	public static DialogManager Get_Dialog_Manager() {
		return (DialogManager) HushPuppy.safeFindComponent("GameController", "DialogManager");
	}

	void Start() {
		StartCoroutine(Show_Ink(inkFile));
	}

	void Update() {
		if (Input.GetButtonDown("Fire1") && dialog_active) {
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

		skip_display = false;
		text_running = false;
		dialogText.text = text;
	}

	#region choices

		bool choice_selected = false;

		IEnumerator Display_Choices(List<Choice> choices) {
			for (int i = 0; i < choices.Count; i++) {
				GameObject choice = Instantiate(choicePrefab, choiceContainer.transform, false);
				if (i == 0) {
					yield return new WaitForEndOfFrame();
					choice.GetComponentInChildren<Button>().Select();
				}

				choice.GetComponentInChildren<ChoiceBox>().Initialize(i);
				choice.GetComponentInChildren<TextMeshProUGUI>().text = choices[i].text;
			}
			
			yield return new WaitUntil(() => choice_selected);
			choice_selected = false;
		}

		public void Select_Choice(int index) {
			if (inkStory.currentChoices.Count > 0) {
				inkStory.ChooseChoiceIndex(index);
				choice_selected = true;
			}

			HushPuppy.destroyChildren(choiceContainer);
		}

	#endregion
}
