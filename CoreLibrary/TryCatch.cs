using CoreLibrary.Interface;
using System.Diagnostics;
using System.Reflection;

namespace CoreLibrary
{
    public static class TryCatch
    {
        /// <summary>
        /// Belirtilen bir işlevi çalıştırır ve işlem bilgisini döndürür.
        /// </summary>
        /// <typeparam name="T">Dönüş değeri türü.</typeparam>
        /// <param name="methodFuncT">Çalıştırılacak işlev.</param>
        /// <param name="tryCatchConfiguration">Try-Catch ayarları (isteğe bağlı).</param>
        /// <returns>İşlem bilgisi.</returns>
        public static IOperationResult<T> Run<T>(Func<T> methodFuncT, TryCatchConfiguration? tryCatchConfiguration = null)
        {
            // Try-Catch ayarlarını varsayılan değerle başlat
            tryCatchConfiguration ??= new TryCatchConfiguration();

            // İşlem bilgisi oluştur
            IOperationResult<T> transaction = new OperationResult<T>();

            try
            {
                // İşlevi çağır ve dönüş değerini ayarla
                transaction.Result = methodFuncT();
                // İşlem başarılı olduğunu işaretle
                transaction.Success = true;
            }
            catch (Exception ex)
            {
                // Hata durumunda istisna bilgisini ve işlem durumunu ayarla
                transaction.Error = ExceptionDetailsBuilder(ex);

                // Hata günlüğünü kaydetme ayarı aktifse
                if (tryCatchConfiguration.Logging) Logger.Instance.Add(LogLevel.Error, Tools.ObjectToJsonString(transaction.Error));

                if (tryCatchConfiguration.CustomExceptionHandling) CustomExceptionMethods.Handler(ex);

                if (tryCatchConfiguration.ThrowOnException) throw;
            }
            //if (tryCatchConfiguration.Logging && transaction.Success) Logger.Log.Push(LogLevel.Success, transaction.Message);
            // İşlem bilgisini döndür
            return transaction;
        }

        /// <summary>
        /// Belirtilen bir işlemi gerçekleştirir ve işlem bilgisini döndürür.
        /// </summary>
        /// <param name="methodAction">Gerçekleştirilecek işlem.</param>
        /// <param name="tryCatchConfiguration">Try-Catch ayarları (isteğe bağlı).</param>
        /// <returns>İşlem bilgisi.</returns>
        public static IOperationResult Run(Action methodAction, TryCatchConfiguration? tryCatchConfiguration = null)
        {
            // Try-Catch ayarlarını varsayılan değerle başlat
            tryCatchConfiguration ??= new TryCatchConfiguration();

            // İşlem bilgisi nesnesini oluştur
            IOperationResult transaction = new OperationResult();

            try
            {
                // İşlemi gerçekleştir
                methodAction();
                // İşlem başarılı olduğunu işaretle
                transaction.Success = true;
            }
            catch (Exception ex)
            {
                // Hata durumunda işlem bilgisini ve durumunu ayarla
                transaction.Error = ExceptionDetailsBuilder(ex);

                // Hata günlüğünü kaydetme ayarı aktifse
                if (tryCatchConfiguration.Logging) Logger.Instance.Add(LogLevel.Error, Tools.ObjectToJsonString(transaction.Error));

                if (tryCatchConfiguration.CustomExceptionHandling) CustomExceptionMethods.Handler(ex);

                if (tryCatchConfiguration.ThrowOnException) throw;
            }
            //if (tryCatchConfiguration.Logging && transaction.Success) Logger.Log.Push(LogLevel.Success);
            // İşlem bilgisini döndür
            return transaction;
        }

        /// <summary>
        /// Bir istisna nesnesinden ayrıntılı hata bilgisini oluşturur.
        /// </summary>
        /// <param name="exception">İstisna nesnesi.</param>
        /// <returns>Oluşturulan özel istisna nesnesi.</returns>
        private static OperationError ExceptionDetailsBuilder(Exception exception)
        {
            OperationError transactionError = new()
            {
                Message = exception.Message,
                Details = []
            };

            StackTrace stackTrace = new(exception, true);
            int frameCount = stackTrace.FrameCount;

            for (int i = 0; i < frameCount; i++)
            {
                StackFrame frame = stackTrace.GetFrame(i)!;
                MethodBase methodInformation = frame.GetMethod()!;

                string? className = methodInformation.ReflectedType?.FullName;
                string methodName = methodInformation.Name;

                int mnStartIndex = methodName.IndexOf('<') + 1;
                int mnEndIndex = methodName.IndexOf('>');

                if (className != ("CoreLibrary.TryCatch"))
                {
                    className = methodInformation.Module.Name.Replace(".dll", "");
                    methodName = methodName[mnStartIndex..mnEndIndex];

                    transactionError.Details.Add(new ExceptionDetail
                    {
                        Row = frame.GetFileLineNumber(),
                        Class = className + "." + methodName,
                    });
                }
            }
            return transactionError;
        }
    }
    /// <summary>
    /// Try-Catch bloklarındaki davranışı yapılandırmak için kullanılan sınıf.
    /// </summary>
    public class TryCatchConfiguration
    {
        /// <summary>
        /// Bir istisna oluştuğunda, try-catch bloğundaki kodun istisnayı tekrar fırlatıp fırlatmayacağını belirten bir değer.
        /// Varsayılan değer: false.
        /// </summary>
        public bool ThrowOnException { get; set; } = false;

        /// <summary>
        /// Hata günlüğüne kaydetme işleminin yapıp yapılmayacağını belirten bir değer.
        /// Varsayılan değer: false.
        /// </summary>
        public bool Logging { get; set; } = false;

        /// <summary>
        /// Özel olarak hatalın kendilerine has işlemlerin yapılması için bir değer.
        /// </summary>
        public bool CustomExceptionHandling { get; set; } = false;
    }
}
