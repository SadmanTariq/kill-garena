> This project is now archived since League of Legends in SEA is not run by Garena anymore.

# KillGarena
The Garena launcher has a background service running all the time even when the launcher is closed. No one knows what this service does and the launcher seems to work fine without it. I wrote this service to kill Garena's service automatically because I don't trust them.


## Installing
1. Download a release zip file from the [releases page](https://github.com/SadmanTariq/kill-garena/releases/).
2. Extract it to a suitable location. Make sure that this location is final as you will have to uninstall first before moving.
3. Run `Install.bat`.

## Uninstalling
* Run `Uninstall.bat` to uninstall.

## Disabling gxx speed launcher task
Garena also has a startup task in addition to the service for some reason. Here's how to disable it.
1. Open start menu and search for Task Scheduler.
2. Click on *Task Scheduler Library* from the left pane.
3. Find `gxx speed launcher` in the list.
4. Right click on it then click disable.
