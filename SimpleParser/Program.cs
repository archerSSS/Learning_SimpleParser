using System;
using System.Diagnostics;   // Нужна, чтобы запускать внешние процессы
using System.IO;
using System.Net;           // Нужна, чтобы работать с Web
using System.Threading;

namespace SimpleParser
{
    //https://vk.com/id20210102?z=photo20210102_457243035%2Falbum20210102_0%2Frev
    //aimp.ru/index.php?do=download&sub=catalog&id=
    //http://www.aimp.ru/index.php?do=catalog&rec_id=

    class Program
    {
        static void Main(string[] args)
        {
            AgileParser AP = new AgileParser();
            AP.Parse();

            /*ParserObject PO = new ParserObject();
            PO.Parsing();*/

            /*string sn = "https://vk.com/id20210102?z=photo20210102_457243035%2Falbum20210102_0%2Frev";
            WebClient wc = new WebClient();
            wc.DownloadFile("https://vk.com/id20210102?z=photo20210102_457243035%2Falbum20210102_0%2Frev", "JNX8dh4MuL8.jpg");*/
            

            //Console.WriteLine(DownloadSkinsForAimp(wc, sn));
        }

        static string DownloadSkinsForAimp(WebClient wc, string leftPart)
        {
            string downLoadLeftPart = "";

            Console.WriteLine("Downloading began");
            try
            {
                for (int i = 0; i <= 1; i++)
                {
                    string id = "79" + i;
                    string name = "";//GetNameOfSkin(wc, id, downLoadLeftPart);

                    // Путь, откуда мы будем скачивать
                    string path = leftPart + id;

                    try
                    {
                        // Запускаем нужный нам браузер и передаем ему в качестве аргумента путь скачивания
                        // google chrome
                        Process.Start("chrome.exe", "https://vk.com/id20210102?z=photo20210102_457243035%2Falbum20210102_0%2Frev");

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

        static string GetNameOfSkin(WebClient wc, string id, string leftPart)
        {
            // Получаем строку с html разметкой
            string html = wc.DownloadString(leftPart + id);
            
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
