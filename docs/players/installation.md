# Installation 

## Windows

* Download the [latest BepInEx 5 release here](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.2).
    * BepInEx 6 will **not** work.
    * Choose the right version depending on your machine architecture (in 99% of the case, choose "BepInEx_win_x64_5.4.23.2.zip")
* Extract the BepInEx archive content into the root of the game directory
    * e.g. on Windows, you should have a `BepInEx` folder alongside `Minishoot.exe`, in a folder called `Windows`.
* Launch the game a first time. This will allow BepInEx to create all necessary files. You can close the game afterwards when you arrive at the main menu.
* **IMPORTANT** : For the randomizer to work, you need to modify the configuration file of BepInEx. Once the game is closed, go to `<game-root-directory>/BepInEx/config` and edit the `BepInEx.cfg` file (for example, with Notepad).
    * After that, search for the line containing `HideManagerGameObject` (it should be near the top of the file by default), and ensure that its value is set to `true`.
* Download the randomizer [here](https://github.com/TheNooodle/MinishootRandomizer/releases).
* Extract `MinishootRandomizer.zip` in `<game-root-directory>/BepInEx/plugins`.
    * You should have a `MinishootRandomizer` directory in the `plugins` folder, with some `.dll` files in the former.
* Launch the game. Once on the title screen, you should see a window in the top-left titled "Randomizer Menu". This means the mod was successfully installed.
    * Please note that the game might be slower to start, due to the randomizer doing some bootstrapping work before letting the game start.

## MacOS

Thanks to @origamiswami for the install procedure on MacOS.

(These steps got it working on a MacBook Pro, Apple M1 chip, MacOS Sequoia 15.3.1)

* Download BepInEx 5.4.21.0 for Unix https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21

* Move the contents of the download to the `Minishoot' Adventures/` folder (find it by going to "Browse Local Files" in Steam).
* You should now have a folder called `BepInEx/`, a folder called `doorstop_libs/`, a txt file called `changelog.txt` and a file called `run_bepinex.sh` alongside the pre-existing folder `Contents/`.

* Modify the contents of `run_bepinex.sh` with the content of [this file](./run_bepinex.sh)

* In Terminal, run the following commands:
```sh
# navigate to the folder containing the Minishoot executable file
cd /Users/<UserName>/Library/Application\ Support/Steam/steamapps/common/Minishoot\'\ Adventures/Contents/MacOS
# change permissions to allow running the shell script
chmod +x /Users/<UserName>/Library/Application\ Support/Steam/steamapps/common/Minishoot\'\ Adventures/run_bepinex.sh
# run the shell script
/Users/<UserName>/Library/Application\ Support/Steam/steamapps/common/Minishoot\'\ Adventures/run_bepinex.sh
```

* You will see a few popups saying `libdoorstop_x64` cannot be opened. Click through these (don't move it to trash), then open System Settings and go to Privacy and Security. Scroll down to the Security section, and there should be an option to give permission to run that file.
* Run the shell script again (see step 3). Now, it should launch Minishoot and BepInEx will create the necessary files. Close Minishoot from the main menu.
* **IMPORTANT** : For the randomizer to work, you need to modify the configuration file of BepInEx. Once the game is closed, go to `<game-root-directory>/BepInEx/config` and edit the `BepInEx.cfg` file (for example, with Notepad).
    * After that, search for the line containing `HideManagerGameObject` (it should be near the top of the file by default), and ensure that its value is set to `true`.
* Download the randomizer [here](https://github.com/TheNooodle/MinishootRandomizer/releases).
* Extract `MinishootRandomizer.zip` in `<game-root-directory>/BepInEx/plugins`.
    * You should have a `MinishootRandomizer` directory in the `plugins` folder, with some `.dll` files in the former.

* Go to "Properties" from Minishoot's Library page in Steam (click the gear icon). In "Launch Options", enter the following:
```sh
/Users/<UserName>/Library/"Application Support"/Steam/steamapps/common/"Minishoot' Adventures"/run_bepinex.sh %command%
```

* You can now launch the game from Steam ! The loading screen will blink a few times, but after it loads you will see the randomizer menu in the top left.

## Linux

This is tested on Arch and aims at the [Steam version](https://store.steampowered.com/app/1634860/Minishoot_Adventures/) of the game.

* Download the `win_x64` version of [BepInEx 5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.4).
* Extract the content of the .zip in the games root folder where you find `Minishoot.exe`.
    
    - `BepInEx` folder 
    - `changelog.txt`
    - `doorstep_config.ini`
    - `winhttp.dll`

* Make sure that you have [protontricks](https://github.com/Matoking/protontricks?tab=readme-ov-file#installation) downloaded.
    
    - Pick the right community package for your distro, if available.
    - If not, pick the [flathub](https://flathub.org/en/apps/com.github.Matoking.protontricks) version.

* Open `protontricks`.
    - If you installed it via a community package, open a terminal and type:

       ```
       protontricks --gui
       ```

* A window should open that asks for a Steam app.
    
    - Pick `Minishoot' Adventures` and press ok.
    - Now it asks for a **wineprefix**, pick `Select the default wineprefix`.
    - Another window opens and asks **What would you like to do with this wineprefix?**, select `Run winecfg`.

* Now the **Wine configuration** window opens.

    - Select the **Libraries** tab
    - In the **New override for library** dropbox, search for `winhttp` and click the `Add` button right next to it.
    - It now appears in the **Existing overrides** dropbox.
    - Now press **Apply** and close the window.

* `BepInEx` needs to create all necessary folder and files, launch the game and wait till you are in the main menu and close it.
    
    - Now you should see more folders and files in the `BepInEx` directory.
        - `cache`
        - `config`
        - `core`
        - `patchers`
        - `plugins`
        - `LogOutput.log`
        
    - Navigate to the `config` folder and open the `BepInEx.cfg`.
        - Change the value of `HideManagerGameObject` to `true`.
        - Save the changes and close the file.

* Download the [MinishootRandomizer](https://github.com/TheNooodle/MinishootRandomizer/releases).

    - Extract the .zip into the `plugins` folder of `BepInEx`.

* Now you can open the game trough Steam and if everything works, a window in the top left corner should appear and you can connect to your AP room. 
