[![NuGet](https://img.shields.io/nuget/v/InputStateManager.svg?maxAge=2592000)](https://www.nuget.org/packages/InputStateManager/) [![NuGet](https://img.shields.io/nuget/dt/InputStateManager.svg?maxAge=2592000)](https://www.nuget.org/packages/InputStateManager/)
 [![license](https://img.shields.io/github/license/unterrainerinformatik/collisiongrid.svg?maxAge=2592000)](http://unlicense.org)  [![Twitter Follow](https://img.shields.io/twitter/follow/throbax.svg?style=social&label=Follow&maxAge=2592000)](https://twitter.com/throbax)  

# General

This section contains various useful projects that should help your development-process.  

This section of our GIT repositories is free. You may copy, use or rewrite every single one of its contained projects to your hearts content.  
In order to get help with basic GIT commands you may try [the GIT cheat-sheet][coding] on our [homepage][homepage].  

This repository located on our  [homepage][homepage] is private since this is the master- and release-branch. You may clone it, but it will be read-only.  
If you want to contribute to our repository (push, open pull requests), please use the copy on github located here: [the public github repository][github]  

We accept pull requests!

> **If you like this repo, please don't forget to star it.**
> **Thank you.**



# ![Icon](https://github.com/UnterrainerInformatik/InputStateManager/raw/master/icon.png)InputStateManager

This is a PCL library for MonoGame useful for querying keyboard, mouse, touch and gamepad inputs.

Currently it provides convenience-methods in a fluent manner for:

* Keyboard (Key)
  Sub-context for 'Is' and 'Was', helper functions for 'Any-ALT (...SHIFT, CTRL)' and NumLock and CapsLock.

* Mouse (Mouse)
  Sub-context for 'Is' and 'Was' and delta-functions for mouse-wheel, position, etc...

* GamePad (Pad)
  Sub-context for 'Is' and 'Was' and for 'DPad', 'Triggers' and 'ThumbSticks'.

* TouchPanel (Touch)
  Sub-context for 'Is' and 'Was'. Currently only exposes TouchCollections.

  ​

## Usage

### Setup

```c#
// Due to a naming-clash you'll need the following line in your imports:
using Mouse = InputStateManager.Inputs.Mouse;

// Now lets create a manager for our inputs:
private readonly InputManager input = new InputManager();
...

/// <summary>
///     Allows the game to run logic such as updating the world,
///     checking for collisions, gathering input, and playing audio.
/// </summary>
/// <param name="gameTime">Provides a snapshot of timing values.</param>
protected override void Update(GameTime gameTime)
{
  if (input.Pad.Is.Press(Buttons.Back) || input.Key.Is.Press(Keys.Escape))
    Exit();
  // Update the manager so that it can record changes:
  input.Update();
  base.Update(gameTime);
}
...
```

### Mouse

```c#
// Somewhere in your code:

if (input.Mouse.Is.Press(Mouse.Button.LEFT)) {...}
  // ...which is equivalent to:
  if(input.Mouse.Is.Down(Mouse.Button.LEFT) && 
     input.Mouse.Was.Up(Mouse.Button.LEFT))) {...}
// Or:
if (input.Mouse.Is.Release(Mouse.Button.LEFT)) {...}
  // ...which is equivalent to:
  if(input.Mouse.Is.Up(Mouse.Button.LEFT) && 
     input.Mouse.Was.Down(Mouse.Button.LEFT))) {...}
// If you want to see if both buttons are currently down:
if (input.Mouse.Is.Down(Mouse.Button.Left, Mouse.Button.Right)) {...}
// Or, for example, if you wanna see if one of those is down:
if (input.Mouse.Is.OneDown(Mouse.Button.Left, Mouse.Button.Right)) {...}

var positionDelta = input.Mouse.Is.PositionDelta;
var scrollWheelDelta = input.Mouse.Is.ScrollWheelDelta;
// Or, if you want the values of the last update:
if (input.Mouse.Was.Up(Mouse.Button.MIDDLE)) {...}
...
```

### Keyboard

```c#
// Somewhere in your code:

if (input.Key.Is.Press(Keys.Escape)) {...}
// Register keys that are currently held down:
if (input.Key.Is.Down(Keys.A) && input.Key.Is.Down(Keys.B)) {...}
// Same as:
if (input.Key.Is.Down(Keys.A, Keys.B)) {...}
// Or:
if (input.Key.Is.Down(new []{Keys.A, Keys.B})) {...}
// Register key-presses (down -> up):
if (input.Key.Is.Press(Keys.A) || input.Key.Is.Press(Keys.B)) {...}
// Same as:
if (input.Key.Is.OnePress(Keys.A, Keys.B)) {...}
// Or:
if (input.Key.Is.OnePress(new []{Keys.A, Keys.B})) {...}
// Query for any SHIFT-key pressed:
if (input.Key.Is.Down(Keys.A) && input.Key.Is.ShiftDown) {...}
// Same for CTRL and ALT:
if (input.Key.Is.CtrlRelease() || input.Key.Is.AltPress()) {...}
// And NumLock- and CapsLock-states:
if (input.Key.Is.NumLockOn() && input.Key.Is.CapsLockOff()) {..}
// And the transitions of those states:
if (input.Key.Is.NumLockStateExit() || input.Key.Is.CapsLockStateEnter()) {..}
// Or their respective key-presses or up-down values:
if (input.Key.Is.Down(Keys.NumLock) || input.Key.Is.Release(Keys.CapsLock))
...


```

### GamePad

```c#
// Somewhere in your code:
if (input.Pad.Is.Press(Buttons.A, PlayerIndex.One)) {...}
// For prototypes the PlayerIndex defaults to One on all calls:
if (input.Pad.Is.Press(Buttons.A)) {...} //Same as above
// In a real game you'd have a variable as PlayerIndex, like:
PlayerIndex p;
// Then, for example:
if (input.Pad.Is.Connected(p) && input.Pad.Is.Release(Buttons.Back, p)) {...}
// Or to catch the event of a GamePad being connected:
if (input.Pad.Is.JustConnected(p)) {...}
// Or:
if (input.Pad.Is.DPad.Press(Pad.DPadDirection.LEFT, p) {...}
// Or for the thumb-sticks (directional vector2):
Vector2 thumbPos = input.Pad.Is.ThumbSticks.Left();
Vector2 thumbDelta = input.Pad.Is.ThumbSticks.RightDelta(p);
// Or for the triggers (%pressed as float):
float triggerPos = input.Pad.Is.Triggers.Right();
float triggerDelta = input.Pad.Is.Triggers.LeftDelta();
...
```

### TouchPanel

```c#
// Somewhere in your code:

// For now the touchPanel implementation only provides the
// TouchCollection for 'Is' and 'Was':
var touchCollection = input.Touch.Is.Collection;
var oldTouchCollection = input.Touch.Was.Collection;
```

Delta functions like fields post-fixed with 'Delta' like 'MouseWheelDelta' or the functions Press() and Release() are not available in 'Was' context since there is no state to compare to.

In Addition to those functions it exposes the current state (State) and the old state (OldState) as well. So you can use those as you are used to.

[homepage]: http://www.unterrainer.info
[coding]: http://www.unterrainer.info/Home/Coding
[github]: https://github.com/UnterrainerInformatik/InputStateManager
