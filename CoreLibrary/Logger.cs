public enum LogLevel
{
    Success,
    Error,
    Info
}

public sealed class Logger
{
    private static readonly Lazy<Logger> lazyInstance = new Lazy<Logger>(() => new Logger());
    public static Logger Instance => lazyInstance.Value;

    private readonly string logDateTimeFormat = "dd.MM.yyyy | HH:mm:ss";
    private readonly string logSeparator = "==>";
    private readonly string logFolderPath;
    private const string logFileType = "txt";

    private Logger()
    {
        logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");

        if (!Directory.Exists(logFolderPath)) Directory.CreateDirectory(logFolderPath);
    }

    public void Add(LogLevel level, string? message, string? logFileName = null)
    {
        string logPrefix = level switch
        {
            LogLevel.Success => "[SUCCESS]",
            LogLevel.Error => "[ERROR]",
            LogLevel.Info => "[INFO]",
            _ => "[UNKNOWN]"
        };

        string defaultMessage = level switch
        {
            LogLevel.Success => "İşlem Başarılı.",
            LogLevel.Error => "İşlem Başarısız.",
            LogLevel.Info => "İşlem Bilgisi.",
            _ => "Bilinmeyen işlem."
        };

        message ??= defaultMessage;

        string logMessage = string.Join(' ', logPrefix, getCurrentDateTime(), logSeparator, message);

        logFileName ??= "log";

        logToFile(logMessage, string.Join('.', logFileName, logFileType));
    }

    private void logToFile(string logMessage, string fileName)
    {
        string logFilePath = Path.Combine(logFolderPath, fileName);
        using StreamWriter writer = new StreamWriter(logFilePath, true);
        writer.WriteLine(logMessage);
    }

    private string getCurrentDateTime()
    {
        return "[" + DateTime.Now.ToString(logDateTimeFormat) + "]";
    }
}
