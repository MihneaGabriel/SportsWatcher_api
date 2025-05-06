namespace SportsWatcher.WebApi.Utils
{
    public class GeneratePasswordUtils
    {
        public static string GenerateRandomPassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var res = new char[length];
            var rng = new Random();

            for (int i = 0; i < length; i++)
                res[i] = valid[rng.Next(valid.Length)];

            return new string(res);
        }
    }
}
