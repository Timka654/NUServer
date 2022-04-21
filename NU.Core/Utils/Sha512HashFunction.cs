using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NU.Core.Utils
{
    public sealed class Sha512HashFunction : IDisposable
    {
        private byte[] _hash;

        private readonly SHA512 _hashFunc;

        public Sha512HashFunction()
        {
            _hashFunc = SHA512.Create();
        }

        public void Update(byte[] data, int offset, int count)
        {
            if (_hash != null)
            {
                throw new InvalidOperationException();
            }

            _hashFunc.TransformBlock(data, offset, count, outputBuffer: null, outputOffset: 0);
        }

        public byte[] GetHashBytes()
        {
            if (_hash == null)
            {
                _hashFunc.TransformFinalBlock(new byte[0], inputOffset: 0, inputCount: 0);

                _hash = _hashFunc.Hash;
            }

            return _hash;
        }

        public string GetHash()
        {
            return Convert.ToBase64String(GetHashBytes());
        }

        public void Dispose()
        {
            _hashFunc.Dispose();
        }
    }
}
