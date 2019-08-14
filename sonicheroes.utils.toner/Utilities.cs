using System;
using System.Collections.Generic;
using System.Text;

namespace sonicheroes.utils.toner
{
    public static class Utilities
    {
        /// <summary>
        /// Converts a fixed array of null terminated chars into a string instance.
        /// </summary>
        public static unsafe string CharPointerToAsciiString(byte* fileName)
        {
            int fileNameLength = 0;
            while (true)
            {
                if (fileName[fileNameLength] == 0)
                    break;

                fileNameLength += 1;
            }

            return Encoding.ASCII.GetString(fileName, fileNameLength);
        }

        /// <summary>
        /// Writes a string to a specified char pointer in ASCII format.
        /// </summary>
        /// <param name="text">The text to write to the pointer.</param>
        /// <param name="pointer">The pointer to write to.</param>
        public static unsafe void AsciiStringToCharPointer(string text, byte* pointer)
        {
            byte[] asciiText = Encoding.ASCII.GetBytes(text);

            for (int x = 0; x < asciiText.Length; x++)
                pointer[x] = asciiText[x];
        }
    }
}
