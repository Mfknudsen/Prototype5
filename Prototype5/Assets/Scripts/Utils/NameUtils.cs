using System;
using System.Text.RegularExpressions;

namespace Utils
{
    public static class NameUtils
    {
        public static string RemovePotionPrefix(string name)
        {
            const string PREFIX = "Potion";
            if (name.Equals(PREFIX)) return name;
            return name.StartsWith(PREFIX) ? name.Substring(PREFIX.Length + 1) : name;
        }

        public static string RemovePrefabFromName(string name)
        {
            const string PREFAB = "prefab";

            int index = name.IndexOf(PREFAB, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
                return name;

            return name.Remove(index, PREFAB.Length).Trim();
        }
        
        public static string RemoveItemNumberFromName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;
            
            name = Regex.Replace(name, @"\(\d\)", "");
            
            return name.Trim();
        }

        public static string CleanName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            // Apply each cleanup step in order
            name = RemovePotionPrefix(name);
            name = RemovePrefabFromName(name);
            name = RemoveItemNumberFromName(name);

            return name.Trim();
        }

    }
}