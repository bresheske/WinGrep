using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinGrep.Core.Services
{
    public class RegexValidator
    {
        public bool Validate(string regex)
        {
            try
            {
                new Regex(regex);
                return true;
            }
            catch { }
            return false;
        }
    }
}
