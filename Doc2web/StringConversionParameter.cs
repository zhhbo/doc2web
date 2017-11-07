using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Doc2web
{
    public class StringConversionParameter : ConversionParameter
    {
        private MemoryStream _stream;

        public override Stream Stream
        {
            get => _stream;
            set => throw new InvalidOperationException("Cannot set the stream of a StringConversionParameter");
        }

        public override bool AutoFlush
        {
            get => false;
            set => throw new InvalidOperationException("Cannot set the Autoflush of a StringConversionParameter");
        }

        public StringConversionParameter()
        {
            _stream = new MemoryStream();
        }

        public string GetResult()
        {
            _stream.Position = 0;
            using (var sr = new StreamReader(Stream))
            {
                return sr.ReadToEnd();
            }
        }
    }

}
