using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using Bot2.Models;
using System.Text;

namespace Bot2
{
    public class Bot
    {
        public string lastindexId;
        public string lastfavorId;
        TelegramBotClient botClient = new TelegramBotClient("5169484233:AAEr8BddqPpep--xgb2WTqu8bsRkv_kv6rc");
        CancellationToken source = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };
        public async Task Start()
        {
            ClientBot clientBot = new ClientBot();
            var responce1 = await clientBot.Client.GetAsync($"Db/LastId");
            int last = int.Parse(responce1.Content.ReadAsStringAsync().Result);
            last++;
            lastindexId = last.ToString();
            
            var responce2 = await clientBot.Client.GetAsync($"/DbPlayer/LastId");
            int last2 = int.Parse(responce2.Content.ReadAsStringAsync().Result);
            last2++;
            lastfavorId = last2.ToString();

            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, source);
            var BotMe = await botClient.GetMeAsync();
            Console.WriteLine($"Бот{BotMe.Username} начал работу");
            Console.ReadKey();

        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch //метод принимает ошибки, и выводит апи ошибку в бота.
            {
                ApiRequestException apiRequestException => $"Ошибка в телеграм бот АПИ:\n{apiRequestException.ErrorCode}" +
                $"\n{ apiRequestException.Message}",
                _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandlerCallbackQueryAsync(botClient, update.CallbackQuery);

            }


            //метод отвечает за соообщения которые обновляются в чате
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                await HandlerMessageAsync(botClient, update.Message);
            }
        }

        private async Task HandlerCallbackQueryAsync(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            int last;
            ClientBot clientBot = new ClientBot();
            CallBackDat callback = JsonConvert.DeserializeObject<CallBackDat>(callbackQuery.Data);

            switch (callback.Action)
            {
                case "AddFavorite":
                    var response = await clientBot.Client.GetAsync($"/NBAControllers/IDPLAYER?Id={callback.Id}");
                    ///DbPlayer/add
                    ///

                    string content = response.Content.ReadAsStringAsync().Result;

                    ID dat = JsonConvert.DeserializeObject<ID>(content);

                    PlayerInfoDb addplayer = new PlayerInfoDb
                    {
                        Id = lastfavorId,
                        IdPlayer = dat.id,
                        first_name = dat.first_name,
                        last_name = dat.last_name,
                        position = dat.position,
                        Team_Full_Name = dat.Team.full_name,
                        UserId = callbackQuery.Message.From.Id.ToString()
                    };

                    last = int.Parse(lastfavorId);
                    last++;
                    lastfavorId = last.ToString();
                    var json = JsonConvert.SerializeObject(addplayer);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var post = await clientBot.Client.PostAsync("/DbPlayer/add", data);
                    post.EnsureSuccessStatusCode();

                    await botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "Вы добавили игрока в избранное");

                    return;
            }
        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            int last = 0;
            int lastplayer = 0;

            switch (message.Text)
            {
                case "/start":
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Виберите команду /keyboard");
                    return;
                case "/keyboard":
                    ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                    {
                        new KeyboardButton [] {"Данные игрока", "Данные команды"},
                        new KeyboardButton [] {"Статистика игрока", "Избранные игроки"},
                        new KeyboardButton [] {"Поиск по Youtube"},
                        
                    }
                    )
                    {
                        ResizeKeyboard = true
                    };
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Виберите пункт меню:", replyMarkup: replyKeyboardMarkup);
                    return;
                case "Данные игрока":
                    RequestAdder.AddRecuestAsync(lastindexId, message.From.Id.ToString(), "#Данные игрока");
                    last = int.Parse(lastindexId);
                    last++;
                    lastindexId = last.ToString();

                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите имя игрока");
                    return;
                case "Статистика игрока":
                    RequestAdder.AddRecuestAsync(lastindexId, message.From.Id.ToString(), "#Статистика игрока");
                    last = int.Parse(lastindexId);
                    last++;
                    lastindexId = last.ToString();
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите Id игрока");
                    return;
                
                case "Поиск по Youtube":
                    RequestAdder.AddRecuestAsync(lastindexId, message.From.Id.ToString(), "#Поиск по Youtube");
                    last = int.Parse(lastindexId);
                    last++;
                    lastindexId = last.ToString();
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите запрос");
                    return;
                case "Избранные игроки":
                    RequestAdder.AddRecuestAsync(lastindexId, message.From.Id.ToString(), "#Показать избранных игроков");
                    last = int.Parse(lastindexId);
                    last++;
                    lastindexId = last.ToString();
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите Id");
                    return;
                case "Данные команды":
                    RequestAdder.AddRecuestAsync(lastindexId, message.From.Id.ToString(), "#Данные команды");
                    last = int.Parse(lastindexId);
                    last++;
                    lastindexId = last.ToString();
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Введите Id команды от 1до 30 \n1: Atlanta Hawks\n2: Boston Celtics \nBrooklyn Nets .... и т.д.");
                    return;

            }
            ClientBot clientBot1 = new ClientBot();
            var Responce1 = await clientBot1.Client.GetAsync($"/Db/LastMode?userId={message.From.Id}");
            string Content = Responce1.Content.ReadAsStringAsync().Result;

            switch (Content)
            {
                case "#Данные игрока":
                    ClientBot clientBot = new ClientBot();
                    var responce1 = await clientBot.Client.GetAsync($"/NBAControllers/Stats?player={message.Text}");
                    string content = responce1.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<PlayerInfo>(content);
                    if (result == null)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка ввода,такого игрока не сушествует");
                        return;
                    }
                    CallBackDat callBack = new CallBackDat()
                    {
                        Id = result.id.ToString(),
                        Action = "AddFavorite"
                            
                    };
                    var json = JsonConvert.SerializeObject(callBack);

                    InlineKeyboardMarkup inlineKeyboardMarkup = new
                    (
                    new[]
                    {

                         InlineKeyboardButton.WithCallbackData("Add to favorite", json)
                     }
                    );

                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Данные:\nИмя: {result.first_name}\nФамилия: { result.last_name}\nId: { result.id}\nПозиция: { result.position}\nКоманда: { result.team.Abbreviation}\n{ result.team.full_name}", replyMarkup: inlineKeyboardMarkup);

                    return;
                case "#Статистика игрока":
                    ClientBot clientBot3 = new ClientBot();
                    var responce3 = await clientBot3.Client.GetAsync($"/NBAControllers/Average?playerId={message.Text}&season=2021");
                        string content3 = responce3.Content.ReadAsStringAsync().Result;
                        var result3 = JsonConvert.DeserializeObject<PlayerStatics>(content3);
                    if(result3 == null || result3.player_id == 0)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка ввода, такого Id игрока не сушествует");
                        return;
                    }
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Данные:\nСыграно Игр: {result3.games_played}\nId Игрока:{ result3.player_id}\nСезон: { result3.Season}\nМин времени на поле: { result3.Min}\nFgm: { result3.Fgm}\nFga: { result3.Fga}\nFg3m: { result3.Fg3m}\nFg3a: { result3.Fg3a}\nFtm: { result3.Ftm}\nFta: { result3.Fta}\nOreb: { result3.Oreb}");
                    return;
                case "#Поиск по Youtube":
                    ClientBot clientBot4 = new ClientBot();
                    var responce4 = await clientBot4.Client.GetAsync($"/YouTubeApi/videosbyrequest?request={message.Text}");
                    if (responce4.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        List<Models.Video> videos = JsonConvert.DeserializeObject<List<Models.Video>>(responce4.Content.ReadAsStringAsync().Result);

                        if (videos.Count != 0)
                        {
                            List<List<InlineKeyboardButton>> buttons = new List<List<InlineKeyboardButton>> { };

                            for (int i = 0; i < videos.Count; i++)
                            {
                                buttons.Add(new List<InlineKeyboardButton>
                        {
                            InlineKeyboardButton.WithUrl($"{videos[i].VideoTitle}", @"https://www.youtube.com/watch?v=" + videos[i].VideoId)
                        }
                                );
                                CallBackDat callBack4 = new CallBackDat()
                                {
                                    Id = videos[i].VideoId,
                                };
                                var json1 = JsonConvert.SerializeObject(callBack4);
                                CallBackDat callBack2 = new CallBackDat()
                                {
                                    Id = videos[i].VideoId,
                                };
                                var json4 = JsonConvert.SerializeObject(callBack2);
                            }

                                InlineKeyboardMarkup keyboardMarkup = new InlineKeyboardMarkup
                                    (
                                    buttons
                                    );
                                await botClient.SendTextMessageAsync(message.Chat.Id, "Ваш запрос", replyMarkup: keyboardMarkup);
                        }
                        else
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Нет результата по вашему запросу");
                        }


                    }
                    return;

                case "#Показать избранных игроков":
                    ClientBot clientBot5 = new ClientBot();
                    var responce5 = await clientBot5.Client.GetAsync($"/DbPlayer/ById?id={message.Text}");
                    string content5 = responce5.Content.ReadAsStringAsync().Result;
                    if (content5 == "Obj not found")
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка ввода, такого Id игрока не сушествует в базе");
                        
                    }
                    else 
                    if(content5 != "Obj not found")
                    {
                        var result5 = JsonConvert.DeserializeObject<PlayerInfoDb>(content5);
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Номер в базе: {result5.Id}\nИмя: {result5.first_name}\nИмя: {result5.last_name}\n Команда: {result5.Team_Full_Name}\nId игрока: {result5.IdPlayer}\nПозиция: {result5.position}");
                    }
                    return;

                case "#Данные команды":

                    if (int.TryParse(message.Text , out int f)&&  int.Parse(message.Text) <=30 && int.Parse(message.Text) >= 1)
                    {
                        ClientBot clientBot6 = new ClientBot();
                        var responce6 = await clientBot6.Client.GetAsync($"/NBAControllers/Team?ID={message.Text}");
                        string content6 = responce6.Content.ReadAsStringAsync().Result;
                        var result6 = JsonConvert.DeserializeObject<Team>(content6);
                        if (result6 == null)
                        {
                            await botClient.SendTextMessageAsync(message.Chat.Id, "Ошибка ввода, такого Id команды не существует");
                            return;
                        }
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Id: {result6.Id}\nАбривиатура: {result6.Abbreviation}\nГород: {result6.City}\nКонференция: {result6.Conference}\nДивизон: {result6.Division}\nПолное имя: {result6.full_name}\nИмя: {result6.Name}");
                        return;
                    }
                    else
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, "Введите число с диапазона");
                        return;

                    }

            }

        }


    }
}
