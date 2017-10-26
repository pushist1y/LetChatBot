using System;
using System.Collections.Generic;

namespace LetChatBot.Model
{
    public partial class PhpbbCaptchaQuestions
    {
        public int QuestionId { get; set; }
        public sbyte Strict { get; set; }
        public int LangId { get; set; }
        public string LangIso { get; set; }
        public string QuestionText { get; set; }
    }
}
