using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LuisBot
{
    public class Result
    {
        public string result { get; set; }
    }

    public class Text
    {
        public string index { get; set; }

        public string text { get; set; }

        public string score { get; set; }
    }


    public class FileResult
    {
        public string file { get; set; }
    }

    public class AudioRecognize
    {
        public ICollection<Text> alternatives { get; set; }
    }
}