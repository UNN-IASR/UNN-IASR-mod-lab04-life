using Xunit;
using cli_life;
using System.IO;
using System;
using System.Linq;

namespace Life.Tests
{
    public class GameOfLifeTests
    {
        // ���� 1: ��������, ��� ������ � ����� ������ �������� �������� �����
        [Fact]
        public void Cell_DetermineNextLiveState_AliveWithTwoNeighbors_StaysAlive()
        {
            Cell cell = new Cell { IsAlive = true };
            AddNeighbors(cell, 2, true);
            AddNeighbors(cell, 6, false);
            cell.DetermineNextLiveState();
            Assert.True(cell.IsAlive);
        }

        // ���� 2: ��������, ��� ������� ������ � ����� ������ �������� �������
        [Fact]
        public void Cell_DetermineNextLiveState_DeadWithThreeNeighbors_BecomesAlive()
        {
            Cell cell = new Cell { IsAlive = false };
            AddNeighbors(cell, 3, true);
            AddNeighbors(cell, 5, false);
            cell.DetermineNextLiveState();
            Assert.True(cell.IsAlive);
        }

        // ���� 3: ��������, ��� ����� ������ � ����� ������� �������
        [Fact]
        public void Cell_DetermineNextLiveState_AliveWithOneNeighbor_Dies()
        {
            Cell cell = new Cell { IsAlive = true };
            AddNeighbors(cell, 1, true);
            AddNeighbors(cell, 7, false);
            cell.DetermineNextLiveState();
            Assert.False(cell.IsAlive);
        }

        // ���� 4: �������� ���������� ������������� ���������� �������� � Board
        [Fact]
        public void Board_Initialization_InitializesCorrectColumns()
        {
            int width = 10;
            int height = 10;
            int cellSize = 1;
            double liveDensity = 0.5;
            int expectedColumns = width / cellSize;
            var board = new Board(width, height, cellSize, liveDensity);
            Assert.Equal(expectedColumns, board.Columns);
        }

        // ���� 5: �������� ���������� ������������� ���������� ����� � Board
        [Fact]
        public void Board_Initialization_InitializesCorrectRows()
        {
            int width = 10;
            int height = 10;
            int cellSize = 1;
            double liveDensity = 0.5;
            int expectedRows = height / cellSize;
            var board = new Board(width, height, cellSize, liveDensity);
            Assert.Equal(expectedRows, board.Rows);
        }

        // ���� 6: �������� ����������� ���������� ������ ���� � Board.Advance()
        [Fact]
        public void Board_Advance_AppliesRulesCorrectly()
        {
            Board board = new Board(width: 3, height: 3, cellSize: 1, liveDensity: 0);
            board.Cells[1, 1].IsAlive = true;
            board.Cells[1, 2].IsAlive = true;
            board.Cells[2, 1].IsAlive = true;

            board.Advance();

            Assert.True(board.Cells[1, 1].IsAlive);
            Assert.True(board.Cells[2, 2].IsAlive);
            Assert.False(board.Cells[1, 2].IsAlive);
        }

        // ���� 7: ��������, ��� SaveState ������� ����
        [Fact]
        public void Program_SaveState_CreatesFile()
        {
            string filename = "test.txt";
            Program.board = new Board(width: 5, height: 5, cellSize: 1, liveDensity: 0);
            Program.SaveState(filename);
            Assert.True(File.Exists(filename));
            File.Delete(filename);
        }

        // ���� 8: ��������, ��� LoadState ��������� ���������� ���������
        [Fact]
        public void Program_LoadState_LoadsCorrectState()
        {
            string filename = "test.txt";
            string fileContent = ".....\n.*...\n..*..\n.***.\n.....\n";
            File.WriteAllText(filename, fileContent);
            Program.board = new Board(width: 5, height: 5, cellSize: 1, liveDensity: 0);
            Program.LoadState(filename);
            Assert.True(Program.board.Cells[1, 1].IsAlive);
            Assert.True(Program.board.Cells[2, 2].IsAlive);
            Assert.True(Program.board.Cells[1, 3].IsAlive);
            Assert.True(Program.board.Cells[2, 3].IsAlive);
            Assert.True(Program.board.Cells[3, 3].IsAlive);
            File.Delete(filename);
        }

        // ���� 9: ��������, ��� CheckStability ���������� ���������� ���������
        [Fact]
        public void Board_CheckStability_DetectsStableState()
        {
            Board board = new Board(width: 3, height: 3, cellSize: 1, liveDensity: 0);
            board.Cells[1, 1].IsAlive = true;
            board.CheckStability();
            board.Advance();
            board.CheckStability();
            Assert.True(board.IsStable);
        }

        // ���� 10: ��������, ��� ������� ComparePattern �������� ���������
        [Fact]
        public void Program_ComparePattern_ReturnsTrueForMatchingPattern()
        {
            Program.board = new Board(width: 5, height: 5, cellSize: 1, liveDensity: 0);
            Program.board.Cells[1, 1].IsAlive = true;
            Program.board.Cells[1, 2].IsAlive = true;
            Program.board.Cells[2, 1].IsAlive = true;
            Program.board.Cells[2, 2].IsAlive = true;

            bool[,] blockPattern = new bool[,] { { true, true }, { true, true } };
            bool isMatch = Program.ComparePattern(1, 1, blockPattern);

            Assert.True(isMatch);
        }

        // ��������������� ������� ��� ���������� ������� � ������
        private void AddNeighbors(Cell cell, int count, bool isAlive)
        {
            for (int i = 0; i < count; i++)
            {
                cell.neighbors.Add(new Cell { IsAlive = isAlive });
            }
        }
    }
}