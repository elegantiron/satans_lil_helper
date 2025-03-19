using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SatansLilHelper.Utils;

public class MessageLog
{
    private List<Message> Messages;

    private class Message(string text, Color color)
    {
        private string Text = text;
        private Color Color = color;
        private int Count = 0;
        public string PlainText
        {
            get { return Text; }
        }

        public void Stack()
        {
            Count++;
        }
    }

    public void AddMessage(string text, Color color, bool stack = true)
    {
        if (stack && Messages[-1].PlainText == text)
            Messages[-1].Stack();
        else
            Messages.Add(new Message(text, color));
    }
}
