# Consolix

Console applications are quick way to have something done either as a supporting tool in bigger project or something handy to run from time-to-time. Unfortunately the possibilities of console application and default Visual Studio template leaves space for many improvements, which are covered in this console extensions package.

There are numerous packages covering some of aspects (color console, spinner, progress bar, input helpers, command and parameter parsers) and they are more extensive in that one thing they do. This package takes minimum necessary for simple tooling/app build and packs all these things in one convenient package.

To use commands and parameter parsing/mapping to command properties, package expects console application to be created as hosted application (introduced in .Net Core), using dependency injection and other possibilities usually available by default in ASP.NET applications. This includes logging availability and configuration file usage, too.

[![Build](https://github.com/salixzs/Consolix/actions/workflows/build.yml/badge.svg?branch=main)](https://https://github.com/salixzs/Consolix/actions/workflows/build.yml) [![Nuget version](https://img.shields.io/nuget/v/Consolix.svg)](https://www.nuget.org/packages/Consolix/) [![NuGet Downloads](https://img.shields.io/nuget/dt/Consolix.svg)](https://www.nuget.org/packages/Consoix/)


# Package provides:

* Creating [console commands](https://github.com/salixzs/Consolix/wiki/Commands) as separate classes based on provided interface:
  * Necessary command [parameter wiring via attribute](https://github.com/salixzs/Consolix/wiki/Parameters) use on command class properties (read/mapped from command line or from configuration file).
  * Automated validation of parameters and their presence (if mandatory).
  * Automated command(s) on-screen help generation.
  * One (default to run) or multiple command (specify in command line) possibility.
* [Output helpers](https://github.com/salixzs/Consolix/wiki/Output-helpers) (with colors!):
  * [Simple output coloring](https://github.com/salixzs/Consolix/wiki/Colored-output) (both foreground and background colors).
  * String.Format placeholders bi-coloring.
  * Clear console line, overwrite current console line.
  * Setting predefined [color scheme](https://github.com/salixzs/Consolix/wiki/Color-schemes) from 4 available in package.
  * [Simple menu](https://github.com/salixzs/Consolix/wiki/Input-helpers#menu).
  * [Busy indicator (spinner)](https://github.com/salixzs/Consolix/wiki/Spinner) for long process visualization (does not block working thread).
    * Working status messages along with spinner.
    * Elapsed time display for process.
    * Inline Progress bar.
  * [Progress bar](https://github.com/salixzs/Consolix/wiki/Progress-bar)
    * Based on currentStep/totalSteps (not percentage!).
    * Shows execution time along with % done.
* [Input helpers](https://github.com/salixzs/Consolix/wiki/Input-helpers):
  * Password input (entered characters replaced with asterisk (*) on screen).
  * Wait for specific keys.
  * Wait for Enter
  * Wait for Escape.
  * Wait for y/n.

# Examples

#### Spinner (here growing arrow) with elapsed time and status messages
![Spinner with time and status messages](https://raw.githubusercontent.com/wiki/salixzs/Consolix/spinner_time.gif)

#### Progress bar
![Progress bar](https://raw.githubusercontent.com/wiki/salixzs/Consolix/progress_bar.gif)

#### Menu
![Simple menu](https://raw.githubusercontent.com/wiki/salixzs/Consolix/menu.gif)

#### Output coloring
![Output coloring](https://raw.githubusercontent.com/wiki/salixzs/consolix/string_format.jpg)

#### Command auto-help
![Command auto-help](https://raw.githubusercontent.com/wiki/salixzs/consolix/command-args-help.jpg)

#### Color scheme choices (here: Raspberry)
![Raspberry color scheme](https://raw.githubusercontent.com/wiki/salixzs/consolix/raspberry.jpg)

# Documentation

[Repository Wiki](https://github.com/salixzs/Consolix/wiki) has usage documentation and code snippets for package use.

# Demo (Sample) project

Repository includes a [sample (Demo) project](https://github.com/salixzs/Consolix/tree/main/Sample) to visualize its possibilities and usage patterns.