﻿namespace ChatApp.Api.Models;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int UserId { get; set; }
    public User User { get; set; }
    public int ChatId { get; set; }
    public Chat Chat { get; set; }
}