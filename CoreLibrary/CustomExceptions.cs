
namespace CoreLibrary
{
    public class CustomExceptions
    {
        public static void Handler(Exception ex)
        {
            switch (ex)
            {
                case AggregateException:
                    Console.WriteLine("Bu hata sistem tarafından gerçekleştirilmiştir.");
                    break;

                case BerkeExcpt:
                    Console.WriteLine("Burada istediğimiz işlemleri yapabiliriz.");
                    break;


                default:
                    // Default Yapılandırma.
                    Console.WriteLine("Bu hata türü için özel bir ayar yapılandırılmamış.");
                    break;
            }
        }

        /// <summary>
        /// Buradaki hatalar alındığı anda içerisindeki işlemler gerçekleştirilir.
        /// </summary>
        public class BerkeExcpt : CustomExceptionBase
        {
            public BerkeExcpt()
            {

            }
            public BerkeExcpt(string? message = null) : base(message) { }
        }

        public class SemihExcpt : CustomExceptionBase
        {
            public SemihExcpt()
            {

            }
            public SemihExcpt(string? message = null) : base(message) { }
        }

        public class TalhaExcpt : CustomExceptionBase
        {
            public TalhaExcpt()
            {

            }
            public TalhaExcpt(string? message = null) : base(message) { }
        }


    }

    public class CustomExceptionBase : Exception
    {
        private const string _defaultMessage = "Bu hata mesajı otomatik olarak üretilmiştir.";
        public CustomExceptionBase() { }
        public CustomExceptionBase(string? message = null) : base(message)
        {
            if (message.IsNullOrEmpty().NotThis()) workingMethod(message);
            else Console.WriteLine(_defaultMessage);
        }

        private static void workingMethod(string? message)
        {
            if (message.IsNullOrEmpty().NotThis()) Console.WriteLine(message);
        }
    }
}
