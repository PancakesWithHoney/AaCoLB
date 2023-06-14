using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace WebApplication
{
    public class CookieSec
    {
        public static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = MachineKey.Protect(plainTextBytes, "Purpose_TicketNumber");
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = MachineKey.Unprotect(encryptedBytes, "Purpose_TicketNumber");
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}