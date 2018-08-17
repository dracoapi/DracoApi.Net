using DracoProtos.Core.Base;
using System;

namespace DracoLib.Core.Text
{
    public class Strings
    {
        private readonly DracoClient dracoClient;

        internal Strings(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        private string Load(string obj)
        {
            try
            {
                var lang = dracoClient.ClientInfo.language;

                if (lang == Langues.Bulgarian.ToString())
                    return Bulgarian.Load[obj];
                else if (lang == Langues.English.ToString())
                    return English.Load[obj];
                else if (lang == Langues.Russian.ToString())
                    return Russian.Load[obj];
                else if (lang == Langues.Czech.ToString())
                    return Czech.Load[obj];
                else if (lang == Langues.German.ToString())
                    return German.Load[obj];
                else if (lang == Langues.Spanish.ToString())
                    return Spanish.Load[obj];
                else if (lang == Langues.French.ToString())
                    return French.Load[obj];
                else if (lang == Langues.Italian.ToString())
                    return Italian.Load[obj];
                else if (lang == Langues.Dutch.ToString())
                    return Dutch.Load[obj];
                else if (lang == Langues.Polish.ToString())
                    return Polish.Load[obj];
                else if (lang == Langues.SerboCroatian.ToString())
                    return SerboCroatian.Load[obj];
                else if (lang == Langues.Turkish.ToString())
                    return Turkish.Load[obj];
                else if (lang == Langues.Slovenian.ToString())
                    return Slovenian.Load[obj];
                else if (lang == Langues.Slovak.ToString())
                    return Slovak.Load[obj];
                else if (lang == Langues.Hungarian.ToString())
                    return Hungarian.Load[obj];
                else if (lang == Langues.Portuguese.ToString())
                    return Portuguese.Load[obj];
                else if (lang == Langues.Ukrainian.ToString())
                    return Ukrainian.Load[obj];
                else if (lang == Langues.Japanese.ToString())
                    return Japanese.Load[obj];
                else if (lang == Langues.Korean.ToString())
                    return Korean.Load[obj];

                return English.Load[obj];
            }
            catch (Exception)
            {
                return obj;
            }
        }

        [System.Obsolete("use GetCreatureName(CreatureType obj) instead")]
        public string GetCreatureName(string obj)
        {
            return Load($"creature.{ obj }");
        }

        public string GetCreatureName(CreatureType obj)
        {
            return Load($"creature.{ obj }");
        }

        [System.Obsolete("use GetItemName(ItemType obj) instead")]
        public string GetItemName(string obj)
        {
            return Load($"key.item.{ obj }");
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
