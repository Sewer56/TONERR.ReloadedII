using System;
using System.Collections.Generic;
using System.Text;
using Reloaded.Memory.Sources;

namespace sonicheroes.utils.toner
{
    /// <summary>
    /// Represents a single memory allocation.
    /// </summary>
    public class Allocation : IDisposable
    {
        public IntPtr Address { get; set; }
        public int    Size    { get; set; }

        /// <summary>
        /// Allocates a new region of memory with a given size.
        /// </summary>
        /// <param name="size">Size of the allocation.</param>
        public Allocation(int size)
        {
            Address = Memory.CurrentProcess.Allocate(size);
            Size = size;
        }

        ~Allocation()
        {
            Dispose();
        }

        public void Dispose()
        {
            Memory.CurrentProcess.Free(Address);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns true if the item can fit into the current memory block.
        /// </summary>
        public bool CanItemFit(int itemSize)
        {
            return itemSize <= Size;
        }
    }
}
