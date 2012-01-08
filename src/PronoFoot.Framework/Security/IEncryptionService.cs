﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Security
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Decodes data that has been encrypted.
        /// </summary>
        /// <param name="encodedData">The encrypted data to decrypt.</param>
        /// <returns>A Byte[] array that represents the decrypted data.</returns>
        byte[] Decode(byte[] encodedData);

        /// <summary>
        /// Encrypts data.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <returns>The encrypted value.</returns>
        byte[] Encode(byte[] data);
    }
}
