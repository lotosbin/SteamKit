/*
 * This file is subject to the terms and conditions defined in
 * file 'license.txt', which is part of this source code package.
 */



using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SteamKit2
{
    static class Utils
    {
        public static string EncodeHexString(byte[] input)
        {
            return input.Aggregate(new StringBuilder(),
                       (sb, v) => sb.Append(v.ToString("x2"))
                      ).ToString();
        }

        public static byte[] DecodeHexString(string hex)
        {
            if (hex == null)
                return null;

            int chars = hex.Length;
            byte[] bytes = new byte[chars / 2];

            for (int i = 0; i < chars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }

        public static EOSType GetOSType()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return EOSType.WinUnknown;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return EOSType.MacOSUnknown;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return EOSType.LinuxUnknown;
            }
            else
            {
                return EOSType.Unknown;
            }
        }

        public static bool IsRunningOnDarwin()
        {
            // Replace with a safer way if one exists in the future, such as if
            // Mono actually decides to use PlatformID.MacOSX
            var buffer = IntPtr.Zero;
            try
            {
                buffer = Marshal.AllocHGlobal( 8192 );
                if ( uname( buffer ) == 0 )
                {
                    var kernelName = Marshal.PtrToStringAnsi( buffer );
                    if ( kernelName == "Darwin" )
                    {
                        return true;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if ( buffer != IntPtr.Zero )
                    Marshal.FreeHGlobal( buffer );
            }

            return false;
        }

        [DllImport ("libc")]
        static extern int uname (IntPtr buf);

        public static T[] GetAttributes<T>( this Type type, bool inherit = false )
            where T : Attribute
        {
            return type.GetTypeInfo().GetCustomAttributes( typeof( T ), inherit ) as T[];
        }
    }

    /// <summary>
    /// Contains various utility functions for dealing with dates.
    /// </summary>
    public static class DateUtils
    {
        /// <summary>
        /// Converts a given unix timestamp to a DateTime
        /// </summary>
        /// <param name="unixTime">A unix timestamp expressed as seconds since the unix epoch</param>
        /// <returns>DateTime representation</returns>
        public static DateTime DateTimeFromUnixTime(ulong unixTime)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(unixTime);
        }
        /// <summary>
        /// Converts a given DateTime into a unix timestamp representing seconds since the unix epoch.
        /// </summary>
        /// <param name="time">DateTime to be expressed</param>
        /// <returns>64-bit wide representation</returns>
        public static ulong DateTimeToUnixTime(DateTime time)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return (ulong)(time - origin).TotalSeconds;
        }
    }

    /// <summary>
    /// Contains various utility functions for handling EMsgs.
    /// </summary>
    public static class MsgUtil
    {
        private const uint ProtoMask = 0x80000000;
        private const uint EMsgMask = ~ProtoMask;

        /// <summary>
        /// Strips off the protobuf message flag and returns an EMsg.
        /// </summary>
        /// <param name="msg">The message number.</param>
        /// <returns>The underlying EMsg.</returns>
        public static EMsg GetMsg( uint msg )
        {
            return ( EMsg )( msg & EMsgMask );
        }

        /// <summary>
        /// Strips off the protobuf message flag and returns an EMsg.
        /// </summary>
        /// <param name="msg">The message number.</param>
        /// <returns>The underlying EMsg.</returns>
        public static uint GetGCMsg( uint msg )
        {
            return ( msg & EMsgMask );
        }

        /// <summary>
        /// Strips off the protobuf message flag and returns an EMsg.
        /// </summary>
        /// <param name="msg">The message number.</param>
        /// <returns>The underlying EMsg.</returns>
        public static EMsg GetMsg( EMsg msg )
        {
            return GetMsg( ( uint )msg );
        }

        /// <summary>
        /// Determines whether message is protobuf flagged.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>
        ///   <c>true</c> if this message is protobuf flagged; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsProtoBuf( uint msg )
        {
            return ( msg & ProtoMask ) > 0;
        }

        /// <summary>
        /// Determines whether message is protobuf flagged.
        /// </summary>
        /// <param name="msg">The message.</param>
        /// <returns>
        ///   <c>true</c> if this message is protobuf flagged; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsProtoBuf( EMsg msg )
        {
            return IsProtoBuf( ( uint )msg );
        }

        /// <summary>
        /// Crafts an EMsg, flagging it if required.
        /// </summary>
        /// <param name="msg">The EMsg to flag.</param>
        /// <param name="protobuf">if set to <c>true</c>, the message is protobuf flagged.</param>
        /// <returns>A crafted EMsg, flagged if requested.</returns>
        public static EMsg MakeMsg( EMsg msg, bool protobuf = false )
        {
            if ( protobuf )
                return ( EMsg )( ( uint )msg | ProtoMask );

            return msg;
        }
        /// <summary>
        /// Crafts an EMsg, flagging it if required.
        /// </summary>
        /// <param name="msg">The EMsg to flag.</param>
        /// <param name="protobuf">if set to <c>true</c>, the message is protobuf flagged.</param>
        /// <returns>A crafted EMsg, flagged if requested.</returns>
        public static uint MakeGCMsg( uint msg, bool protobuf = false )
        {
            if ( protobuf )
                return msg | ProtoMask;

            return msg;
        }
    }

    static class WebHelpers
    {
        static bool IsUrlSafeChar( char ch )
        {
            if ( ( ( ( ch >= 'a' ) && ( ch <= 'z' ) ) || ( ( ch >= 'A' ) && ( ch <= 'Z' ) ) ) || ( ( ch >= '0' ) && ( ch <= '9' ) ) )
            {
                return true;
            }

            switch ( ch )
            {
                case '-':
                case '.':
                case '_':
                    return true;
            }

            return false;
        }

        public static string UrlEncode( string input )
        {
            return UrlEncode( Encoding.UTF8.GetBytes( input ) );
        }


        public static string UrlEncode( byte[] input )
        {
            StringBuilder encoded = new StringBuilder( input.Length * 2 );

            for ( int i = 0 ; i < input.Length ; i++ )
            {
                char inch = ( char )input[ i ];

                if ( IsUrlSafeChar( inch ) )
                {
                    encoded.Append( inch );
                }
                else if ( inch == ' ' )
                {
                    encoded.Append( '+' );
                }
                else
                {
                    encoded.AppendFormat( "%{0:X2}", input[ i ] );
                }
            }

            return encoded.ToString();
        }
    }

    static class NetHelpers
    {
        public static IPAddress GetLocalIP(Socket activeSocket)
        {
            IPEndPoint ipEndPoint = activeSocket.LocalEndPoint as IPEndPoint;

            if ( ipEndPoint == null || ipEndPoint.Address == IPAddress.Any )
                throw new Exception( "Socket not connected" );

            return ipEndPoint.Address;
        }

        public static IPAddress GetIPAddress( uint ipAddr )
        {
            byte[] addrBytes = BitConverter.GetBytes( ipAddr );
            Array.Reverse( addrBytes );

            return new IPAddress( addrBytes );
        }
        public static uint GetIPAddress( IPAddress ipAddr )
        {
            byte[] addrBytes = ipAddr.GetAddressBytes();
            Array.Reverse( addrBytes );

            return BitConverter.ToUInt32( addrBytes, 0 );
        }


        public static uint EndianSwap( uint input )
        {
            return ( uint )IPAddress.NetworkToHostOrder( ( int )input );
        }
        public static ulong EndianSwap( ulong input )
        {
            return ( ulong )IPAddress.NetworkToHostOrder( ( long )input );
        }
        public static ushort EndianSwap( ushort input )
        {
            return ( ushort )IPAddress.NetworkToHostOrder( ( short )input );
        }
        public static bool TryParseIPEndPoint(string stringValue, out IPEndPoint endPoint)
        {
            var endpointParts = stringValue.Split(':');
            if (endpointParts.Length != 2)
            {
                endPoint = null;
                return false;
            }

            IPAddress address;
            if (!IPAddress.TryParse(endpointParts[0], out address))
            {
                endPoint = null;
                return false;
            }

            int port;
            if (!int.TryParse(endpointParts[1], out port))
            {
                endPoint = null;
                return false;
            }

            endPoint = new IPEndPoint(address, port);
            return true;
        }
    }
}
