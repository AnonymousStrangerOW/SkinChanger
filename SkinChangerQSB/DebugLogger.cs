namespace SkinChangerQSB;

public static class DebugLogger
{
    public static void Write(string message) => SkinChanger.SkinChanger.instance.ModHelper.Console.WriteLine(message);
    public static void WriteError(string message) => SkinChanger.SkinChanger.instance.ModHelper.Console.WriteLine(message, OWML.Common.MessageType.Error);
}
