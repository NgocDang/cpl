using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public static class ValidateAddressHelper
    {
        public static bool IsValidETHAddress(string address)
        {
            if (IsValidAddressLength(address))
            {
                if (!Regex.IsMatch(address, @"(0x)?[0-9a-fA-f]{40}"))
                    return false;
                else if (Regex.IsMatch(address, @"(0x)?[0-9a-f]{40}") || Regex.IsMatch(address, @"(0x)?[0-9A-F]{40}"))
                    return true;
                else
                    return IsChecksumAddress(address);
            }
            else return false;
        }

        public static bool IsValidBTCAddress(string address)
        {
            char[] prefixes = new char[] { };
            prefixes = new char[] { '1', '3' };
            if (prefixes.Contains(address[0]))
            {
                byte[] hex = Base58CheckToByteArray(address);
                if (hex == null || hex.Length != 21)
                    return false;
                else
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is valid address length] [the specified address].
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>
        ///   <c>true</c> if [is valid address length] [the specified address]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValidAddressLength(string address)
        {
            // github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Util/AddressUtil.cs
            address = address.RemoveHexPrefix();
            return address.Length == 40;
        }

        /// <summary>
        /// Determines whether [is checksum address] [the specified address].
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>
        ///   <c>true</c> if [is checksum address] [the specified address]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsChecksumAddress(string address)
        {
            // github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Util/AddressUtil.cs
            address = address.RemoveHexPrefix();
            var addressHash = new Sha3Keccack().CalculateHash(address.ToLower());
            for (var i = 0; i < 40; i++)
            {
                var value = int.Parse(addressHash[i].ToString(), NumberStyles.HexNumber);
                if (value > 7 && address[i].ToString().ToUpper() != address[i].ToString() ||
                    value <= 7 && address[i].ToString().ToLower() != address[i].ToString())
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Base58s the check to byte array.
        /// </summary>
        /// <param name="base58">The base58.</param>
        /// <returns></returns>
        private static byte[] Base58CheckToByteArray(string base58)
        {
            bool IgnoreChecksum = false;
            if (base58.EndsWith("?"))
            {
                IgnoreChecksum = true;
                base58 = base58.Substring(0, base58.Length - 1);
            }

            byte[] bb = ToByteArray(base58);
            if (bb == null || bb.Length < 4) return null;

            if (IgnoreChecksum == false)
            {
                Sha256Digest bcsha256a = new Sha256Digest();
                bcsha256a.BlockUpdate(bb, 0, bb.Length - 4);

                byte[] checksum = new byte[32];  //sha256.ComputeHash(bb, 0, bb.Length - 4);
                bcsha256a.DoFinal(checksum, 0);
                bcsha256a.BlockUpdate(checksum, 0, 32);
                bcsha256a.DoFinal(checksum, 0);

                for (int i = 0; i < 4; i++)
                {
                    if (checksum[i] != bb[bb.Length - 4 + i]) return null;
                }
            }
            byte[] rv = new byte[bb.Length - 4];
            Array.Copy(bb, 0, rv, 0, bb.Length - 4);
            return rv;
        }

        /// <summary>
        /// To the byte array.
        /// </summary>
        /// <param name="base58">The base58.</param>
        /// <returns></returns>
        private static byte[] ToByteArray(string base58)
        {
            Org.BouncyCastle.Math.BigInteger bi2 = new Org.BouncyCastle.Math.BigInteger("0");
            string b58 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

            foreach (char c in base58)
            {
                if (b58.IndexOf(c) != -1)
                {
                    bi2 = bi2.Multiply(new Org.BouncyCastle.Math.BigInteger("58"));
                    bi2 = bi2.Add(new Org.BouncyCastle.Math.BigInteger(b58.IndexOf(c).ToString()));
                }
                else
                {
                    return null;
                }
            }

            byte[] bb = bi2.ToByteArrayUnsigned();

            // interpret leading '1's as leading zero bytes
            foreach (char c in base58)
            {
                if (c != '1') break;
                byte[] bbb = new byte[bb.Length + 1];
                Array.Copy(bb, 0, bbb, 1, bb.Length);
                bb = bbb;
            }

            return bb;
        }

    }
}
