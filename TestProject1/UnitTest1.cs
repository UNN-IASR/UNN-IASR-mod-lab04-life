using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using cli_life;
using System.IO;
using System.Text.Json;

namespace NET
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        // ���� ���������, ��� ������ � ����� ������ �������� �������
        public void TestCellRevivesWithThreeNeighbors()
        {
            Cell cell = new Cell();
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });

            cell.DetermineNextLiveState();
            Assert.IsTrue(cell.IsAliveNext);
        }

        [TestMethod]
        // ���� ���������, ��� ������ � ����� ������ �������� �������� �����
        public void TestCellStaysAliveWithTwoNeighbors()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });

            cell.DetermineNextLiveState();
            Assert.IsTrue(cell.IsAliveNext);
        }

        [TestMethod]
        // ���� ���������, ��� ������ � ����� ����� �������� ������� �� �����������
        public void TestCellDiesWithOneNeighbor()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });

            cell.DetermineNextLiveState();
            Assert.IsFalse(cell.IsAliveNext);
        }

        [TestMethod]
        // ���� ���������, ��� ������ � �������� ������ �������� ������� �� �������������
        public void TestCellDiesWithFourNeighbors()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });

            cell.DetermineNextLiveState();
            Assert.IsFalse(cell.IsAliveNext);
        }

        [TestMethod]
        // ���� ��������� ���������� ���������� ��������� ������
        public void TestCellAdvanceState()
        {
            Cell cell = new Cell { IsAliveNext = true };
            cell.Advance();
            Assert.IsTrue(cell.IsAlive);
        }

        [TestMethod]
        // ���� ��������� ������������� ����� � �������� ���������� ����� ������
        public void TestBoardInitialization()
        {
            Board board = new Board(10, 10, 1, 0.5);
            int liveCells = board.CountAliveCells();
            Assert.IsTrue(liveCells > 0 && liveCells < 100);
        }

        [TestMethod]
        // ���� ��������� ���������� ��������� �����
        public void TestSaveBoardState()
        {
            Board board = new Board(10, 10, 1, 0.5);
            string filePath = "test_state.json";
            board.SaveState(filePath);
            Assert.IsTrue(File.Exists(filePath));
        }

        [TestMethod]
        // ���� ��������� �������� ��������� �����
        public void TestLoadBoardState()
        {
            string filePath = "test_state.json";
            Board board = new Board(10, 10, 1, 0.5);
            board.SaveState(filePath);

            Board newBoard = new Board(10, 10, 1, 0.0);
            newBoard.LoadState(filePath);
            Assert.AreEqual(board.CountAliveCells(), newBoard.CountAliveCells());
        }

        [TestMethod]
        // ���� ��������� ������������� ��������� �����
        public void TestBoardClassification()
        {
            Board board = new Board(10, 10, 1, 0.5);
            var classifications = board.ClassifyElements();
            Assert.IsTrue(classifications.Count > 0);
        }

        [TestMethod]
        // ���� ��������� ����� �������� ����� ������
        public void TestCountAliveCells()
        {
            Board board = new Board(10, 10, 1, 0.5);
            int liveCells = board.CountAliveCells();
            Assert.IsTrue(liveCells > 0 && liveCells < 100);
        }

        [TestMethod]
        // ���� ��������� ����� �������� ��������
        public void TestLoadPattern()
        {
            Board board = new Board(10, 10, 1, 0.0);
            string pattern = "1\n0\n1\n0\n1\n0\n1\n0\n1\n0\n";
            File.WriteAllText("test_pattern.txt", pattern);
            board.LoadPattern("test_pattern.txt");
            Assert.AreEqual(5, board.CountAliveCells());
        }

        [TestMethod]
        // ���� ��������� �������� ����������� ���������
        public void TestStandardPatterns()
        {
            var patterns = Pattern.GetStandardPatterns();
            Assert.IsTrue(patterns.Count > 0);
        }

        [TestMethod]
        // ���� ��������� ���������� ����������� ������� ������
        public void TestCellNeighbors()
        {
            Board board = new Board(10, 10, 1, 0.5);
            Cell cell = board.Cells[1, 1];
            Assert.AreEqual(8, cell.neighbors.Count);
        }

        [TestMethod]
        // ���� ���������, ��� ������ ������� ������ ��� ���������� ����� �������
        public void TestCellStaysDead()
        {
            Cell cell = new Cell();
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });

            cell.DetermineNextLiveState();
            Assert.IsFalse(cell.IsAliveNext);
        }

        [TestMethod]
        // ���� ���������, ��� ������ ������� ����� ��� ��� ����� �������
        public void TestCellStaysAliveWithThreeNeighbors()
        {
            Cell cell = new Cell { IsAlive = true };
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = true });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });
            cell.neighbors.Add(new Cell { IsAlive = false });

            cell.DetermineNextLiveState();
            Assert.IsTrue(cell.IsAliveNext);
        }

        [TestMethod]
        // ���� ���������, ��� ����� ��������� ������������� ��������� ������
        public void TestBoardRandomization()
        {
            Board board = new Board(10, 10, 1, 0.5);
            board.Randomize(0.7);
            int liveCells = board.CountAliveCells();
            Assert.IsTrue(liveCells > 50 && liveCells < 100);
        }

        [TestMethod]
        // ���� ��������� ���������� ���������� � �������� ������������
        public void TestSaveLoadConfig()
        {
            Config config = new Config { Width = 50, Height = 50, CellSize = 1, LiveDensity = 0.5 };
            string filePath = "test_config.json";
            File.WriteAllText(filePath, JsonSerializer.Serialize(config));
            Config loadedConfig = Config.Load(filePath);
            Assert.AreEqual(config.Width, loadedConfig.Width);
            Assert.AreEqual(config.Height, loadedConfig.Height);
            Assert.AreEqual(config.CellSize, loadedConfig.CellSize);
            Assert.AreEqual(config.LiveDensity, loadedConfig.LiveDensity);
        }

        [TestMethod]
        // ���� ���������, ��� ������� ��������� �������������� �������� ����� ���������� ���������
        public void TestClassificationAfterGenerations()
        {
            Board board = new Board(10, 10, 1, 0.5);
            for (int i = 0; i < 10; i++)
            {
                board.Advance();
            }
            var classifications = board.ClassifyElements();
            Assert.IsTrue(classifications.Count > 0);
        }
    }
}
