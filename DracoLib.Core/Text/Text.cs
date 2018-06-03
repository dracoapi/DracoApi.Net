using DracoLib.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoLib.Core.Text
{
    public class Strings
    {
        private readonly DracoClient dracoClient;

        internal Strings(DracoClient dracoClient)
        {
            this.dracoClient = dracoClient;
        }

        public string Load(string obj)
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
    }
}
