﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osadniciZKatanu
{
    public interface ILanguage
    {
        string ResultOfRollTheDiceToString();
        string ColorToString(GameDesc.color playerColor);
        string MaterialToString(Game.materials material);
        string NameOfActionCard(Game.actionCards actionCard);
        string PlayerFirstAndSecondMoveToString(GameDesc.color playerColor);
        string PlayerMoveToString(GameDesc.color playerColor);
        string PlayerMoveInformation();
        string Road();
        string Village();
        string Town();

        string Points();
        string Knights();
        string StartGameLabel();
        string StartGameButton();
        string NextPlayerButton();
        string ChangeMaterialButton();
        string CancelChangingMaterial();
        string BuyActionCardButton();
        string UseActionCardButton();
        string ActionCardLabel();
        string MaterialLabel();
        string MoveThief();

        string MaterialForRoad();
        string MaterialForVillage();
        string MaterialForTown();
        string MaterialForActionCard();

        string PickPlayer();
        string StealMaterial();
        string PickMaterialButton();
        string PickMaterialLabel();

        string TwoFreeRoadLabel();
        string MaterialFromPlayers();
        string TwoMaterialsFirstLabel();
        string TwoMaterialsSecondLabel();

        string EndOfGame();
        string NextGame();

        string win();

        string MoveDescFirstVillAndRoad(int villID, int roadID);
        string MoveDescKnight(int fieldID);
        string MoveDescKnight(int fieldID, GameDesc.color plCol);
        string MoveDescCoupon();
        string MoveDescMatFromPl(GameDesc.materials mat);
        string MoveDescTwoMat(GameDesc.materials fsMat, GameDesc.materials scMat);
        string MoveDescTwoRoad(int fsID, int scID);
        string MoveDescBuildRoad(int roadID);
        string MoveDescBuildVillage(int villageID);
        string MoveDescBuildTown(int townID);
        string MoveDescBuyActCard();
        string MoveDescExchangeMat(GameDesc.materials matFrom, GameDesc.materials matTo);
        string MoveDescThief(int fieldID);
        string MoveDescThief(int fieldID, GameDesc.color plCol);

    }
}
