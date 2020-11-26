using System;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Args;
using Newtonsoft.Json.Linq;

namespace telegramBot
{
    class Program
    {
        static string Temper(string Title)
        {
            HttpClient httpClient = new HttpClient();
            try
            {
                string url = $" "; //api openweathermap
                string data = httpClient.GetStringAsync(url).Result;
                dynamic r = JObject.Parse(data);
                return $"В городе {Title} {r.main.temp}°c";

            }
            catch(Exception)
            {
                return "Ошибка";
            }
        }

      

        static void Main(string[] args)
        {
            string token = " "; //Токен бота телеги
            
            TelegramBotClient telegramBotClient = new TelegramBotClient(token);

            //Получаем сообщение
            telegramBotClient.OnMessage +=
                delegate (object sender, MessageEventArgs e)
                {
                    if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Sticker)
                    {
                        telegramBotClient.SendStickerAsync(e.Message.Chat.Id, e.Message.Sticker.FileId);
                    }
                    else if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
                    {
                        Console.WriteLine(e.Message.Text);
                        telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id,Temper(e.Message.Text));
                    }
                    else
                    {
                        Console.WriteLine("Кинули не понятно что");
                        telegramBotClient.SendTextMessageAsync(e.Message.Chat.Id, "Прикол");
                    }
                };
            telegramBotClient.StartReceiving();
            Console.ReadKey();

        }
        
    }
}
