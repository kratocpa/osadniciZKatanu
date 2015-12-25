using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public class EngLanguage : ILanguage
    {
        public string ResultOfRollTheDiceToString() { return "Result of roll the dice: "; }
        public string ColorToString(GameDesc.color playerColor)
        {
            string result = "";
            switch (playerColor)
            {
                case GameDesc.color.blue: result = "blue";
                    break;
                case GameDesc.color.red: result = "red";
                    break;
                case GameDesc.color.white: result = "white";
                    break;
                case GameDesc.color.yellow: result = "yellow";
                    break;
            }
            return result;
        }
        public string MaterialToString(Game.materials material)
        {
            string result = "";
            switch (material)
            {
                case Game.materials.brick:
                    result = "brick";
                    break;
                case Game.materials.desert:
                    result = "desert";
                    break;
                case Game.materials.grain:
                    result = "grain";
                    break;
                case Game.materials.sheep:
                    result = "cotton";
                    break;
                case Game.materials.stone:
                    result = "rock";
                    break;
                case Game.materials.wood:
                    result = "wood";
                    break;
            }
            return result;
        }
        public string NameOfActionCard(Game.actionCards actionCard)
        {
            string result = "";
            switch (actionCard)
            {
                case Game.actionCards.coupon:
                    result = "one point";
                    break;
                case Game.actionCards.knight:
                    result = "knight";
                    break;
                case Game.actionCards.materialsFromPlayers:
                    result = "one type of material from other players";
                    break;
                case Game.actionCards.twoMaterials:
                    result = "two free materials";
                    break;
                case Game.actionCards.twoRoad:
                    result = "two free roads";
                    break;
            }
            return result;
        }
        public string PlayerFirstAndSecondMoveToString(GameDesc.color playerColor)
        {
            return ColorToString(playerColor) + " player build village and road";
        }
        public string PlayerMoveToString(GameDesc.color playerColor)
        {
            return ColorToString(playerColor) + " player is on the move";
        }

        public string PlayerMoveInformation() { return "You can build "; }
        public string Road() { return "road"; }
        public string Village() { return "village"; }
        public string Town() { return "town"; }

        public string Points() { return "Points: "; }
        public string Knights() { return "Knights: "; }
        public string StartGameLabel() { return "Press the start button "; }
        public string StartGameButton() { return "Start"; }
        public string NextPlayerButton() { return "Next player"; }
        public string ChangeMaterialButton() { return "Change materials"; }
        public string CancelChangingMaterial() { return "Cancel"; }
        public string BuyActionCardButton() { return "Buy action card"; }
        public string UseActionCardButton() { return "Use action card"; }
        public string ActionCardLabel() { return "Action cards"; }
        public string MaterialLabel() { return "Materials"; }
        public string MoveThief() { return "Remove the thief"; }

        public string MaterialForRoad() { return "Road"; }
        public string MaterialForVillage() { return "Village"; }
        public string MaterialForTown() { return "Town"; }
        public string MaterialForActionCard() { return "Action card"; }

        public string PickPlayer() { return "Pick player to robbed one random material"; }
        public string StealMaterial() { return "Steal material"; }
        public string PickMaterialButton() { return "Pick material"; }
        public string PickMaterialLabel() { return "Pick material to change"; }

        public string TwoFreeRoadLabel() { return "Build two free road"; }
        public string MaterialFromPlayers() { return "Select the material you want to get from all players"; }
        public string TwoMaterialsFirstLabel() { return "Pick first free material"; }
        public string TwoMaterialsSecondLabel() { return "Pick second free material"; }

        public string EndOfGame() { return "end of game, win "; }
        public string NextGame() { return "Next game"; }

        public string win() { return "won a"; }

        public string MoveDescFirstVillAndRoad(int villID, int roadID)
        {
            return "I have build village on vertex with ID " + villID + " and road on edge with ID " + roadID;
        }

        public string MoveDescKnight(int fieldID)
        {
            return "I am playing knight. I moved thief to field number " + fieldID;
        }
        public string MoveDescKnight(int fieldID, GameDesc.color plCol)
        {
            return "I am playing knight. I moved thief to field number " + fieldID + ", I stole " + ColorToString(plCol) + " player";
        }
        public string MoveDescCoupon()
        {
            return "I am playing coupon";
        }
        public string MoveDescMatFromPl(GameDesc.materials mat)
        {
            return "I am playing one material from players. I picked " + MaterialToString(mat);
        }
        public string MoveDescTwoMat(GameDesc.materials fsMat, GameDesc.materials scMat)
        {
            return "I am playing two free materials. I picked " + MaterialToString(fsMat) + " and " + MaterialToString(scMat);
        }
        public string MoveDescTwoRoad(int fsID, int scID)
        {
            return "I am playing two free roads. I build on edge with ID " + fsID + " and " + scID;
        }
        public string MoveDescBuildRoad(int roadID)
        {
            return "I am building road on edge with ID " + roadID;
        }
        public string MoveDescBuildVillage(int villageID)
        {
            return "I am building village on vertex with ID " + villageID;
        }
        public string MoveDescBuildTown(int townID)
        {
            return "I am building town on vertex with ID " + townID;
        }
        public string MoveDescBuyActCard()
        {
            return "I bought action card.";
        }
        public string MoveDescExchangeMat(GameDesc.materials matFrom, GameDesc.materials matTo)
        {
            return "I am changing " + MaterialToString(matFrom) + " to " + MaterialToString(matTo);
        }
        public string MoveDescThief(int fieldID)
        {
            return "I am moving thief to field with number " + fieldID;
        }
        public string MoveDescThief(int fieldID, GameDesc.color plCol)
        {
            return "I moved thief to field with number " + fieldID + ", I stole " + ColorToString(plCol);
        }
    }
}
