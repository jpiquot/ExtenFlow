using System;
using System.Buffers;
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
                    _ => (char)encodedBytes[i]
                };
            }

            return new string(chars);
        }

        /// <summary>
        /// Converts to guid.
        /// </summary>
        /// <param name="guidString">The unique identifier string.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        /// <exception cref="System.ArgumentException">guidString</exception>
        public static Guid ToGuid(this string guidString)
            => ToGuidOrDefault(guidString) ?? throw new ArgumentException(Properties.Resources.StringInvalidGuid, nameof(guidString));

        /// <summary>
        /// Converts to guidordefault.
        /// </summary>
        /// <param name="guidString">The unique identifier string.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public static Guid? ToGuidOrDefault(this string guidString)
        {
            if (string.IsNullOrWhiteSpace(guidString))
            {
                return null;
            }
            if (guidString.Length == 22 || (guidString.Length == 24 && guidString[22] == '=' && guidString[23] == '=')) // base 64 string
            {
                Span<byte> base64Bytes = stackalloc byte[24];
                const byte slash = (byte)'/';
                const byte plus = (byte)'+';
                for (int i = 0; i < 22; i++)
                {
                    base64Bytes[i] = (guidString[i]) switch
                    {
                        '_' => slash,
                        '-' => plus,
                        _ => (byte)guidString[i]
                    };
                }
                base64Bytes[22] = base64Bytes[23] = (byte)'=';
                Span<byte> guidBytes = stackalloc byte[16];
                OperationStatus status = Base64.DecodeFromUtf8(base64Bytes, guidBytes, out _, out _);
                if (status == OperationStatus.Done)
                {
                    return new Guid(guidBytes);
                }
                return null;
            }
            return Guid.TryParse(guidString, out Guid uniqueId) ?
                (Guid?)uniqueId :
                null;
        }
    }
}