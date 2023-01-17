using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Helper
{
    public interface IDataProtectionRepository
    {
        string Encrypt(string input);
        string Decrypt(string cipherText);
    }
}
