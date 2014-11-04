// Copyright © 2004-2014 Tim Van Holder, Windower Team
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

// Define this to include MDX 2.0 in the list of assemblies to (try to) load.  Currently disabled because it is not backwards compatible.
//#define USE_MDX_2

using System;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ManagedDirectX
{

    #region DirectSound

    #region Top-Level Helper Class

    public class ManagedDirectSound
    {
        private ManagedDirectSound() { }

        private static bool Initialized = false;
        private static Assembly Assembly = null;
        private static byte AssemblyVersion = 0;

        private static void Load()
        {
            if (!ManagedDirectSound.Initialized)
            {
                ManagedDirectSound.AssemblyVersion = 0;
#if USE_MDX_2
                if (ManagedDirectSound.Assembly == null)
                {
                    try
                    {
                        // 2.0 (beta)
                        ManagedDirectSound.Assembly =
                            Assembly.Load("Microsoft.DirectX, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                        ManagedDirectSound.AssemblyVersion = 2;
                    }
                    catch {}
                }
#endif
                if (ManagedDirectSound.Assembly == null)
                {
                    try
                    {
                        // 1.1
                        ManagedDirectSound.Assembly =
                            Assembly.Load(
                                "Microsoft.DirectX.DirectSound, Version=1.0.2902.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                        ManagedDirectSound.AssemblyVersion = 1;
                    }
                    catch {}
                }
                ManagedDirectSound.Initialized = true;
            }
        }

        public static bool Available
        {
            get
            {
                ManagedDirectSound.Load();
                return (ManagedDirectSound.Assembly != null);
            }
        }

        public static Type GetType(string Name)
        {
            if (ManagedDirectSound.Available)
            {
                return ManagedDirectSound.Assembly.GetType(Name, false, false);
            }
            return null;
        }

        public static object CreateObject(string Type)
        {
            if (ManagedDirectSound.Available)
            {
                return ManagedDirectSound.Assembly.CreateInstance(Type, false);
            }
            return null;
        }

        public static object CreateObject(string Type, params object[] Arguments)
        {
            if (ManagedDirectSound.Available)
            {
                return ManagedDirectSound.Assembly.CreateInstance(Type, false, BindingFlags.CreateInstance, null, Arguments,
                    Application.CurrentCulture, null);
            }
            return null;
        }

        public static object CreateObject(Type Type)
        {
            if (ManagedDirectSound.Available)
            {
                return ManagedDirectSound.Assembly.CreateInstance(Type.FullName, false);
            }
            return null;
        }

        public static object CreateObject(Type Type, params object[] Arguments)
        {
            if (Arguments == null)
            {
                Arguments = new object[] { };
            }
            if (ManagedDirectSound.Available)
            {
                return ManagedDirectSound.Assembly.CreateInstance(Type.FullName, false, BindingFlags.CreateInstance, null, Arguments,
                    Application.CurrentCulture, null);
            }
            return null;
        }

        public static byte Version
        {
            get { return ManagedDirectSound.AssemblyVersion; }
        }
    }

    #endregion

    namespace DirectSound
    {

        #region Enums

        public class BufferPlayFlags
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferPlayFlags");

            private BufferPlayFlags() { }

            public static object Default
            {
                get
                {
                    try
                    {
                        return TMe.GetField("Default").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }

            public static object Looping
            {
                get
                {
                    try
                    {
                        return TMe.GetField("Looping").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }
        }

        public class CooperativeLevel
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.CooperativeLevel");

            private CooperativeLevel() { }

            public static object Normal
            {
                get
                {
                    try
                    {
                        return TMe.GetField("Normal").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }
        }

        public class LockFlag
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.LockFlag");

            private LockFlag() { }

            public static object None
            {
                get
                {
                    try
                    {
                        return TMe.GetField("None").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }

            public static object EntireBuffer
            {
                get
                {
                    try
                    {
                        return TMe.GetField("EntireBuffer").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }

            public static object FromWriteCursor
            {
                get
                {
                    try
                    {
                        return TMe.GetField("FromWriteCursor").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }
        }

        public class WaveFormatTag
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.WaveFormatTag");

            private WaveFormatTag() { }

            public static object Pcm
            {
                get
                {
                    try
                    {
                        return TMe.GetField("Pcm").GetValue(null);
                    }
                    catch {}
                    return null;
                }
            }
        }

        #endregion

        #region Static Classes

        public class DSoundHelper
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.DSoundHelper");

            private DSoundHelper() { }

            public static Guid DefaultPlaybackDevice
            {
                get
                {
                    try
                    {
                        return (Guid)TMe.GetField("DefaultPlaybackDevice").GetValue(null);
                    }
                    catch {}
                    return Guid.Empty;
                }
            }
        }

        #endregion

        #region Normal Classes

        public class Device
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.Device");
            internal object Me;

            public Device() { Me = ManagedDirectSound.CreateObject(TMe); }

            public void SetCooperativeLevel(Control Owner, object CooperativeLevel)
            {
                TMe.InvokeMember("SetCooperativeLevel", BindingFlags.InvokeMethod, null, Me,
                    new object[] { Owner.Handle, CooperativeLevel }, Application.CurrentCulture);
            }
        }

        public class BufferDescription
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferDescription");
            internal object Me;

            public BufferDescription(WaveFormat Format)
            {
                if (ManagedDirectSound.Version == 2)
                {
                    // Managed DirectX 2.x only has a 0-argument constructor, and a WaveFormat property
                    Me = ManagedDirectSound.CreateObject(TMe);
                    TMe.GetProperty("WaveFormat").SetValue(Me, Format.Me, null);
                }
                else
                {
                    // Managed DirectX 1.x has a constructor that takes a WaveFormat, and no way to set it afterwards
                    Me = ManagedDirectSound.CreateObject(TMe, Format.Me);
                }
            }

            public int BufferBytes
            {
                get
                {
                    try
                    {
                        return (int)TMe.GetProperty("BufferBytes").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("BufferBytes").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public bool GlobalFocus
            {
                get
                {
                    try
                    {
                        return (bool)TMe.GetProperty("GlobalFocus").GetValue(Me, null);
                    }
                    catch {}
                    return false;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("GlobalFocus").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public bool StickyFocus
            {
                get
                {
                    try
                    {
                        return (bool)TMe.GetProperty("StickyFocus").GetValue(Me, null);
                    }
                    catch {}
                    return false;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("StickyFocus").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public bool ControlPositionNotify
            {
                get
                {
                    try
                    {
                        return (bool)TMe.GetProperty("ControlPositionNotify").GetValue(Me, null);
                    }
                    catch {}
                    return false;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("ControlPositionNotify").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }
        }

        public class BufferCaps
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferCaps");
            internal object Me;

            public BufferCaps(object BufferCaps) { Me = BufferCaps; }

            public int BufferBytes
            {
                get
                {
                    try
                    {
                        return (int)TMe.GetProperty("BufferBytes").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("BufferBytes").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }
        }

        public class WaveFormat
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.WaveFormat");
            internal object Me;

            public WaveFormat() { Me = ManagedDirectSound.CreateObject(TMe); }

            public object FormatTag
            {
                get
                {
                    try
                    {
                        return TMe.GetProperty("FormatTag").GetValue(Me, null);
                    }
                    catch {}
                    return null;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("FormatTag").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public short Channels
            {
                get
                {
                    try
                    {
                        return (short)TMe.GetProperty("Channels").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("Channels").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public int SamplesPerSecond
            {
                get
                {
                    try
                    {
                        return (int)TMe.GetProperty("SamplesPerSecond").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("SamplesPerSecond").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public short BitsPerSample
            {
                get
                {
                    try
                    {
                        return (short)TMe.GetProperty("BitsPerSample").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("BitsPerSample").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public short BlockAlign
            {
                get
                {
                    try
                    {
                        return (short)TMe.GetProperty("BlockAlign").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("BlockAlign").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public int AverageBytesPerSecond
            {
                get
                {
                    try
                    {
                        return (int)TMe.GetProperty("AverageBytesPerSecond").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("AverageBytesPerSecond").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }
        }

        public class BufferStatus
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferStatus");
            internal object Me;

            internal BufferStatus(object BufferStatus) { Me = BufferStatus; }

            public bool Playing
            {
                get
                {
                    try
                    {
                        return (bool)TMe.GetProperty("Playing").GetValue(Me, null);
                    }
                    catch {}
                    return false;
                }
            }
        }

        public class BufferPositionNotify
        {
            internal static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferPositionNotify");
            internal object Me;

            internal BufferPositionNotify() { Me = ManagedDirectSound.CreateObject(TMe); }

            public int Offset
            {
                get
                {
                    try
                    {
                        return (int)TMe.GetProperty("Offset").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("Offset").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }

            public IntPtr EventNotifyHandle
            {
                get
                {
                    try
                    {
                        return (IntPtr)TMe.GetProperty("EventNotifyHandle").GetValue(Me, null);
                    }
                    catch {}
                    return IntPtr.Zero;
                }
                set
                {
                    try
                    {
                        TMe.GetProperty("EventNotifyHandle").SetValue(Me, value, null);
                    }
                    catch {}
                }
            }
        }

        public class Notify : IDisposable
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.Notify");
            internal object Me;

            public Notify(SecondaryBuffer Buffer) { Me = ManagedDirectSound.CreateObject(TMe, Buffer.Me); }

            public void SetNotificationPositions(BufferPositionNotify[] Positions)
            {
                Array RealPositions = Array.CreateInstance(BufferPositionNotify.TMe, Positions.Length);
                for (int i = 0; i < Positions.Length; ++i)
                {
                    RealPositions.SetValue(Positions[i].Me, i);
                }
                TMe.InvokeMember("SetNotificationPositions", BindingFlags.InvokeMethod, null, Me, new object[] { RealPositions },
                    Application.CurrentCulture);
            }

            public void Dispose()
            {
                if (Me != null && Me is IDisposable)
                {
                    (Me as IDisposable).Dispose();
                }
            }
        }

        public class SecondaryBuffer : IDisposable
        {
            private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.SecondaryBuffer");
            internal object Me;

            public SecondaryBuffer(Stream Stream, Device ParentDevice)
            {
                Me = ManagedDirectSound.CreateObject(TMe, Stream, ParentDevice.Me);
            }

            public SecondaryBuffer(Stream Stream, BufferDescription BufferDescription, Device ParentDevice)
            {
                Me = ManagedDirectSound.CreateObject(TMe, Stream, BufferDescription.Me, ParentDevice.Me);
            }

            public SecondaryBuffer(BufferDescription BufferDescription, Device ParentDevice)
            {
                Me = ManagedDirectSound.CreateObject(TMe, BufferDescription.Me, ParentDevice.Me);
            }

            public BufferCaps Caps
            {
                get
                {
                    try
                    {
                        return new BufferCaps(TMe.GetProperty("Caps").GetValue(Me, null));
                    }
                    catch {}
                    return null;
                }
            }

            public BufferStatus Status
            {
                get
                {
                    try
                    {
                        return new BufferStatus(TMe.GetProperty("Status").GetValue(Me, null));
                    }
                    catch {}
                    return null;
                }
            }

            public int PlayPosition
            {
                get
                {
                    try
                    {
                        return (int)TMe.GetProperty("PlayPosition").GetValue(Me, null);
                    }
                    catch {}
                    return 0;
                }
            }

            public void Play(int Priority, object BufferPlayFlags)
            {
                TMe.InvokeMember("Play", BindingFlags.InvokeMethod, null, Me, new object[] { Priority, BufferPlayFlags },
                    Application.CurrentCulture);
            }

            public void Stop() { TMe.InvokeMember("Stop", BindingFlags.InvokeMethod, null, Me, null, Application.CurrentCulture); }

            public void Write(int BufferStartingLocation, Stream Data, int NumberOfBytesToWrite, object LockFlag)
            {
                TMe.InvokeMember("Write", BindingFlags.InvokeMethod, null, Me,
                    new object[] { BufferStartingLocation, Data, NumberOfBytesToWrite, LockFlag }, Application.CurrentCulture);
            }

            public void Dispose()
            {
                if (Me != null && Me is IDisposable)
                {
                    (Me as IDisposable).Dispose();
                }
            }
        }

        #endregion
    }

    #endregion
}
