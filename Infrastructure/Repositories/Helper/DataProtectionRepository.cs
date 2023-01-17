using Core.Interface.Helper;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Helper
{
    public class DataProtectionRepository : IDataProtectionRepository
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private const string _Key = "lwl8NMlayp7xJGE45DpqvHXaB3sgJ5lD9C";

        public DataProtectionRepository(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }
        public string Encrypt(string input)
        {
            var protector = _dataProtectionProvider.CreateProtector(_Key);
            return protector.Protect(input);
        }
        public string Decrypt(string cipherText)
        {
            var protector = _dataProtectionProvider.CreateProtector(_Key);
            return protector.Unprotect(cipherText);
        }
    }
}
