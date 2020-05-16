using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;

namespace ExtenFlow.Infrastructure
{
    /// <summary>
    /// Class GuidHelper.
    /// </summary>
    public static class GuidHelper
    {
        /// <summary>
        /// Encodes the Guid in a base64 string. The trailing '==' have been removed and '+/'
        /// characters have been replaced by '-_' to avoid issues when using this string in URLs.
        /// </summary>
        /// <param name="uniqueIdentifier">The unique identifier.</param>
        /// <returns>The encoded string.</returns>
        public static string ToBase64String(this Guid uniqueIdentifier)
        {
            if (uniqueIdentifier == Guid.Empty)
            {
                return string.Empty;
            }
            Span<byte> guidBytes = stackalloc byte[16];
            Span<byte> encodedBytes = stackalloc byte[24];

            if (!MemoryMarshal.TryWrite(guidBytes, ref uniqueIdentifier)) // write bytes from the Guid
            {
                throw new Exception(Properties.Resources.MemoryWriteError);
            }
            Base64.EncodeToUtf8(guidBytes, encodedBytes, out _, out _);

            Span<char> chars = stackalloc char[22];

            const byte slash = (byte)'/';
            const byte plus = (byte)'+';
            for (int i = 0; i < 22; i++)
            {
                chars[i] = (encodedBytes[i]) switch
                {
                    slash => '_',
                    plus => '-',
                    _ => (char)encodedBytes[i],
                };
            }

            return new string(chars);
        }
    }
}