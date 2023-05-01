using System;
using System.Collections.Generic;

namespace PogodaCityBot;

public partial class Notification
{
    public long Id { get; set; }

    public string? ChatId { get; set; }

    public string? NameCity { get; set; }

    public string? Time { get; set; }

    public int Done { get; set; }

    public Notification(string? chatId, string? nameCity, string? time, int done)
    {
        ChatId = chatId;
        NameCity = nameCity;
        Time = time;
        Done = done;
    }
}
