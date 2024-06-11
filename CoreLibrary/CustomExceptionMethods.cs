
namespace CoreLibrary
{
    public class CustomExceptionMethods
    {
        public static void Handler(Exception ex)
        {
            switch (ex)
            {
                case AggregateException:
                    Console.WriteLine("Bu hata sistem tarafından gerçekleştirilmiştir.");
                    break;

                case CustomExceptions.BerkeExcpt:
                    Console.WriteLine("Burada istediğimiz işlemleri yapabiliriz.");
                    break;


                default:
                    // Default Yapılandırma.
                    Console.WriteLine("Bu hata türü için özel bir ayar yapılandırılmamış.");
                    break;
            }
        }

    }
}
