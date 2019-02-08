using DracoProtos.Core.Base;
using DracoProtos.Core.Objects;

namespace DracoLib.Core
{
    public class Magic : MagicService
    {
        private readonly DracoClient client;

        public Magic(DracoClient dracoClient)
        {
            this.client = dracoClient;
        }

        public new FUpdate CastSpell(FBuildingRequest request)
        {
            return client.Call(client.clientMagic.CastSpell(request));
        }

        public new FAltarDetails ChangeAltarSpell(FBuildingRequest request, RecipeType recipeName)
        {
            return client.Call(client.clientMagic.ChangeAltarSpell(request, recipeName));
        }

        public new FBagUpdate ConvertRunes(ItemType type)
        {
            return client.Call(client.clientMagic.ConvertRunes(type));
        }

        public new FUpdate GetAltarDetailsV2(FBuildingRequest request, string passwordHash)
        {
            return client.Call(client.clientMagic.GetAltarDetailsV2(request, passwordHash));
        }

        public new FUpdate PutAltar(GeoCoords avatarCoords, RecipeType recipeName, bool shared, string passwordHash)
        {
            return client.Call(client.clientMagic.PutAltar(avatarCoords, recipeName, shared, passwordHash));
        }

        public new FUpdate PutRune(FBuildingRequest request, string passwordHash, int slot)
        {
            return client.Call(client.clientMagic.PutRune(request, passwordHash, slot));
        }

        public new FUpdate RemoveAltar(GeoCoords avatarCoords)
        {
            return client.Call(client.clientMagic.RemoveAltar(avatarCoords));
        }

        public new FUpdate TakeRune(FBuildingRequest request, string passwordHash, int slot)
        {
            return client.Call(client.clientMagic.TakeRune(request, passwordHash, slot));
        }
    }
}
