using System;
using System.Collections.Generic;
using NLog;
using NLog.Targets;
using Sentry;

namespace BAUERGROUP.Shared.Core.ErrorTracking
{
    /// <summary>
    /// NLog Target für Sentry Integration
    /// Ermöglicht transparente Weiterleitung von BGLogger an Sentry
    ///
    /// Verwendung in BGLoggerConfiguration:
    /// BGLogger.Configuration.ErrorTracking = true;
    /// </summary>
    [Target("BGErrorTracking")]
    public class BGErrorTrackingTarget : TargetWithLayout
    {
        /// <summary>
        /// Minimum Log Level ab dem Events an Sentry gesendet werden
        /// </summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Error;

        /// <summary>
        /// Nur Exceptions senden (keine reinen Nachrichten)
        /// </summary>
        public bool ExceptionsOnly { get; set; } = false;

        /// <summary>
        /// Breadcrumbs für niedrigere Log-Level aktivieren
        /// </summary>
        public bool AddBreadcrumbs { get; set; } = true;

        /// <summary>
        /// Log Level ab dem Breadcrumbs hinzugefügt werden
        /// </summary>
        public LogLevel BreadcrumbMinLevel { get; set; } = LogLevel.Debug;

        public BGErrorTrackingTarget()
        {
            Name = "BGErrorTracking";
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (!BGErrorTracking.IsInitialized)
                return;

            // Breadcrumbs für niedrigere Level
            if (AddBreadcrumbs && logEvent.Level < MinLevel && logEvent.Level >= BreadcrumbMinLevel)
            {
                WriteBreadcrumb(logEvent);
                return;
            }

            // Events nur ab MinLevel senden
            if (logEvent.Level < MinLevel)
                return;

            // Wenn ExceptionsOnly aktiv und keine Exception vorhanden, nur Breadcrumb
            if (ExceptionsOnly && logEvent.Exception == null)
            {
                if (AddBreadcrumbs)
                    WriteBreadcrumb(logEvent);
                return;
            }

            // Event an Sentry senden
            WriteEvent(logEvent);
        }

        private void WriteBreadcrumb(LogEventInfo logEvent)
        {
            var message = Layout.Render(logEvent);
            var level = MapToBreadcrumbLevel(logEvent.Level);

            var data = new Dictionary<string, string>
            {
                { "logger", logEvent.LoggerName },
                { "level", logEvent.Level.Name }
            };

            if (logEvent.Exception != null)
            {
                data["exception_type"] = logEvent.Exception.GetType().Name;
                data["exception_message"] = logEvent.Exception.Message;
            }

            BGErrorTracking.AddBreadcrumb(
                message: message,
                category: "nlog",
                type: "default",
                data: data,
                level: level
            );
        }

        private void WriteEvent(LogEventInfo logEvent)
        {
            var message = Layout.Render(logEvent);
            var sentryLevel = MapToSentryLevel(logEvent.Level);

            BGErrorTracking.CaptureException(logEvent.Exception, message, sentryLevel);
        }

        private static SentryLevel MapToSentryLevel(LogLevel level)
        {
            if (level == LogLevel.Fatal)
                return SentryLevel.Fatal;
            if (level == LogLevel.Error)
                return SentryLevel.Error;
            if (level == LogLevel.Warn)
                return SentryLevel.Warning;
            if (level == LogLevel.Info)
                return SentryLevel.Info;

            return SentryLevel.Debug;
        }

        private static BreadcrumbLevel MapToBreadcrumbLevel(LogLevel level)
        {
            if (level == LogLevel.Fatal)
                return BreadcrumbLevel.Critical;
            if (level == LogLevel.Error)
                return BreadcrumbLevel.Error;
            if (level == LogLevel.Warn)
                return BreadcrumbLevel.Warning;
            if (level == LogLevel.Info)
                return BreadcrumbLevel.Info;

            return BreadcrumbLevel.Debug;
        }

        internal static LogLevel MapFromErrorTrackingLevel(ErrorTrackingLogLevel level)
        {
            switch (level)
            {
                case ErrorTrackingLogLevel.Trace:
                    return LogLevel.Trace;
                case ErrorTrackingLogLevel.Debug:
                    return LogLevel.Debug;
                case ErrorTrackingLogLevel.Info:
                    return LogLevel.Info;
                case ErrorTrackingLogLevel.Warn:
                    return LogLevel.Warn;
                case ErrorTrackingLogLevel.Error:
                    return LogLevel.Error;
                case ErrorTrackingLogLevel.Fatal:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Error;
            }
        }
    }
}
