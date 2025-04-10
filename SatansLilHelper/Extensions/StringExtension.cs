using System.Collections.Generic;

using Microsoft.Xna.Framework.Graphics;

namespace SatansLilHelper.Extensions;

public static class StringExtension
{
    public static List<string> Wrap(this string text, SpriteFont font, int maxLength)
    {
        if (text.Length == 0)
            return [];

        string[] words = text.Split(" ");
        List<string> lines = [];
        string currentLine = "";

        foreach (string currentWord in words)
        {
            int lineLength = (int)font.MeasureString(currentLine).X;
            int wordLength = (int)font.MeasureString((lineLength > 0 ? " " : "") + currentWord).X;

            if ((lineLength > maxLength) || ((lineLength + wordLength) > maxLength))
            {
                lines.Add(currentLine);
                currentLine = "";
            }

            currentLine += (currentLine.Length > 0 ? " " : "") + currentWord;
        }

        if (currentLine.Length > 0)
            lines.Add(currentLine);

        return lines;
    }
}
