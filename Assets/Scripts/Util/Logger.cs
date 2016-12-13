using UnityEngine;

//#if LOGGER_BUILD 

public enum LogLevel {
    Debug,
    Info,
    Warning,
    Error
};

public class Logger {
    static LogLevel logLevelForSendingServer = LogLevel.Debug;
    public delegate void OnSendLog(string msg, LogLevel logLevel);
    public static event OnSendLog onSendLog = null;

    public static void Log(object message) {
        Log(ObjectToString(message), LogLevel.Debug);
    }

    public static void Log(string format, params Object[] args) {
        string message = GetFormatString(format, args);
        Log(message, LogLevel.Debug);
    }

    public static void LogInfo(object message) {
        Log(ObjectToString(message), LogLevel.Info);
    }

    public static void LogInfo(string format, params Object[] args) {
        string message = GetFormatString(format, args);
        Log(message, LogLevel.Info);
    }

    public static void LogWarning(object message) {
        Log(ObjectToString(message), LogLevel.Warning);
    }

    public static void LogWarning(string format, params Object[] args) {
        string message = GetFormatString(format, args);
        Log(message, LogLevel.Warning);
    }

    public static void LogError(object message) {
        Log(ObjectToString(message), LogLevel.Error);
    }

    public static void LogError(string format, params Object[] args) {
        string message = GetFormatString(format, args);
        Log(message, LogLevel.Error);
    }

    static void Log(string msg, LogLevel logLevel) {
        if (logLevel == LogLevel.Debug) {
            Debug.Log(msg);
        } else if (logLevel == LogLevel.Info) {
            Debug.LogWarning(msg);
        } else if (logLevel == LogLevel.Warning) {
            Debug.LogWarning(msg);
        } else if (logLevel == LogLevel.Error) {
            Debug.LogError(msg);
        }

        if (logLevel >= logLevelForSendingServer) {
            if (onSendLog != null) {
                onSendLog(msg, logLevel);
            }
        }
    }

    static string ObjectToString(object message) {
        string text = "";
        if (message != null) {
            text = message.ToString();
        }
        return text;
    }

    static string GetFormatString(string format, params Object[] args) {
        if (format == null) {
            return "";
        }

        if (args.Length == 0) {
            return format;
        }

        return string.Format(format, args);
    }
}

//#endif