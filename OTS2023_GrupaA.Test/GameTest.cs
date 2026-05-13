using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Framework;
using OTS2026_GrupaB.Exceptions;
using OTS2026_GrupaB.Models;
using OTS2026_GrupaB.Test;

namespace OTS2026_GrupaB
{
    [TestFixture]
    internal class GameTest
    {
        //F1
        [Test]
        public void GameSetup_Game_Success() {
            Position pPos = new Position(1, 2, 0);
            Position iPos = new Position(1, 24, 0);
            Game newGame = new Game(pPos, iPos);
            Assert.That(newGame.Player.Position.X, Is.EqualTo(0));
            Assert.That(newGame.Player.Position.Y, Is.EqualTo(0));
            Assert.That(newGame.Player.Position.Z, Is.EqualTo(0));
        }

        [TestCase(10, 5, 0, 0, 0, 0)]
        [TestCase(0, 0, 0, 10, 20, 0)]
        [TestCase(-1, 0, 0, 0, 0, 0)]
        [TestCase(0, 0, 0, 0, 0, 31)]
        public void GameSetup_Game_Failure(int px, int py, int pz, int ix, int iy, int iz)
        {
            Position pPos = new Position(0, 0, 0);
            Position iPos = new Position(0, 0, 0);
            Assert.That(() => new Game(pPos, iPos), Throws.TypeOf<PositionOutsideOfMapException>();
        }

        //F2
        //check movement in all directions
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        public void MovePlayer_movePlayerOnXYZ_Success(int direction)
        {
            Position pPos = new Position(1, 2, 1);
            Position iPos = new Position(1, 24, 0);
            Game newGame = new Game(pPos, iPos);
            if (direction == 1) {
                newGame.MovePlayer(Move.Up);
                Assert.That(newGame.Player.Position.Y, Is.EqualTo(1));
            }
            if (direction == 2)
            {
                newGame.MovePlayer(Move.Down);
                Assert.That(newGame.Player.Position.Y, Is.EqualTo(3));
            }
            if (direction == 3)
            {
                newGame.MovePlayer(Move.Left);
                Assert.That(newGame.Player.Position.X, Is.EqualTo(0));
            }
            if (direction == 4)
            {
                newGame.MovePlayer(Move.Right);
                Assert.That(newGame.Player.Position.X, Is.EqualTo(2));
            }
            if (direction == 5)
            {
                newGame.MovePlayer(Move.Forward);
                Assert.That(newGame.Player.Position.Z, Is.EqualTo(2));
            }
            if (direction == 6)
            {
                newGame.MovePlayer(Move.Back);
                Assert.That(newGame.Player.Position.Z, Is.EqualTo(0));
            }
        }

        //F3
        [TestCase(9, 5, 0, 1)]
        [TestCase(9, 5, 0, 2)]
        public void MovePlayer_MovePlayerOnDifferentTiles_Success(int x, int y, int z, int scenario)
        {
            Position pPos = new Position(x, y, z);
            Position iPos = new Position(9, 4, 0);
            Game newGame = new Game(pPos, iPos);
            if(scenario == 1)
            {
                newGame.MovePlayer(Move.Up);
                Assert.That(newGame.Player.Position.Y, Is.EqualTo(y - 1));
            }
            if (scenario == 2)
            {
                newGame.Map.AddTile(TileType.Standard, TileContent.Gold, 10, 5, 0);
                newGame.MovePlayer(Move.Right);
                Assert.That(newGame.Player.Position.X, Is.EqualTo(x + 1));
            }
        }

        //F4
        [Test]
        public void MovePlayer_CollectItems_Success()
        {
            Position pPos = new Position(1, 2, 0);
            Position iPos = new Position(1, 4, 0);
            Game newGame = new Game(pPos, iPos);
            newGame.Map.AddTile(TileType.Standard, TileContent.Gold, 1, 3, 0);
            newGame.Map.AddTile(TileType.Standard, TileContent.UncoverItem, 1, 5, 0);
            newGame.MovePlayer(Move.Down);
            newGame.MovePlayer(Move.Down);
            newGame.MovePlayer(Move.Down);
            Assert.That(newGame.Player.AmountOfGold, Is.EqualTo(1));
            
        }
        //F5
        /*  vKE:    gold        [0, +]
         *          hiddenGold  [0, +]
         *          UncoverItem [true, false]
         *          Grade       [good, average, bad]
         *  gold: 9, 10, 15, 16
         *  hiddenGold: 5, 6
         *  canUncover: true, false
         *  grade: good, bad, average
         */
        [TestCaseSource(typeof(Parser), "GetTestCasesData", new object[] { "test-results.txt" })]
        public void TestGameStates_Success(int gold, int hiddenGold, bool canUncover, Enum grade)
        {

            Position pPos = new Position(1, 2, 0);
            Position iPos = new Position(1, 4, 0);
            Game newGame = new Game(pPos, iPos);
            newGame.Player.AmountOfGold = gold;
            newGame.Player.AmountOfCoveredGold = hiddenGold;
            newGame.Player.CanUncover = canUncover;
            Assert.That(newGame.CalculateScore, Is.EqualTo(grade));
        }

    }
}
