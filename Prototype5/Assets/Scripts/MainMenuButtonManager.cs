using UnityEngine;

public class MainMenuButtonManager : MonoBehaviour
{
	[SerializeField] private MainMenuManager.MainMenuButtons _buttonType;

	public void ButtonClicked()
	{
		MainMenuManager._manager.MainMenuButtonClicked(_buttonType);
	}

}
