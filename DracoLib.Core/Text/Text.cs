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
                var lang = dracoClient.ClientInfo.language.ToUpper();
                if (lang == Langues.ENGLISH.ToString())
                {
                    return English.Load[obj];
                }
                else if (lang == Langues.FRENCH.ToString())
                {
                    return French.Load[obj];
                }
                else if (lang == Langues.GERMAN.ToString())
                {
                    return German.Load[obj];
                }
                else if (lang == Langues.SPANISH.ToString())
                {
                    return Spanish.Load[obj];
                }

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
