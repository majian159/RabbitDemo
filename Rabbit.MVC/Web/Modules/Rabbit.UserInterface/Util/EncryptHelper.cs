using System;
using System.Text;

namespace Rabbit.UserInterface.Util
{
    internal class EncryptHelper
    {
        public static string Encrypt(string content)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        }
    }
}