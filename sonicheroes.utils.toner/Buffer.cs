namespace sonicheroes.utils.toner;

public unsafe class Buffer : IDisposable
{
    /// <summary>
    /// Retrieves the size of the current buffer allocation.
    /// </summary>
    public int AllocationSize => _memoryAllocation.Size;

    /// <summary>
    /// Allocation for game files of abnormal size (HD Textures etc).
    /// </summary>
    private Allocation _largeObjectAlloc;

    /// <summary>
    /// Allocation for game files of regular size.
    /// </summary>
    private Allocation _memoryAllocation = new Allocation(0x100000); // 1 MiB

    public void Dispose()
    {
        _memoryAllocation.Dispose();
    }

    /// <summary>
    /// Returns an address of a memory buffer capable of fitting a given size.
    /// </summary>
    /// <returns>True if this is a large object allocation and should be freed with <see cref="FreeLargeObject"/>.</returns>
    public bool Get(int size, out void* address)
    {
        if (_memoryAllocation.CanItemFit(size))
        {
            address = (void*)_memoryAllocation.Address;
            return false;
        }

        // Note the finalizer inside Allocation.
        // After a period of time, the GC will manually cleanup the old memory.
        _largeObjectAlloc = new Allocation(size);
        address = (void*)_largeObjectAlloc.Address;
        return true;
    }

    /// <summary>
    /// Frees the large object heap allocation.
    /// </summary>
    public void FreeLargeObject() => _largeObjectAlloc.Dispose();
}