namespace NU.Core.Utils
{
    internal class Hex
    {
        private static readonly char[] HexValues = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        // Reference https://github.com/dotnet/corefx/blob/2c2e4a599889652ec579a870054b0f8915ea70fd/src/System.Security.Cryptography.Xml/src/System/Security/Cryptography/Xml/Utils.cs#L736
        internal static string EncodeHexString(byte[] sArray)
        {
            uint start = 0;
            uint end = (uint)sArray.Length;
            string result = null;
            if (sArray != null)
            {
                char[] hexOrder = new char[(end - start) * 2];
                uint digit;
                for (uint i = start, j = 0; i < end; i++)
                {
                    digit = (uint)((sArray[i] & 0xf0) >> 4);
                    hexOrder[j++] = HexValues[digit];
                    digit = (uint)(sArray[i] & 0x0f);
                    hexOrder[j++] = HexValues[digit];
                }
                result = new string(hexOrder);
            }
            return result;
        }
    }
}
