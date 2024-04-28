using cli_life;

namespace ProgramTests;

[TestClass]
public class ProgramTests
{
    /// <summary>
    /// ������ ������� ��� ����� ������� ������� ���.
    /// </summary>
    [TestMethod]
    public void DetermineNextLiveState_AliveCellMoreThanThreeNeighbours_CellDies()
    {
        Cell neighbour;
        var cell = new Cell();
        cell.IsAlive = true;

        for (var index = 0; index < 4; index++)
        {
            neighbour = new Cell();
            neighbour.IsAlive = true;

            cell.neighbours.Add(neighbour);
        }

        Assert.AreEqual(cell.DetermineNextLiveState(), false);
    }

    /// <summary>
    /// ������ �������� ����� ��� ����� ������� ������ ���� ��� ���.
    /// </summary>
    [TestMethod]
    public void DetermineNextLiveState_AliveCellThreeNeighbours_CellLives()
    {
        Cell neighbour;
        var cell = new Cell();
        cell.IsAlive = true;

        for (var index = 0; index < 3; index++)
        {
            neighbour = new Cell();
            neighbour.IsAlive = true;

            cell.neighbours.Add(neighbour);
        }

        Assert.AreEqual(cell.DetermineNextLiveState(), true);
    }

    /// <summary>
    /// ��� ����� ������� ������ ��� ���������� ����� ������.
    /// </summary>
    [TestMethod]
    public void DetermineNextLiveState_TwoOrThreeNeighbours_NewAliveCell()
    {
        Cell neighbour;
        var cell = new Cell();
        cell.IsAlive = false;

        for (var index = 0; index < 3; index++)
        {
            neighbour = new Cell();
            neighbour.IsAlive = true;

            cell.neighbours.Add(neighbour);
        }

        Assert.AreEqual(cell.DetermineNextLiveState(), true);
    }

    /// <summary>
    /// ��� ����� ������� ������� ��� ����� ������ �� ����������.
    /// </summary>
    [TestMethod]
    public void DetermineNextLiveState_LessThanThreeNeighbours_NoNewCell()
    {
        Cell neighbour;
        var cell = new Cell();
        cell.IsAlive = false;

        for (var index = 0; index < 2; index++)
        {
            neighbour = new Cell();
            neighbour.IsAlive = true;

            cell.neighbours.Add(neighbour);
        }

        Assert.AreEqual(cell.DetermineNextLiveState(), false);
    }

    /// <summary>
    /// ��� ��������� ��������� ����� ������ "����" ������ ������� �����.
    /// </summary>
    [TestMethod]
    public void Advance_IsAliveCellIsAliveNext_CellIsAlive()
    {
        Cell neighbour;
        var cell = new Cell();
        cell.IsAlive = true;

        for (var index = 0; index < 3; index++)
        {
            neighbour = new Cell();
            neighbour.IsAlive = true;

            cell.neighbours.Add(neighbour);
        }

        cell.DetermineNextLiveState();
        cell.Advance();

        Assert.AreEqual(cell.IsAlive, true);
    }

    /// <summary>
    /// ��� ��������� ��������� ����� ������ "������" ������ �������.
    /// </summary>
    [TestMethod]
    public void Advance_IsAliveCellIsNotAliveNext_CellIsNotAlive()
    {
        Cell neighbour;
        var cell = new Cell();
        cell.IsAlive = true;

        for (var index = 0; index < 4; index++)
        {
            neighbour = new Cell();
            neighbour.IsAlive = true;

            cell.neighbours.Add(neighbour);
        }

        cell.DetermineNextLiveState();
        cell.Advance();

        Assert.AreEqual(cell.IsAlive, false);
    }

    /// <summary>
    /// ��� �������� ��������� ������� ������ ������ ������� 
    /// ��������� ������������� ����� ����� � �������� ������� ������.
    /// </summary>
    [TestMethod]
    public void BoardConstructor_CellSizeGreaterThanOne_CorrectNumberRowsColumns()
    {
        var width = 50;
        var heigth = 20;
        var cellSize = 2;
        var board = new Board(width, heigth, cellSize);

        Assert.AreEqual(board.Columns, width / cellSize);
        Assert.AreEqual(board.Rows, heigth / cellSize);
    }

    /// <summary>
    /// ��������� ������ ������ � � ��������, ����� ������� ����� ������.
    /// </summary>
    [TestMethod]
    public void ConnectNeighbours_Board_ConnectsCellNeighbours()
    {
        var width = 3;
        var heigth = 3;
        var cellSize = 1;

        var board = new Board(width, heigth, cellSize);

        for (var row = 0; row < heigth; row++)
        {
            for (var col = 0; col < width; col++)
            {
                Assert.AreEqual(8, board.Cells[row, col].neighbours.Count);
            }
        }
    }

    /// <summary>
    /// ��������� ��������� ����� �� JSON-����� ��� ��� �������.
    /// </summary>
    [TestMethod]
    public void LoadBoardSettings_JsonFileExists_LoadsBoardSettings()
    {
        var boardSettings = new BoardSettings();
        boardSettings.LoadBoardSettings("../../../boardSettings.json");

        Assert.AreEqual(50, boardSettings.Width);
        Assert.AreEqual(20, boardSettings.Height);
        Assert.AreEqual(1, boardSettings.CellSize);
        Assert.AreEqual(0.5, boardSettings.LiveDensity);
    }

    /// <summary>
    /// ����������� ���������� ArgumentException ��� ������ ���� �����.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void LoadBoardSettings_EmptyFilePath_ThrowsArgumentException()
    {
        new BoardSettings().LoadBoardSettings(string.Empty);
    }

    /// <summary>
    /// ��������� ��������� ����� �������� ���������� ��� ��������� ������� �������������� JSON-�����.
    /// </summary>
    [TestMethod]
    public void LoadBoardSettings_IncorrectJsonFile_SetsZeroValues()
    {
        var boardSettings = new BoardSettings();
        boardSettings.LoadBoardSettings("../../../incorrectJson.json");

        Assert.AreEqual(0, boardSettings.Width);
        Assert.AreEqual(0, boardSettings.Height);
        Assert.AreEqual(0, boardSettings.CellSize);
        Assert.AreEqual(0, boardSettings.LiveDensity);
    }

    /// <summary>
    /// ��������� ��������� � ����.
    /// </summary>
    [TestMethod]
    public void SaveBoardState_CorrectFilePath_CreatesFile()
    {
        new Board().SaveBoardState("../../../gameOfLife_sf1.txt");

        Assert.IsTrue(File.Exists("../../../gameOfLife_sf1.txt"));
    }

    /// <summary>
    /// ����������� ���������� ArgumentException ��� ������ ���� �����.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void SaveBoardState_EmptyFilePath_ThrowsArgumentException()
    {
        new Board().SaveBoardState(string.Empty);
    }

    /// <summary>
    /// ��������� ���������� ��������� ����� �� �����.
    /// </summary>
    [TestMethod]
    public void LoadBoardState_FileExists_LoadsCorrectParameters()
    {
        var width = 50;
        var heigth = 20;
        var cellSize = 1;

        var board = new Board(width, heigth, cellSize);
        board.SaveBoardState("../../../gameOfLife_sf2.txt");
        board = new(1, 1, 1, 1);
        board.LoadBoardState("../../../gameOfLife_sf2.txt");

        Assert.AreEqual(width, board.Width);
        Assert.AreEqual(heigth, board.Height);
        Assert.AreEqual(cellSize, board.CellSize);
    }

    /// <summary>
    /// ����������� ���������� ArgumentException ��� ������ ���� �����.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void LoadBoardState_EmptyFilePath_ThrowsArgumentException()
    {
        new Board().LoadBoardState(string.Empty);
    }
}