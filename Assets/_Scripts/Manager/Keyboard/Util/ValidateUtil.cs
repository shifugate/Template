using System.Text.RegularExpressions;

namespace UnloopLib.Keyboards.Util
{
    public class ValidateUtil
    {
        public static bool ValidName(string name)
        {
            return name.Trim().Length > 0;
        }

        public static bool ValidEmail(string email)
        {
            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}
