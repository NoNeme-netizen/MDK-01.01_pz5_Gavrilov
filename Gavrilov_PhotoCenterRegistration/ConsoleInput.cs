using System;

namespace Gavrilov_PhotoCenterRegistration
{
    internal static class ConsoleInput
    {
        public static string ReadValue(string message, int maxLength)
        {
            while (true)
            {
                Console.Write(message);
                string value = Console.ReadLine().Trim();

                if (value.Length == 0)
                {
                    Console.WriteLine("Поле не может быть пустым.");
                    continue;
                }

                if (value.Length > maxLength)
                {
                    Console.WriteLine("Слишком длинное значение, максимум " + maxLength + " символов.");
                    continue;
                }

                return value;
            }
        }

        public static string ReadPassword(string message)
        {
            Console.Write(message);

            string password = "";

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                    continue;
                }

                password = password + key.KeyChar;
                Console.Write("*");
            }

            return password;
        }
    }
}