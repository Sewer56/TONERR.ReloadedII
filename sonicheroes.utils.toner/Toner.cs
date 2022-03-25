using System;
using Heroes.SDK;
using Heroes.SDK.Classes.NativeClasses;
using Heroes.SDK.Definitions.Structures.Archive.OneFile;
using Heroes.SDK.Parsers;
using Reloaded.Hooks.Definitions;
using static Heroes.SDK.Classes.NativeClasses.OneFile;

namespace sonicheroes.utils.toner;

public unsafe class Toner : IDisposable
{
    private Buffer _buffer = new Buffer();

    private IHook<Native_OneFileLoadHAnimation>        _loadHAnimationHook;
    private IHook<Native_OneFileLoadClump>             _loadClumpHook;
    private IHook<Native_OneFileLoadTextureDictionary> _loadTextureDictionaryHook;
    private IHook<Native_OneFileLoadSpline>            _loadSplineHook;
    private IHook<Native_OneFileLoadDeltaMorph>        _loadDeltaMorphHook;
    private IHook<Native_OneFileLoadWorld>             _loadWorldHook;
    private IHook<Native_OneFileLoadUVAnim>            _loadUvAnimHook;
    private IHook<Native_OneFileLoadMaestro>           _loadMaestroHook;
    private IHook<Native_OneFileLoadCameraTmb>         _loadCameraTmbHook;

    /* Initialization */
    public Toner()
    {
        _loadHAnimationHook         = Fun_LoadHAnimation.Hook(LoadHAnimation).Activate();
        _loadTextureDictionaryHook  = Fun_LoadTextureDictionary.Hook(LoadTextureDictionary).Activate();
        _loadClumpHook              = Fun_LoadClump.Hook(LoadClump).Activate();
        _loadSplineHook             = Fun_LoadSpline.Hook(LoadSpline).Activate();
        _loadDeltaMorphHook         = Fun_LoadDeltaMorph.Hook(LoadDeltaMorph).Activate();
        _loadWorldHook              = Fun_LoadWorld.Hook(LoadWorld).Activate();
        _loadUvAnimHook             = Fun_LoadUVAnim.Hook(LoadUvAnim).Activate();
        _loadMaestroHook            = Fun_LoadMaestro.Hook(LoadMaestro).Activate();
        _loadCameraTmbHook          = Fun_LoadCameraTmb.Hook(LoadCameraTmb).Activate();
    }

    ~Toner()
    {
        Dispose();
    }

    public void Dispose()
    {
        Disable();
        _buffer.Dispose();
        GC.SuppressFinalize(this);
    }

    /* Loader API */
    public void Disable()
    {
        _loadHAnimationHook.Disable();
        _loadClumpHook.Disable();
        _loadTextureDictionaryHook.Disable();
        _loadSplineHook.Disable();
        _loadDeltaMorphHook.Disable();
        _loadWorldHook.Disable();
        _loadUvAnimHook.Disable();
        _loadMaestroHook.Disable();
        _loadCameraTmbHook.Disable();
    }

    public void Enable()
    {
        _loadHAnimationHook.Enable();
        _loadClumpHook.Enable();
        _loadTextureDictionaryHook.Enable();
        _loadSplineHook.Enable();
        _loadDeltaMorphHook.Enable();
        _loadWorldHook.Enable();
        _loadUvAnimHook.Enable();
        _loadMaestroHook.Enable();
        _loadCameraTmbHook.Enable();
    }

    /* Implementation */
    private int GetSizeOfFile(int fileIndex, ref OneFile thisPointer)
    {
        // Sometimes the game may request a file of index 0, despite indices starting at 2.
        if (fileIndex < 2)
            return _buffer.AllocationSize;

        // Note: The pointer to the start of file in thisPointer might not be initialized, but the other pointers e.g. Start of name section.
        int fileEntry = fileIndex - 2;
        var startOfFile = ((byte*)thisPointer.NameSectionHeaderPointer) - sizeof(OneArchiveHeader);
        var oneArchive = new OneArchive(startOfFile);
        var enumerator = oneArchive.GetFileEntryEnumerator();

        // Move to correct entry.
        for (int x = 0; x <= fileEntry; x++)
            enumerator.MoveNext();

        return SDK.Prs.Estimate(enumerator.Current->GetCompressedDataPtr(), enumerator.Current->FileSize);
    }

    /* Hooks */
    private void* LoadCameraTmb(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadCameraTmbHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadMaestro(void* addressToDecompressTo, ref OneFile thisPointer, int fileIndex)
    {
        bool shouldFree = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadMaestroHook.OriginalFunction(newAddress, ref thisPointer, fileIndex);
        if (shouldFree)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadUvAnim(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadUvAnimHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadWorld(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadWorldHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadDeltaMorph(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadDeltaMorphHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadSpline(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadSplineHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadTextureDictionary(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadTextureDictionaryHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadClump(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadClumpHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }

    private void* LoadHAnimation(int fileIndex, void* addressToDecompressTo, ref OneFile thisPointer)
    {
        bool isLargeObject = _buffer.Get(GetSizeOfFile(fileIndex, ref thisPointer), out var newAddress);
        var returnValue = _loadHAnimationHook.OriginalFunction(fileIndex, newAddress, ref thisPointer);
        if (isLargeObject)
            _buffer.FreeLargeObject();

        return returnValue;
    }
}