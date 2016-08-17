using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class PlayerBattleController : Battler {

	bool choosing;

	// Prototype-Only, controls focus.
	bool inMenu;

	// This is what contains everything.
	public GameObject CombatUIPanel;

	// This contains the four "Special Move" buttons.
	public GameObject CombatButtonsPanel;
	// This contains the top "Special Move" buttons.
	public GameObject CombatTopButton;

	public GameObject MenuButtonsPanel;
	public GameObject MenuTopButton;

	// Used to update range text.
	public Text Range;

	// Prototype one only shows four attacks, so the last three arrays are useless.
	// USELESS!
	string[] DefaultMenu;
	string[] AttackCategoryMenu;
	string[] AttackListClose;
	string[] AttackListMiddle;
	string[] AttackListLong;
	// USELESS!

	void Start()
	{
		// Gives "choosing" a value other than null.
		choosing = false;

		inMenu = true;
		// Hides the panel containing all the combatui elements.
		CombatUIPanel.SetActive (false);
		CombatButtonsPanel.SetActive (false);

		// Will remain hidden until CombatUIPanel becomes active.
		MenuButtonsPanel.SetActive (true);
	}

    public override IEnumerator ChooseAction(Action Finish)
	{
		CombatUIPanel.SetActive(true);
		// This triggers the choice GUI.

		choosing = true;
		yield return new WaitUntil (() => !choosing);
		choosing = false;
		CombatUIPanel.SetActive (false);

		// One second for the message to disappear.
		yield return new WaitForSeconds(1);
        // Until GUI is implemented, automatically choose Basic Attack.
        DoAction = BasicAttack;

		// TODO: Animations for attack.
		// AnimateMethod(DoAction, ref bool)
		// yield return new WaitUntil(()=>bool)

		// In place of animations, there is a 2 second wait.
		yield return new WaitForSeconds(2);

        GameObject enemy = GameObject.FindWithTag("Enemy");
        singleAttackTarget = enemy.transform.parent.gameObject.GetComponent<Battler>();

        Finish();
    }
		

	// Triggered when player selects the initial Top Button("Basic Attack").
	public void BasicAttackButtonPress()
	{
		//
		// TODO: Create "Basic" Attack, it will be assigned here.
		//

		//DoAction = BasicAttack;
		choosing = false;
		StartCoroutine(CombatUI.Instance.DisplayMessage("Basic Attack Chosen", 1));
	}

	// Triggered when player selects the initial Bottom Button("Special Attacks").
	public void SpecialAttacksButtonPress()
	{
		CombatButtonsPanel.SetActive (true);
		MenuButtonsPanel.SetActive (false);

		inMenu = false;

		EventSystem.current.SetSelectedGameObject(CombatTopButton);
	}


	// Triggered when player selects the Top Button ("Hyper").
	public void TopButtonPress()
	{
		//
		// TODO: Create "Hyper" attack, it will be assigned here.
		//

		//DoAction = Hyper;
		choosing = false;
		StartCoroutine(CombatUI.Instance.DisplayMessage("Hyper", 1));
	}

	// Triggered when player selects the Top-Middle Button ("Heal").
	public void MiddleTopButtonPress()
	{
		// 
		// TODO: Create "Heal" attack, it will be assigned here.
		//

		//DoAction = Heal;
		choosing = false;
		StartCoroutine(CombatUI.Instance.DisplayMessage("Heal", 1));
	}

	// Triggered when player selects the Bottom-Middle Button ("Resolve").
	public void MiddleBottomButtonPress()
	{
		//
		// TODO: Create "Resolve" attack, it will be assigned here.
		//

		//DoAction = Resolve;
		choosing = false;
		StartCoroutine(CombatUI.Instance.DisplayMessage("Resolve", 1));
	}

	// Triggered when player selects the Bottom Button ("Back").
	public void BottomButtonPress()
	{
		MenuButtonsPanel.SetActive (true);
		CombatButtonsPanel.SetActive (false);

		inMenu = true;

		EventSystem.current.SetSelectedGameObject(MenuTopButton);
	}

	void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == null) {
			if(inMenu)
				EventSystem.current.SetSelectedGameObject(MenuTopButton);
			else
				EventSystem.current.SetSelectedGameObject(CombatTopButton);
		}
	}
}
