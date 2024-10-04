using CST350_Minesweeper.Models;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

public class SavedGameDAO
{
    private readonly string _connectionString;

    public SavedGameDAO(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // Save the game
    public void SaveGame(SavedGame savedGame)
    {
        string sqlQuery = "INSERT INTO SavedGames (UserId, SaveTime, GameData) VALUES (@UserId, @SaveTime, @GameData)";
        using (var connection = new MySqlConnection(_connectionString))
        {
            var command = new MySqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@UserId", savedGame.UserId);
            command.Parameters.AddWithValue("@SaveTime", savedGame.SaveTime);
            command.Parameters.AddWithValue("@GameData", savedGame.GameData);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    // Get all saved games for a user
    public List<SavedGame> GetSavedGames(int userId)
    {
        var savedGames = new List<SavedGame>();
        string sqlQuery = "SELECT * FROM SavedGames WHERE UserId = @UserId";
        using (var connection = new MySqlConnection(_connectionString))
        {
            var command = new MySqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    savedGames.Add(new SavedGame
                    {
                        Id = reader.GetInt32("Id"),
                        UserId = reader.GetInt32("UserId"),
                        SaveTime = reader.GetDateTime("SaveTime"),
                        GameData = reader.GetString("GameData")
                    });
                }
            }
        }
        return savedGames;
    }

    // Get a single saved game
    public SavedGame GetSavedGame(int id)
    {
        SavedGame savedGame = null;
        string sqlQuery = "SELECT * FROM SavedGames WHERE Id = @Id";
        using (var connection = new MySqlConnection(_connectionString))
        {
            var command = new MySqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    savedGame = new SavedGame
                    {
                        Id = reader.GetInt32("Id"),
                        UserId = reader.GetInt32("UserId"),
                        SaveTime = reader.GetDateTime("SaveTime"),
                        GameData = reader.GetString("GameData")
                    };
                }
            }
        }
        return savedGame;
    }

    // Delete a saved game
    public void DeleteSavedGame(int id)
    {
        string sqlQuery = "DELETE FROM SavedGames WHERE Id = @Id";
        using (var connection = new MySqlConnection(_connectionString))
        {
            var command = new MySqlCommand(sqlQuery, connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
