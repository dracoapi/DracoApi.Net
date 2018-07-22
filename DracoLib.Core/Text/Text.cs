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

        public string GetCreatureName(string obj)
        {
            return Load($"creature.{ obj }");
        }

        public string GetItemName(string obj)
        {
            return Load($"key.item.{ obj }");
        }

        public string GetString(string obj)
        {
            return Load(obj);
        }

    }
}
