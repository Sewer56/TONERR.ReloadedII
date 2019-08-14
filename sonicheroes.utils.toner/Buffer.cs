using System;
using System.Collections.Generic;
using Reloaded.Memory.Sources;

namespace sonicheroes.utils.toner
{
    public unsafe class Buffer : IDisposable
    {
        /// <summary>
        /// Retrieves the size of the current buffer allocation.
        /// </summary>
        public int AllocationSize => _memoryAllocation.Size;

        private Allocation _memoryAllocation     = new Allocation(0x100000); // Start with 1 MiB

        public void Dispose()
        {
            _memoryAllocation?.Dispose();
        }

        /// <summary>
        /// Returns an address of a memory buffer capable of fitting a given size.
        /// </summary>
        public void* Get(int size)
        {
            if (_memoryAllocation.CanItemFit(size))
                return (void*)_memoryAllocation.Address;

            // Note the finalizer inside Allocation.
            // After a period of time, the GC will manually cleanup the old memory.
            _memoryAllocation.Dispose();
            _memoryAllocation = new Allocation(size);
            return (void*)_memoryAllocation.Address;
        }
    }
}
