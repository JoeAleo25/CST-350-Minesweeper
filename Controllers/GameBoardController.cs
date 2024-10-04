using CST350_Minesweeper.Models;
using CST350_Minesweeper.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class GameBoardController : Controller
{
    private static GameBoard gameBoard;
    private static bool gameOver = false;
    private readonly GameService _gameService;
    private readonly SavedGameDAO _savedGameDAO;

    public GameBoardController(GameService gameService, SavedGameDAO savedGameDAO)
    {
        _gameService = gameService;
        _savedGameDAO = savedGameDAO;
    }

    public IActionResult Index()
    {
        if (gameBoard == null)
        {
            gameBoard = _gameService.CreateGameBoard(10, 10, 20);
            gameOver = false;
        }

        ViewData["GameOver"] = gameOver;
        return View(gameBoard);
    }

    [HttpPost]
    public IActionResult RevealCell(int row, int column)
    {
        if (!gameOver)
        {
            _gameService.RevealCell(gameBoard, row, column);
            if (gameBoard.GameOver)
            {
                gameOver = true;
                SaveGame();  // Automatically save the game when it ends
                return RedirectToAction("GameOver");
            }
        }
        return PartialView("_GameBoard", gameBoard);
    }

    [HttpPost]
    public IActionResult ToggleFlag(int row, int column)
    {
        if (!gameOver)
        {
            _gameService.ToggleFlag(gameBoard, row, column);
        }
        return PartialView("_GameBoard", gameBoard);
    }

    [HttpPost]
    public IActionResult StartGame()
    {
        gameBoard = _gameService.CreateGameBoard(10, 10, 20);
        gameOver = false;
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SaveGame()
    {
        var userId = 1;  // Replace with actual user ID logic
        var gameData = JsonConvert.SerializeObject(gameBoard);

        var savedGame = new SavedGame
        {
            UserId = userId,
            SaveTime = DateTime.Now,
            GameData = gameData
        };

        _savedGameDAO.SaveGame(savedGame);
        return RedirectToAction("Index");
    }

    public IActionResult LoadGame(int id)
    {
        var savedGame = _savedGameDAO.GetSavedGame(id);
        if (savedGame != null)
        {
            gameBoard = JsonConvert.DeserializeObject<GameBoard>(savedGame.GameData);
            gameOver = false;
            return RedirectToAction("Index");
        }
        return NotFound();
    }

    public IActionResult ShowSavedGames()
    {
        var userId = 1;  // Replace with actual user ID logic
        var savedGames = _savedGameDAO.GetSavedGames(userId);
        return View(savedGames);
    }

    public IActionResult GameOver()
    {
        return View();
    }
}
