# Command Management System

__Current Version:__ 3.0.0-alpha2

    This is a nightly build that is actively under development. It should not be used for production systems. More detailed information can be found on GitHub.

## New Features
 * __Implement Alias function:__ The alias functionality is finally implemented. CommandAttributes can now get additional tags as aliases that can be used instead of the normal tags.

## Changes

 * __Add Generics to DefaultManager:__ The Default Manager now has overloads with generics so it can be configured individually.
 * __Add list of Command tags:__ A CommandManager can now return a list of the executable commands and their aliases.

## BugFixes

 * __Invalid Cast Exception:__ When using the DefaultCommandManager it could happen that a Dispatch led to a Cast Exception. The error has been fixed.
 * __DefaultManager does not find any commands:__ The DefaultManager could not find any assembly commands. In this version the DefaultManager now searches for  the namespaces in the current app domain.

---

__Current Version:__ 3.0.0-alpha1

    This is a nightly build that is actively under development. It should not be used for production systems. More detailed information can be found on GitHub.

## New Features

* __Reinstaning Complex Commands:__ Complex commands are recreated after they are finished with the next dispatch. This behavior can be controlled by a Reinstance property of the CommandAttribute.
* __.NET Standard 2.0 support:__ The new version is available in three versions:. NET 4.5,. NET 4.6 and. NET Standard 2.0

## Known issues

* __Aliases are ignored:__ The alias function has no functionality

## Changes

* __CommandManager.Command_FinishEvent:__ This function is obsolete, please use instead CommandFinishEvent
* __CommandManager.Command_WaitEvent:__ This function is obsolete, please use instead CommandWaitEvent