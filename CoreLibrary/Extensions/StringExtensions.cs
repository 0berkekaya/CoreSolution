
namespace CoreLibrary.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Belirtilen stringin boş veya null olup olmadığını kontrol eder. Boşluk karakterleri dikkate alınır.
        /// </summary>
        /// <param name="_">Kontrol edilecek string.</param>
        /// <returns>Belirtilen string boş veya null ise true, aksi halde false döner.</returns>
        public static bool IsNullOrEmpty(this string? _) => _ == null || _?.Trim() == string.Empty;

        // Bir string'i tersine çevirir.
        public static string? Reverse(this string str)
        {
            if (str == null) return null;
            char[] charArray = str.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        // Bir string'in ilk harfini büyütür.
        public static string CapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;

            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
