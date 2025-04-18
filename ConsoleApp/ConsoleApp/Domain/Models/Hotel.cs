namespace ConsoleApp.Domain.Models;

public record Hotel(string Id, 
    string Name, 
    List<RoomType> RoomTypes,  
    List<Room> Rooms);
