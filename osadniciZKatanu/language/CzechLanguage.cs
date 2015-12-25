using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class CzechLanguage : ILanguage
    {
        public string ResultOfRollTheDiceToString() { return "Výsledek hodu kostkami: "; }
        public string ColorToString(GameDesc.color playerColor)
        {
            return ColorToString(playerColor, 1);
        }
        public string ColorToString(GameDesc.color playerColor, int declension)
        {
            string result = "";
            switch (playerColor)
            {
                case GameDesc.color.blue: result = BlueWithDeclension(declension);
                    break;
                case GameDesc.color.red: result = RedWithDeclension(declension);
                    break;
                case GameDesc.color.white: result = WhiteWithDeclension(declension);
                    break;
                case GameDesc.color.yellow: result = YellowWithDeclension(declension);
                    break;
            }
            return result;
        }
        private string BlueWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "modrý"; break;
                case 2: result = "modrého"; break;
                case 3: result = "modrému"; break;
                case 4: result = "modrého"; break;
                case 5: result = "modrý"; break;
                case 6: result = "modrém"; break;
                case 7: result = "modrým"; break;
                default: result = "modrý"; break;
            }
            return result;
        }
        private string RedWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "červený"; break;
                case 2: result = "červeného"; break;
                case 3: result = "červenému"; break;
                case 4: result = "červeného"; break;
                case 5: result = "červený"; break;
                case 6: result = "červeném"; break;
                case 7: result = "červeným"; break;
                default: result = "červený"; break;
            }
            return result;
        }
        private string WhiteWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "bílý"; break;
                case 2: result = "bílého"; break;
                case 3: result = "bílému"; break;
                case 4: result = "bílého"; break;
                case 5: result = "bílý"; break;
                case 6: result = "bílém"; break;
                case 7: result = "bílým"; break;
                default: result = "bílý"; break;
            }
            return result;
        }
        private string YellowWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "žlutý"; break;
                case 2: result = "žlutého"; break;
                case 3: result = "žlutému"; break;
                case 4: result = "žlutého"; break;
                case 5: result = "žlutý"; break;
                case 6: result = "žlutém"; break;
                case 7: result = "žlutým"; break;
                default: result = "žlutý"; break;
            }
            return result;
        }

        public string MaterialToString(Game.materials material)
        {
            return MaterialToString(material, 1);
        }

        public string MaterialToString(Game.materials material, int declension)
        {
            string result = "";
            switch (material)
            {
                case Game.materials.brick:
                    result = BrickWithDeclension(declension);
                    break;
                case Game.materials.desert:
                    result = DesertWithDeclension(declension);
                    break;
                case Game.materials.grain:
                    result = GrainWithDeclension(declension);
                    break;
                case Game.materials.sheep:
                    result = SheepWithDeclension(declension);
                    break;
                case Game.materials.stone:
                    result = StoneWithDeclension(declension);
                    break;
                case Game.materials.wood:
                    result = WoodWithDeclension(declension);
                    break;
            }
            return result;
        }

        private string BrickWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "cihla"; break;
                case 2: result = "cihly"; break;
                case 3: result = "cihle"; break;
                case 4: result = "cihlu"; break;
                case 5: result = "cihlo"; break;
                case 6: result = "cihle"; break;
                case 7: result = "cihlou"; break;
                default: result = "cihla"; break;
            }
            return result;
        }
        private string DesertWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "poušť"; break;
                case 2: result = "poušťě"; break;
                case 3: result = "poušťi"; break;
                case 4: result = "poušť"; break;
                case 5: result = "poušti"; break;
                case 6: result = "poušti"; break;
                case 7: result = "pouští"; break;
                default: result = "poušť"; break;
            }
            return result;
        }
        private string GrainWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 7: result = "obilím"; break;
                default: result = "obilí"; break;
            }
            return result;
        }
        private string SheepWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "vlna"; break;
                case 2: result = "vlny"; break;
                case 3: result = "vlně"; break;
                case 4: result = "vlnu"; break;
                case 5: result = "vlno"; break;
                case 6: result = "vlně"; break;
                case 7: result = "vlnou"; break;
                default: result = "vlna"; break;
            }
            return result;
        }
        private string StoneWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "kámen"; break;
                case 2: result = "kámenu"; break;
                case 3: result = "kámenu"; break;
                case 4: result = "kámen"; break;
                case 5: result = "kámene"; break;
                case 6: result = "kámenu"; break;
                case 7: result = "kámenem"; break;
                default: result = "kámen"; break;
            }
            return result;
        }
        private string WoodWithDeclension(int dec)
        {
            string result;
            switch (dec)
            {
                case 1: result = "dřevo"; break;
                case 2: result = "dřeva"; break;
                case 3: result = "dřevu"; break;
                case 4: result = "dřevo"; break;
                case 5: result = "dřevo"; break;
                case 6: result = "dřevu"; break;
                case 7: result = "dřevem"; break;
                default: result = "dřevo"; break;
            }
            return result;
        }

        public string NameOfActionCard(Game.actionCards actionCard)
        {
            string result = "";
            switch (actionCard)
            {
                case Game.actionCards.coupon:
                    result = "jeden bod";
                    break;
                case Game.actionCards.knight:
                    result = "rytíř";
                    break;
                case Game.actionCards.materialsFromPlayers:
                    result = "jeden typ suroviny od všech hráčů";
                    break;
                case Game.actionCards.twoMaterials:
                    result = "dvě suroviny zadarmo";
                    break;
                case Game.actionCards.twoRoad:
                    result = "dvě cesty zadarmo";
                    break;
            }
            return result;
        }
        public string PlayerFirstAndSecondMoveToString(GameDesc.color playerColor)
        {
            return ColorToString(playerColor) + " hráč postaví vesnici a cestu";
        }
        public string PlayerMoveToString(GameDesc.color playerColor)
        {
            return ColorToString(playerColor) + " hráč je na tahu";
        }

        public string PlayerMoveInformation() { return "Kliknutím na hrací plochu můžete postavit "; }
        public string Road() { return "cestu"; }
        public string Village() { return "vesnici"; }
        public string Town() { return "město"; }

        public string Points() { return "Body: "; }
        public string Knights() { return "Rytíři: "; }
        public string StartGameLabel() { return "Začněte stisknutím tlačítka start "; }
        public string StartGameButton() { return "Start"; }
        public string NextPlayerButton() { return "Další"; }
        public string ChangeMaterialButton() { return "Vyměnit suroviny"; }
        public string CancelChangingMaterial() { return "Neměnit suroviny"; }
        public string BuyActionCardButton() { return "Koupit akční kartu"; }
        public string UseActionCardButton() { return "použít akční kartu"; }
        public string ActionCardLabel() { return "Akční karty"; }
        public string MaterialLabel() { return "Suroviny"; }
        public string MoveThief() { return "Umísti Zloděje"; }

        public string MaterialForRoad() { return "Cesta"; }
        public string MaterialForVillage() { return "Vesnice"; }
        public string MaterialForTown() { return "Město"; }
        public string MaterialForActionCard() { return "Akční karta"; }

        public string PickPlayer() { return "Vyberte hráče, od kterého chcete získat náhodnou surovinu"; }
        public string StealMaterial() { return "Ukradni surovinu"; }
        public string PickMaterialButton() { return "Vyberte Surovinu"; }
        public string PickMaterialLabel() { return "Vyberte surovinu, kterou chcete vyměnit"; }

        public string TwoFreeRoadLabel() { return "Kliknutím na hrací plochu umístěte dvě cesty zadarmo"; }
        public string MaterialFromPlayers() { return "Vyberte surovinu, kterou chcete získat od všech hráčů"; }
        public string TwoMaterialsFirstLabel() { return "Vyberte si první surovinu zadarmo"; }
        public string TwoMaterialsSecondLabel() { return "Vyberte si druhou surovinu zadarmo"; }

        public string EndOfGame() { return "konec hry, vyhrál "; }
        public string NextGame() { return "Další hra"; }

        public string win() { return "vyhrál"; }

        public string MoveDescFirstVillAndRoad(int villID, int roadID)
        {
            return "Stavím vesnici na pozici " + villID + " a cestu na pozici " + roadID;
        }

        public string MoveDescKnight(int fieldID)
        {
            return "Hraji rytíře. Přesouvám zloděje na pole " + fieldID;
        }
        public string MoveDescKnight(int fieldID, GameDesc.color plCol)
        {
            return "Hraji rytíře. Přesouvám zloděje na pole " + fieldID + ", okrádám " + ColorToString(plCol, 2);
        }
        public string MoveDescCoupon()
        {
            return "Hraji kupón";
        }
        public string MoveDescMatFromPl(GameDesc.materials mat)
        {
            return "Hraji kartu jeden materiál od hráčů. Vybírám " + MaterialToString(mat, 4);
        }
        public string MoveDescTwoMat(GameDesc.materials fsMat, GameDesc.materials scMat)
        {
            return "Hraji kartu dvě suroviny zadarmo. Vybírám " + MaterialToString(fsMat, 4) + " a " + MaterialToString(scMat, 4);
        }
        public string MoveDescTwoRoad(int fsID, int scID)
        {
            return "Hraji dvě cesty zadarmo. Stavím na cestě s ID " + fsID + " a " + scID;
        }
        public string MoveDescBuildRoad(int roadID)
        {
            return "Stavím cestu na pozici " + roadID;
        }
        public string MoveDescBuildVillage(int villageID)
        {
            return "Stavím vesnici na pozici " + villageID;
        }
        public string MoveDescBuildTown(int townID)
        {
            return "Stavím město na pozici " + townID;
        }
        public string MoveDescBuyActCard()
        {
            return "Kupuji Akční kartu";
        }
        public string MoveDescExchangeMat(GameDesc.materials matFrom, GameDesc.materials matTo)
        {
            return "Měním " + MaterialToString(matFrom, 4) + " za " + MaterialToString(matTo, 4);
        }
        public string MoveDescThief(int fieldID)
        {
            return "Přesouvám zloděje na pole " + fieldID;
        }
        public string MoveDescThief(int fieldID, GameDesc.color plCol)
        {
            return "Přesouvám zloděje na pole " + fieldID + ", okrádám " + ColorToString(plCol, 2);
        }

    }
}
