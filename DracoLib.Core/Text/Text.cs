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
                var lan = new Langues();
                switch (lan)
                {
                    case Langues.Bulgarian:
                        return Bulgarian.Load[obj];
                    case Langues.English:
                        return English.Load[obj];
                    case Langues.Russian:
                        return Russian.Load[obj];
                    case Langues.Czech:
                        return Czech.Load[obj];
                    case Langues.German:
                        return German.Load[obj];
                    case Langues.Spanish:
                        return Spanish.Load[obj];
                    case Langues.French:
                        return French.Load[obj];
                    case Langues.Italian:
                        return Italian.Load[obj];
                    case Langues.Dutch:
                        return Dutch.Load[obj];
                    case Langues.Polish:
                        return Polish.Load[obj];
                    case Langues.Serbo:
                        return Serbo.Load[obj];
                    case Langues.Turkish:
                        return Turkish.Load[obj];
                    case Langues.Slovenian:
                        return Slovenian.Load[obj];
                    case Langues.Slovak:
                        return Slovak.Load[obj];
                    case Langues.Hungarian:
                        return Hungarian.Load[obj];
                    case Langues.Portuguese:
                        return Portuguese.Load[obj];
                    case Langues.Ukrainian:
                        return Ukrainian.Load[obj];
                    case Langues.Japanese:
                        return Japanese.Load[obj];
                    case Langues.Korean:
                        return Korean.Load[obj];
                    default:
                        return English.Load[obj];
                }
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
