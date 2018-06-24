using UnityEngine;

public class Hacker : MonoBehaviour {
	
	// Game Configuration Data
	string[] level1Passwords = {"books", "aisle", "shelf", "password", "font", "borrow"};
	string[] level2Passwords = {"prisoner", "handcuffs", "holster", "uniform", "arrest"};
	string[] level3Passwords = {"starfield", "telescope", "environment", "exploration", "astronauts"};
	const string menuHint = "You may type menu at any time";


	// Game State
	int level;
	enum Screen { MainMenu, Password, Win };
	Screen currentScreen;
	string password;


	// Use this for initialization
	void Start () {
		ShowMainMenu();
	}
	
	void ShowMainMenu() {
		currentScreen = Screen.MainMenu;		
		level = 0;
		Terminal.ClearScreen();
		Terminal.WriteLine("What would you like to hack into?");
		Terminal.WriteLine("Press 1 for the local library");
		Terminal.WriteLine("Press 2 for the police station");
		Terminal.WriteLine("Press 3 for NASA \n");
		Terminal.WriteLine("Enter your selection:");
	}

	void OnUserInput(string input) {
		if (input == "menu") {
			ShowMainMenu();
		} else if (currentScreen == Screen.MainMenu) {
			RunMainMenu(input);
		} else if (currentScreen == Screen.Password){
			CheckPassword(input);
		} else if (currentScreen == Screen.Win) {
			// Do nothing
		}
	}

	void RunMainMenu(string input) {
		bool isValidLevelNumber = (input == "1" || input == "2" || input == "3");
		if (isValidLevelNumber) {
			level = int.Parse(input);
			AskForPassword();
		} else if (input == "007") { // easter egg
			Terminal.WriteLine("Please enter a selection Mr Bond:");
		} else {
			Terminal.WriteLine("Please enter a valid selection:");
		}
	} 

	void AskForPassword() {
		currentScreen = Screen.Password;
		Terminal.ClearScreen();		
		SetRandomPassword();
		Terminal.WriteLine("Enter a password: HINT " + password.Anagram());
		Terminal.WriteLine(menuHint);
	}

	void SetRandomPassword() {
		switch(level) {
			case 1:
				password = level1Passwords[Random.Range(0, level1Passwords.Length)];
				break;
			case 2:
				password = level2Passwords[Random.Range(0, level2Passwords.Length)];
				break;
			case 3:
				password = level3Passwords[Random.Range(0, level3Passwords.Length)];
				break;
			default:
				Debug.LogError("Invalid level number");
				break;
		}
	}

	void CheckPassword(string input) {
		if (input == password) {
			DisplayWinScreen();
		} else {
			AskForPassword();
		}
	}

	void DisplayWinScreen() {
		currentScreen = Screen.Win;
		Terminal.ClearScreen();
		ShowLevelReward();
	}

	void ShowLevelReward() {
		switch(level) {
			case 1:
				Terminal.WriteLine("Congratulations on clearing level 1!");
				break;
			case 2:
				Terminal.WriteLine("Congratulations on clearing level 2!");
				break;
			case 3: 
				Terminal.WriteLine("Congratulations on clearing level 3!");
				break;
		}
		Terminal.WriteLine("Enter \"menu\" to try again");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
