using BC = BCrypt.Net.BCrypt;
namespace DLMS_DAL.Helpers
{
    public static class BCryptHelper
    {
        /// <summary>
        /// CryptPassword
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CryptPassword(string password)
        {
            return BC.HashPassword(password);
        }

        /// <summary>
        /// VerifyPassword
        /// </summary>
        /// <param name="data"></param>
        /// <param name="hashData"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string hashPassword)
        {
            return BC.Verify(password, hashPassword);
        }
    }
}
