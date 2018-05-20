using DracoProtos.Core.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DracoProtos.Core.Base
{
    public abstract class FNewsArticleBase : FObject
    {
        public HashSet<string> activeNewsIds;
        public int activeOfferCurrent;
        public int activeOfferTotal;
        public string body;
        public long freshNewsDate;
        public string id;
        public string title;

        public void ReadExternal(FInputStream stream)
        {
            this.activeNewsIds = stream.ReadStaticHashSet<string>(true);
            this.activeOfferCurrent = stream.ReadInt32();
            this.activeOfferTotal = stream.ReadInt32();
            this.body = stream.ReadUtfString();
            this.freshNewsDate = stream.ReadInt64();
            this.id = stream.ReadUtfString();
            this.title = stream.ReadUtfString();
        }

        public void WriteExternal(FOutputStream stream)
        {
            stream.WriteDynamicObject(this.activeNewsIds);
            stream.WriteInt32(this.activeOfferCurrent);
            stream.WriteInt32(this.activeOfferTotal);
            stream.WriteUtfString(this.body);
            stream.WriteInt64(this.freshNewsDate);
            stream.WriteUtfString(this.id);
            stream.WriteUtfString(this.title);
        }
     }
}
