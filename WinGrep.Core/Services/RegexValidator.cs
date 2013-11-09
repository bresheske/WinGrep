using System.Text.RegularExpressions;

namespace WinGrep.Core.Services
{
    /// <summary>
    /// Just a small class which validates regular expressions.
    /// </summary>
    public class RegexValidator
    {
        /// <summary>
        /// Returns true if the regex can be read by the 
        /// .net regex compiler. 
        /// </summary>
        /// <param name="regex">Regex to be validated.</param>
        /// <returns></returns>
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
