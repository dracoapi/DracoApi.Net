using DracoProtos.Core.Base;
using System;

namespace DracoLib.Core.Text
{
    public class Strings
    {
        private string language;

        public Strings(string language)
        {
            this.language = language;
        }

        private string Load(string obj)
        {
            try
            {
                if (language == Langues.Bulgarian.ToString())
                    return Bulgarian.Load[obj];
                else if (language == Langues.English.ToString())
                    return English.Load[obj];
                else if (language == Langues.Russian.ToString())
                    return Russian.Load[obj];
                else if (language == Langues.Czech.ToString())
                    return Czech.Load[obj];
                else if (language == Langues.German.ToString())
                    return German.Load[obj];
                else if (language == Langues.Spanish.ToString())
                    return Spanish.Load[obj];
                else if (language == Langues.French.ToString())
                    return French.Load[obj];
                else if (language == Langues.Italian.ToString())
                    return Italian.Load[obj];
                else if (language == Langues.Dutch.ToString())
                    return Dutch.Load[obj];
                else if (language == Langues.Polish.ToString())
                    return Polish.Load[obj];
                else if (language == Langues.SerboCroatian.ToString())
                    return SerboCroatian.Load[obj];
                else if (language == Langues.Turkish.ToString())
                    return Turkish.Load[obj];
                else if (language == Langues.Slovenian.ToString())
                    return Slovenian.Load[obj];
                else if (language == Langues.Slovak.ToString())
                    return Slovak.Load[obj];
                else if (language == Langues.Hungarian.ToString())
                    return Hungarian.Load[obj];
                else if (language == Langues.Portuguese.ToString())
                    return Portuguese.Load[obj];
                else if (language == Langues.Ukrainian.ToString())
                    return Ukrainian.Load[obj];
                else if (language == Langues.Japanese.ToString())
                    return Japanese.Load[obj];
                else if (language == Langues.Korean.ToString())
                    return Korean.Load[obj];

                return English.Load[obj];
            }
            catch (Exception)
            {
                return obj;
            }
        }

        public string GetCreatureName(CreatureType obj)
        {
            return Load($"creature.{ obj }");
        }

        public string GetCandyFamilyName(CreatureType obj)
        {
            return Load($"key.candy.{ obj }");
        }

        public string GetItemName(ItemType obj)
        {
            return Load($"key.item.{ obj }");
        }

        public string GetString(string obj)
        {
            return Load(obj);
        }
    }
}
