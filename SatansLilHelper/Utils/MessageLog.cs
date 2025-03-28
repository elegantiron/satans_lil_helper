using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SatansLilHelper.Constants;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils;

public class MessageLog
{
    private List<Message> _messages;

    public List<(string, Color)> Messages
    {
        get
        {
            List<(string, Color)> messageList = [];
            if (_messages.Count > 0)
            {
                foreach (var message in _messages)
                {
                    messageList.Add(message.FullText);
                }
            }
            return messageList;
        }
    }

    public MessageLog()
    {
        _messages = [];
        EventBus.Subscribe<LogMessage>(this, Events.AddLogMessage, AddMessage);
        EventBus.Subscribe<LogMessage>(this, Events.PruneLogMessage, RemoveMessage);
    }

    private class Message(string text, Color color)
    {
        private string _text = text;
        private Color _color = color;
        private int _count = 1;

        public int Count
        {
            get { return _count; }
        }
        public string PlainText
        {
            get { return _text; }
        }

        public void Stack()
        {
            _count++;
        }

        public void UnStack()
        {
            _count--;
        }

        public (string, Color) FullText
        {
            get { return ($"{_text}{(Count > 1 ? string.Format(" (x{0})", Count) : "")}", _color); }
        }
    }

    public void AddMessage(string text, Color color, bool stack = true)
    {
        if (stack && _messages.Count > 0 && _messages[^1].PlainText == text)
            _messages[^1].Stack();
        else
        {
            _messages.Add(new Message(text, color));
        }
    }

    public void AddMessage(LogMessage logMessage)
    {
        AddMessage(logMessage.Message, logMessage.Color, logMessage.Stack);
    }

    public void RemoveMessage(LogMessage logMessage)
    {
        RemoveMessage(logMessage.Message);
    }

    public void RemoveMessage(string text)
    {
        if (_messages.Count > 0 && _messages[^1].PlainText == text)
        {
            if (_messages[^1].Count > 1)
                _messages[^1].UnStack();
            else
                _messages.RemoveAt(_messages.Count - 1);
        }
    }
}
