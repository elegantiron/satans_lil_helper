using System;
using System.IO;
using System.Reflection;

try
{
    using var game = new SatansLilHelper.Engine();
    game.Run();
}
catch (Exception exception)
{
    string FILE_PATH = AppContext.BaseDirectory + "error.txt";
    using (StreamWriter writer = new StreamWriter(FILE_PATH, true))
    {
        writer.WriteLine("Date : " + DateTime.Now.ToString());
        writer.WriteLine();

        while (exception != null)
        {
            writer.WriteLine(exception.GetType().FullName);
            writer.WriteLine("Message : " + exception.Message);
            writer.WriteLine(" StackTrace : " + exception.StackTrace);

            exception = exception.InnerException;
        }
    }
}
