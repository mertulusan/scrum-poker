using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ScrumPoker.Model.Enums
{
    public enum CardPoints
    {
        [Description("1")]
        One = 1,
        [Description("2")]
        Two = 2,
        [Description("3")]
        Three = 3,
        [Description("5")]
        Five = 5,
        [Description("8")]
        Eight = 8,
        [Description("13")]
        Thirteen = 13,
        [Description("21")]
        Twentyone = 21,
        [Description("34")]
        Thirtyfour = 34,
        [Description("Break")]
        CoffeeBreak = -1,
        [Description("I don't understand")]
        DontUnderstand = -2,
        [Description("Must be divided")]
        Devide = -3,
    }
}