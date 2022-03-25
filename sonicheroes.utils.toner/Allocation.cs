using System;
using Reloaded.Memory.Sources;

namespace sonicheroes.utils.toner;

/// <summary>
/// Represents a single memory allocation.
/// </summary>
public struct Allocation : IDisposable
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

    public void Dispose()
    {
        Memory.CurrentProcess.Free(Address);
    }

    /// <summary>
    /// Returns true if the item can fit into the current memory block.
    /// </summary>
    public bool CanItemFit(int itemSize)
    {
        return itemSize <= Size;
    }
}