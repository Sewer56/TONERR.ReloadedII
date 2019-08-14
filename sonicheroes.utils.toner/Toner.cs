using System;
using System.Collections.Generic;
using System.Text;
using csharp_prs;
using Reloaded.Hooks.Definitions;
using Reloaded.Hooks.ReloadedII.Interfaces;
using sonicheroes.utils.toner.Heroes.One;
using sonicheroes.utils.toner.Heroes.One.Structures;
using sonicheroes.utils.toner.Heroes.One.Structures.ArchiveStructures;
using static sonicheroes.utils.toner.Heroes.One.OneFile;

namespace sonicheroes.utils.toner
{
    public unsafe class Toner : IDisposable
    {
        private Buffer _buffer = new Buffer();
        private IReloadedHooks _hooks; // Library cannot be unloaded, no need for weak reference.

        private IHook<OneFileLoadHAnimation>        _loadHAnimationHook;
        private IHook<OneFileLoadClump>             _loadClumpHook;
        private IHook<OneFileLoadTextureDictionary> _loadTextureDictionaryHook;
        private IHook<OneFileLoadSpline>            _loadSplineHook;
        private IHook<OneFileLoadDeltaMorph>        _loadDeltaMorphHook;
        private IHook<OneFileLoadWorld>             _loadWorldHook;
        private IHook<OneFileLoadUVAnim>            _loadUvAnimHook;
        private IHook<OneFileLoadMaestro>           _loadMaestroHook;
        private IHook<OneFileLoadCameraTmb>         _loadCameraTmbHook;

        /* Initialization */
        public Toner(IReloadedHooks hooks)
        {
            _hooks = hooks;
            _loadHAnimationHook         = _hooks.CreateHook<OneFileLoadHAnimation>(LoadHAnimation, 0x0042F600).Activate();
            _loadTextureDictionaryHook  = _hooks.CreateHook<OneFileLoadTextureDictionary>(LoadTextureDictionary, 0x0042F3C0).Activate();
            _loadClumpHook              = _hooks.CreateHook<OneFileLoadClump>(LoadClump, 0x0042F440).Activate();
            _loadSplineHook             = _hooks.CreateHook<OneFileLoadSpline>(LoadSpline, 0x0042F4B0).Activate();
            _loadDeltaMorphHook         = _hooks.CreateHook<OneFileLoadDeltaMorph>(LoadDeltaMorph, 0x0042F520).Activate();
            _loadWorldHook              = _hooks.CreateHook<OneFileLoadWorld>(LoadWorld, 0x0042F590).Activate();
            _loadUvAnimHook             = _hooks.CreateHook<OneFileLoadUVAnim>(LoadUvAnim, 0x0042F670).Activate();
            _loadMaestroHook            = _hooks.CreateHook<OneFileLoadMaestro>(LoadMaestro, 0x0042F6F0).Activate();
            _loadCameraTmbHook          = _hooks.CreateHook<OneFileLoadCameraTmb>(LoadCameraTmb, 0x0042F770).Activate();
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
        private int GetSizeOfFile(int fileIndex, OneFile* thisPointer)
        {
            // Sometimes the game may request a file of index 0, despite indices starting at 2.
            if (fileIndex < 2)
                return _buffer.AllocationSize;

            // Note: The pointer to the start of file in thisPointer might not be initialized, but the other pointers e.g. Start of name section.
            int fileEntry = fileIndex - 2;
            var startOfFile = ((byte*)thisPointer->NameSectionHeaderPointer) - sizeof(OneArchiveHeader);
            var oneArchive = new OneArchive(startOfFile);
            var enumerator = oneArchive.GetFileEntryEnumerator();

            // Move to correct entry.
            for (int x = 0; x <= fileEntry; x++)
                enumerator.MoveNext();

            return Prs.Estimate(enumerator.Current->GetCompressedDataPtr(), enumerator.Current->FileSize);
        }

        /* Hooks */
        private void* LoadCameraTmb(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadCameraTmbHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadMaestro(void* addressToDecompressTo, OneFile* thisPointer, int fileIndex)
        {
            return _loadMaestroHook.OriginalFunction(_buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer, fileIndex);
        }

        private void* LoadUvAnim(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadUvAnimHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadWorld(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadWorldHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadDeltaMorph(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadDeltaMorphHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadSpline(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadSplineHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadTextureDictionary(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadTextureDictionaryHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadClump(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadClumpHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }

        private void* LoadHAnimation(int fileIndex, void* addressToDecompressTo, OneFile* thisPointer)
        {
            return _loadHAnimationHook.OriginalFunction(fileIndex, _buffer.Get(GetSizeOfFile(fileIndex, thisPointer)), thisPointer);
        }
    }
}
