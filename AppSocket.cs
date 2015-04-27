using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSocket
{
    class Program
    {
        static Dictionary<string, string> table1 = new Dictionary<string, string>() 
        {
            {"0000","11110"},
            {"0001","01001"},
            {"0010","10100"},
            {"0011","10101"},
            {"0100","01010"},
            {"0101","01011"},
            {"0110","01110"},
            {"0111","01111"},
            {"1000","10010"},
            {"1001","10011"},
            {"1010","10110"},
            {"1011","10111"},
            {"1100","11010"},
            {"1101","11011"},
            {"1110","11100"},
            {"1111","11101"}
        };

        static void Main(string[] args)
        {

            ConectScytl();
        }

        static void ConectScytl()
        {
            byte[] bytes = new byte[1024];

            try
            {
                Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect("X.X.X.X", XXXXX);

                    int bytesRec = sender.Receive(bytes);

                    string messageDecoded = DecodeMessage(bytes);

                    string messageEncoded = EncodeMessage(messageDecoded);

                    byte[] msg = StringToByteArray(messageEncoded.Replace(" ",""));

                    int bytesSent = sender.Send(msg);

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
                finally
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static string DecodeMessage(byte[] bytes)
        {
            string hexString = ByteArrayToHexString(bytes);

            hexString = hexString.Replace(" 6B C6 ", ";").Replace("C6", "").Replace("21", "").Trim();

            //Step 1 (divide the received bytes into packets using the StartPacket, EndPacket and EndTransmission packets):
            //0xC6 0x57 0x54 0x95 0x5E 0x9E 0x6B 0xC6 0x55 0x17 0x55 0x52 0x9E 0x21 
            //0x57 0x54 0x95 0x5E 0x9E
            //0x55 0x17 0x55 0x52 0x9E            
            string[] hexStringArray = hexString.Split(';');

            string[] binaryStringXBlocks = new string[hexStringArray.Length];

            //Step 2 (for each packet, take 5 bytes starting at the second byte and transform into 
            //4 bytes using Table 1’s inverse):

            JoinAndSplit(hexStringArray, 5, ref binaryStringXBlocks);

            //5-bit blocks transformed into 4-bit blocks using Table 1’s inverse 
            //0100 1111 0100 0001 0100 1011 0010 0000
            //0100 0010 0101 0011 0100 0010 0010 0000

            short counter = 0;
            foreach (string row in binaryStringXBlocks)
            {
                string[] arrayValues = row.Split(' ');

                short count = 0;
                foreach (var item in arrayValues)
                {
                    arrayValues[count] = GetTable1KeyFromValue(item);
                    count++;
                }

                //Bits regrouped into 8-bit blocks
                //01001111 01000001 01001011 00100000
                //01000010 01010011 01000010 00100000

                string result = String.Join("", arrayValues);

                binaryStringXBlocks[counter] = SplitPart(result, 8);
                counter++;
            }

            counter = 0;

            //8-bit blocks as bytes
            //0x4F 0x41 0x4B 0x20
            //0x42 0x53 0x42 0x20

            foreach (string row in binaryStringXBlocks)
            {
                string[] arrayValues = row.Split(' ');

                short count = 0;
                foreach (var item in arrayValues)
                {
                    arrayValues[count] = BinaryStringToHexString(item);
                    count++;
                }

                binaryStringXBlocks[counter] = String.Join(" ", arrayValues);
                counter++;
            }

            //Step 3 (concatenate the 4 bytes obtained from each packet into a single byte array): 
            //0x4F 0x41 0x4B 0x20 0x42 0x53 0x42 0x20 
            string resultConcatenated = String.Join(" ", binaryStringXBlocks);

            //Step 4 (decode the concatenated bytes as an ASCII string): 
            //"OAK BSB " 

            //Step 5 (remove any trailing spaces (0x20) at the end of the string): 

            byte[] data = FromHex(resultConcatenated);
            string finalResult = Encoding.ASCII.GetString(data).Trim();

            //"OAK BSB" (decoding result)

            return finalResult;
        }

        static string EncodeMessage(string message)
        {
            /*
                To encode a message:
                1. If the message length is not divisible by 4, pad to the right with spaces (0x20) as needed
                2. Encode the padded message in ASCII
                3. Take every 4 bytes of the ASCII-encoded message and transform into 5 bytes using Table 1
                4. Add the StartPacket byte at the packet’s beginning
                5. Add the EndPacket byte at the packet’s end if there are more bytes to transmit or the
                EndTransmission packet otherwise
            */
            
            string reverse = Reverse(message);


            int arrayLength = 0;
            int i = reverse.Length;

            int value = DivisibleBy4(i);

            if (value > i)
            { 
                reverse = reverse + new String(' ', (value - i));
                arrayLength = (value - i) / 4;
            }
            else
            {
                arrayLength = i / 4;
            }

            string newMessage = StringToHexString(reverse);

            // 3. Take every 4 bytes of the ASCII-encoded message and transform into 5 bytes using Table 1
            //0x42 0x53 0x42 0x20
            //0x4B 0x41 0x4F 0x20

            string[] hexStringArrayNEW = new string[arrayLength];
            string[] hexStringArray = newMessage.Split(' ');

            string temp = "";
            int counter = 0;
            short count2 = 0;
            foreach (var item in hexStringArray)
            {
                if (count2 == 4)
                {
                    count2 = 0;
                    hexStringArrayNEW[counter] = temp.Trim();
                    counter++;
                    temp = "";                    
                }

                temp += item.ToString() + " ";
                count2++;
            }
            hexStringArrayNEW[counter] = temp.Trim();


            //4-bit blocks
            //0100 0010 0101 0011 0100 0010 0010 0000
            //0100 1011 0100 0001 0100 1111 0010 0000
            string[] binaryStringXBlocks = new string[hexStringArrayNEW.Length];

            JoinAndSplit(hexStringArrayNEW, 4, ref binaryStringXBlocks);

            //4-bit blocks transformed into 5-bit blocks using Table 1’s conversion 
            //01010 10100 01011 10101 01010 10100 10100 11110
            //01010 10111 01010 01001 01010 11101 10100 11110

            counter = 0;
            foreach (string row in binaryStringXBlocks)
            {
                string[] arrayValues = row.Split(' ');

                short count = 0;
                foreach (var item in arrayValues)
                {
                    arrayValues[count] = GetTable1ValueFromKey(item);
                    count++;
                }

                //Bits regrouped into 8-bit blocks
                //01010101 00010111 01010101 01010010 10011110
                //01010101 11010100 10010101 01110110 10011110

                string result = String.Join("", arrayValues);

                binaryStringXBlocks[counter] = SplitPart(result, 8);
                counter++;
            }

            //8-bit blocks as bytes
            //0x55 0x17 0x55 0x52 0x9E
            //0x55 0xD4 0x95 0x76 0x9E

            counter = 0;
            foreach (string row in binaryStringXBlocks)
            {
                string[] arrayValues = row.Split(' ');

                short count = 0;
                foreach (var item in arrayValues)
                {
                    arrayValues[count] = BinaryStringToHexString(item);
                    count++;
                }

                binaryStringXBlocks[counter] = String.Join(" ", arrayValues);
                counter++;
            }

            //Steps 4/5: add protocols 
            //0xC6 0x55 0x17 0x55 0x52 0x9E 0x6B
            //0xC6 0x55 0xD4 0x95 0x76 0x9E 0x21

            temp = "";
            counter = 0;
            foreach (var item in binaryStringXBlocks)
            {
                temp = item;

                if ((binaryStringXBlocks.Length - 1) == counter)
                    temp = "C6 " + temp + " 21";
                else
                    temp = "C6 " + temp + " 6B";

                binaryStringXBlocks[counter] = temp;
                counter++;
            }

            //concatenate
            //0xC6 0x55 0x17 0x55 0x52 0x9E 0x6B 0xC6 0x55 0xD4 0x95 0x76 0x9E 0x21 (encoding result)
            string resultConcatenated = String.Join(" ", binaryStringXBlocks);

            return resultConcatenated;
        }

        static string GetTable1ValueFromKey(string value)
        {
            string result = "";
            table1.TryGetValue(value, out result);
            return result;
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace(" ", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        static void JoinAndSplit(string[] hexStringArray, short qtdSplit, ref string[] binaryStringXBlocks)
        {
            string binarystring = "";
            short counter = 0;

            foreach (string item in hexStringArray)
            {
                string[] arrayValues = item.Split(' ');

                foreach (var value in arrayValues)
                {
                    //Data bytes as 8-bit blocks 
                    //01010111 01010100 10010101 01011110 10011110
                    //01010101 00010111 01010101 01010010 10011110

                    binarystring += String.Join(String.Empty, value.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                }

                //Bits regrouped into X-bit blocks 
                //01010 11101 01010 01001 01010 10111 10100 11110
                //01010 10100 01011 10101 01010 10100 10100 11110

                binaryStringXBlocks[counter] = SplitPart(binarystring, qtdSplit);
                binarystring = String.Empty;
                counter++;
            }            
        }

        static string SplitPart(string value, short qtdSplit)
        {
            var parts = value.SplitInParts(qtdSplit);
            return (String.Join(" ", parts)); 
        }

        static string GetTable1KeyFromValue(string value)
        {
            var result = table1.Where(p => p.Value == value).Select(p => p.Key);

            foreach (var key in result) 
                return key;

            return "";
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string ByteArrayToHexString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            hex = hex.Replace("-00", "").Replace("-", " ");
            return hex;
        }

        static string StringToHexString(string text)
        {
            byte[] ba = Encoding.Default.GetBytes(text);          
            var hexString = BitConverter.ToString(ba);
            return hexString.Replace("-", " ");
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public static string BinaryStringToHexString(string binary)
        {
            StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

            int mod4Len = binary.Length % 8;
            if (mod4Len != 0)
            {
                // pad to length multiple of 8
                binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
            }

            for (int i = 0; i < binary.Length; i += 8)
            {
                string eightBits = binary.Substring(i, 8);
                result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
            }

            return result.ToString();
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        static int DivisibleBy4(int value)
        {
            int i = value;

            while (true)
            {
                if (i % 4 == 0)
                    break;
                else
                    i++;
            }
            return i;
        }
    }

    static class StringExtensions
    {
        public static IEnumerable<String> SplitInParts(this String s, Int32 partLength)
        {
            if (s == null)
                throw new ArgumentNullException("s");
            if (partLength <= 0)
                throw new ArgumentException("Part length has to be positive.", "partLength");

            for (var i = 0; i < s.Length; i += partLength)
                yield return s.Substring(i, Math.Min(partLength, s.Length - i));
        }
    }
}
