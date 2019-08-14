<div align="center">
	<h1>Heroes Essentials: TONERR</h1>
	<img src="https://i.imgur.com/BjPn7rU.png" width="150" align="center" />
	<br/> <br/>
	<strong>The One Expandonger Reloaded Reloaded</strong>
    <p>What else did you think TONERR stood for?</p>
<b>Id: sonicheroes.utils.toner</b>
</div>

# Prerequisites
The mod uses the [Hooks Shared Library](https://github.com/Sewer56/Reloaded.SharedLib.Hooks).
Please download and extract that mod first.

# About This Project

The following project is a [Reloaded II](https://github.com/Reloaded-Project/Reloaded-II) Mod Loader mod that allows you to load arbitrarily large files from inside ONE archives. (up to ~2GB)

## How to Use
**A.** Install Reloaded mods as usual. (Extract to mod directory)

**B.** Enable mod and run the game.

# The Problem Under the Hood

When Sonic Heroes extracts files from ONE Archives, it allocates a fixed amount of memory to which the individual files are to be decompressed to. 

Normally this is a non-issue, however once mods and custom files are involved, larger than the originals, the preallocated buffers may become too small for the custom files causing reads in unallocated memory. 

TONERR solves by hooking every single function used to read a file from a ONE archive used by the game. Using my own fast PRS Compression/Decompression library, it calculates the size of the file without decompressing it and provides a new buffer of sufficient size to decompress the file to.

To avoid unnecessary heap allocations, one buffer is reused for all files. This buffer is resized every time a file larger than it shows up.

# Original TONER

The original TONER used a slightly different approach, you can read a more in-depth explanation about the problem and how the original TONER tackled it here: https://github.com/Sewer56/TONER

In comparison to the original, this is a more "dumb TONER", it sacrifices RAM Usage for CPU, with less heap allocations in particular.
