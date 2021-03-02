using System;
using System.Diagnostics;   // Нужна, чтобы запускать внешние процессы
using System.Net;           // Нужна, чтобы работать с Web
using System.Threading;

namespace SimpleParser
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            Console.WriteLine(DownloadSkinsForAimp(wc));
        }

        static string DownloadSkinsForAimp(WebClient wc)
        {
            Console.WriteLine("Downloading began");
            try
            {
                for (int i = 0; i <= 5; i++)
                {
                    string id = "79" + i;
                    string name = GetNameOfSkin(wc, id);

                    // Путь, откуда мы будем скачивать
                    string path = "aimp.ru/index.php?do=download&sub=catalog&id=" + id;

                    try
                    {
                        // Запускаем нужный нам браузер и передаем ему в качестве аргумента путь скачивания
                        Process.Start("chrome.exe", path);

                        Console.WriteLine("Download " + name + " is succesfull!");

                        // Ждем 5 секунд, чтобы вкладки успевали закрываться, иначе может быть переполнение памяти
                        Thread.Sleep(5000);
                    }
                    catch
                    {
                        Console.WriteLine("Download" + name + "failed");
                    }
                }
            }
            catch
            {
                return "\nSomething went is wrong";
            }
            return "\nDownloading complete";

        }

        static string GetNameOfSkin(WebClient wc, string id)
        {
            // Получаем строку с html разметкой
            string html = wc.DownloadString("http://www.aimp.ru/index.php?do=catalog&rec_id=" + id);
            // Находим в ней первое упоминание нужного нам id и удаляем ненужную левую часть
            // Название скина начинается через 5 символов после этого id
            string rightPartOfHtml = html.Substring(html.IndexOf(id) + 5);
            // Находим конец названия и удаляем оставшуюся правую часть
            string name = rightPartOfHtml.Substring(0, rightPartOfHtml.IndexOf("<")).Replace(" ", "_");
            // В итоге нам возвращается только само название скина
            return name;
        }
    }
}
