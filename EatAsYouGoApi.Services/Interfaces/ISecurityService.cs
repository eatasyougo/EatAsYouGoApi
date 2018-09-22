namespace EatAsYouGoApi.Services.Interfaces
{
    public interface ISecurityService
    {
        /// ----------- The commonly used methods ------------------------------    
        /// Encrypt some text and return a string suitable for passing in a URL.
        string EncryptToString(string textValue);

        /// Encrypt some text and return an encrypted byte array.
        byte[] Encrypt(string textValue);

        /// The other side: Decryption methods
        string DecryptString(string encryptedString);

        /// Decryption when working with byte arrays.    
        string Decrypt(byte[] encryptedValue);

        /// Convert a string to a byte array.  NOTE: Normally we'd create a Byte Array from a string using an ASCII encoding (like so).
        //      System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //      return encoding.GetBytes(str);
        // However, this results in character values that cannot be passed in a URL.  So, instead, I just
        // lay out all of the byte values in a long string of numbers (three per - must pad numbers less than 100).
        byte[] StrToByteArray(string str);

        string ByteArrToString(byte[] byteArr);

        /// -------------- Two Utility Methods (not used but may be useful) -----------
        /// Generates an encryption key.
        byte[] GenerateEncryptionKey();

        /// Generates a unique encryption vector
        byte[] GenerateEncryptionVector();
    }
}