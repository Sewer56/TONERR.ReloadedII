﻿using System.Runtime.InteropServices;

namespace sonicheroes.utils.toner.Heroes.One.Structures.ArchiveStructures
{
    /// <summary>
    /// Defines the file name of an individual file inside a .ONE archive.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = FileNameLength)]
    public unsafe struct OneFileName
    {
        /// <summary>
        /// Contains the length of each filename.
        /// </summary>
        public const int FileNameLength = 64;

        /// <summary>
        /// Contains the actual filename of the file as ASCII encoded bytes.
        /// </summary>
        public fixed byte Name[FileNameLength];

        /// <summary>
        /// Constructor which copies a string into the fixed length buffer.
        /// </summary>
        /// <param name="name">The name to use for this ONE file.</param>
        public OneFileName(string name)
        {
            fixed (byte* fileNamePointer = Name)
                Utilities.AsciiStringToCharPointer(name, fileNamePointer);
        }

        /// <summary>
        /// Gets the name of the current ONE file as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            fixed (byte* fileName = Name)
                return Utilities.CharPointerToAsciiString(fileName);
        }
    }
}