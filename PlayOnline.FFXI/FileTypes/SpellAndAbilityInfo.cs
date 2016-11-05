// Copyright © 2010-2012 Chris Baggett, Tim Van Holder, Nevin Stepan
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Runtime.InteropServices;
using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes
{
    public class SpellAndAbilityInfo : FileType
    {
        private enum BlockType
        {
            ContainerEnd = 0x00,
            ContainerBegin = 0x01,
            SpellData = 0x49,
            AbilityData = 0x53,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Header
        {
            private int id;
            private int size;
            private long padding;

            public int ID => id;
            public int Size => (size >> 3 & ~0xF) - 0x10;
            public BlockType Type => (BlockType) (size & 0x7F);
        }

        public static T Read<T>(Stream stream)
        {
            var size = Marshal.SizeOf(typeof(T));
            var data = new byte[size];
            stream.Read(data, 0, data.Length);

            var ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.AllocHGlobal(data.Length);
                Marshal.Copy(data, 0, ptr, data.Length);
                return (T)Marshal.PtrToStructure(ptr, typeof(T));
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }

        public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback)
        {
            var TL = new ThingList();
            ProgressCallback?.Invoke(I18N.GetText("FTM:CheckingFile"), 0);

            var stream = BR.BaseStream;

            var header = Read<Header>(stream);
            var block = stream.Position;

            if (header.Type != BlockType.ContainerBegin)
            {
                throw new InvalidDataException();
            }

            while (header.Type != BlockType.ContainerEnd)
            {
                stream.Position = block + header.Size;

                header = Read<Header>(stream);
                block = stream.Position;

                switch (header.Type)
                {
                case BlockType.SpellData:
                    while (stream.Position < block + header.Size)
                    {
                        var SI2 = new Things.SpellInfo2();
                        if (!SI2.Read(BR))
                        {
                            TL.Clear();
                            return TL;
                        }
                        TL.Add(SI2);
                    }
                    break;

                case BlockType.AbilityData:
                    while (stream.Position < block + header.Size)
                    {
                        var AI2 = new Things.AbilityInfo2();
                        if (!AI2.Read(BR))
                        {
                            TL.Clear();
                            return TL;
                        }
                        TL.Add(AI2);
                    }
                    break;

                }

                ProgressCallback(I18N.GetText("FTM:LoadingData"), (double)BR.BaseStream.Position / BR.BaseStream.Length);
            }

            return TL;
        }
    }
}
