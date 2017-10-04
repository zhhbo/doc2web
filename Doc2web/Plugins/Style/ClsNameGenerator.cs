using System;
using System.Collections.Generic;
using System.Text;

namespace Doc2web.Plugins.Style
{
    public class ClsNameGenerator
    {
        private static char[] _base62chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            .ToCharArray();

        private string _prefix;
        private Random _random;

        public ClsNameGenerator(StyleConfig config)
        {
            _prefix = config.DynamicCssClassPrefix;
            _random = new Random();
        }

        public string GenId()
        {
            char[] uid = new char[_prefix.Length + 7];

            _prefix.CopyTo(0, uid, 0, _prefix.Length);
            AddRandom(uid, _prefix.Length);

            return new string(uid);
        }

        private void AddRandom(char[] uid, int length)
        {
            for (int i = length; i < uid.Length; i++)
                uid[i] = _base62chars[_random.Next(62)];
        }
    }
}
