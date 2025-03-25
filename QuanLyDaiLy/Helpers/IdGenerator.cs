namespace QuanLyDaiLy.Helpers
{
    public static class IdGenerator
    {
        public static string GenerateId<T>()
        {
            DateTime now = DateTime.Now;
            string typeName = typeof(T).Name;
            string prefix = GetTypePrefixFromName(typeName);
            string id = $"{prefix}{now:HHmmdMMyyyy}{now:ss}";

            return id;
        }

        private static string GetTypePrefixFromName(string typeName)
        {
            List<char> prefixChars = new List<char>();
            if (typeName.Length > 0)
                prefixChars.Add(char.ToUpper(typeName[0]));

            for (int i = 1; i < typeName.Length; i++)
            {
                if (char.IsUpper(typeName[i]))
                    prefixChars.Add(typeName[i]);
            }

            if (prefixChars.Count <= 1 && typeName.Length >= 3)
                return typeName.Substring(0, 3).ToUpper();

            return new string(prefixChars.ToArray());
        }
    }
}
