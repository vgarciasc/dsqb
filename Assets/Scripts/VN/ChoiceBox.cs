using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceBox : MonoBehaviour {

	[SerializeField]
	int choice_ID;

	public void Initialize(int choice_index) {
		this.choice_ID = choice_index;
	}

	public void Send_Choice() {
		DialogManager dm = DialogManager.Get_Dialog_Manager();
		dm.Select_Choice(choice_ID);
	}
}
