using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrecept.Core.Utilities;

public static class HungarianNumberConverter
{
    private static readonly string[] Nums0To19 =
    {
        "nulla", "egy", "kettő", "három", "négy", "öt", "hat", "hét", "nyolc",
        "kilenc", "tíz", "tizenegy", "tizenkettő", "tizenhárom", "tizennégy",
        "tizenöt", "tizenhat", "tizenhét", "tizennyolc", "tizenkilenc"
    };

    private static readonly string[] Tizes =
    {
        string.Empty, string.Empty, "huszon", "harminc", "negyven", "ötven",
        "hatvan", "hetven", "nyolcvan", "kilencven"
    };

    private static readonly (int Value, string Name)[] Magnitudes =
    {
        (1_000_000_000, "milliárd"),
        (1_000_000, "millió"),
        (1_000, "ezer")
    };

    private static readonly Dictionary<int, string> DecimalNames = new()
    {
        [1] = "tized",
        [2] = "század",
        [3] = "ezred"
    };

    private static string Convert1To999(int n)
    {
        if (n < 0 || n > 999) throw new ArgumentOutOfRangeException(nameof(n));
        if (n < 20) return Nums0To19[n];
        if (n < 100)
        {
            int t = n / 10;
            int r = n % 10;
            return Tizes[t] + (r != 0 ? Nums0To19[r] : string.Empty);
        }

        int s = n / 100;
        int rest = n % 100;
        var text = (s == 1 ? string.Empty : Nums0To19[s]) + "száz";
        if (rest != 0) text += Convert1To999(rest);
        return text;
    }

    private static string IntegerToHungarian(long n)
    {
        if (n == 0) return "nulla";
        var parts = new List<string>();
        if (n < 0)
        {
            parts.Add("mínusz");
            n = Math.Abs(n);
        }

        bool addHyphen = false;
        foreach (var (value, name) in Magnitudes)
        {
            var count = n / value;
            if (count > 0)
            {
                if (count == 1)
                {
                    parts.Add(name switch
                    {
                        "millió" => "egymillió",
                        "milliárd" => "egymilliárd",
                        "ezer" => "ezer",
                        _ => $"egy {name}"
                    });
                }
                else
                {
                    parts.Add($"{Convert1To999((int)count)}{name}");
                }
                n %= value;
                addHyphen = name == "ezer" && count >= 2;
            }
        }

        if (n > 0)
        {
            if (addHyphen) parts.Add("-");
            parts.Add(Convert1To999((int)n));
        }

        return string.Concat(parts);
    }

    private static string ConvertFraction(int n, int decimals)
    {
        if (n == 0) return string.Empty;
        string mertek = DecimalNames.ContainsKey(decimals)
            ? DecimalNames[decimals]
            : decimals switch
            {
                4 => "tízezred",
                5 => "százezred",
                6 => "millioezred",
                _ => $"10^-{decimals}"
            };
        var fractionText = Convert1To999(n);
        return $"{fractionText} {mertek}";
    }

    public static string ToText(decimal value, int decimals = 2)
    {
        if (decimals < 0) throw new ArgumentOutOfRangeException(nameof(decimals));
        bool negative = value < 0;
        if (negative) value = Math.Abs(value);
        long whole = (long)Math.Floor(value);
        decimal fractionPart = value - whole;
        int multiplier = (int)Math.Pow(10, decimals);
        int fraction = (int)Math.Round(fractionPart * multiplier);
        if (fraction >= multiplier)
        {
            whole += 1;
            fraction = 0;
        }

        var parts = new List<string>();
        if (negative) parts.Add("mínusz");
        parts.Add(IntegerToHungarian(whole));
        if (fraction > 0)
        {
            parts.Add("egész");
            parts.Add(ConvertFraction(fraction, decimals));
        }
        return string.Join(" ", parts);
    }
}
