using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostponedPosting.Persistence.ApplicationService.Abstract
{
    public interface ICryptoService
    {
        string GetEncryptedValue(string key, string value);
        string GetDecryptedValue(string key, string value);
    }
}
