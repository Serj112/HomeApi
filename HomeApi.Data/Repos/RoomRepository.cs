using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using HomeApi.Data.Queries;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }

        public async Task<Room[]> GetRooms()
        {
            return await _context.Rooms.ToArrayAsync();
        }

        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }

        public async Task<Room> GetRoomById(Guid Id)
        {
            return await _context.Rooms.Where(r => r.Id == Id).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        //Обновление по комнате
        public async Task UpdateRoom(Room room, UpdateRoomQuery query)
        {
            var currentRoom = await _context.Rooms.Where(r => r.Id == room.Id).FirstOrDefaultAsync();

            if (currentRoom == null)
            {
                Console.WriteLine(room.Id + "Нет");
                return;
            };

            currentRoom.Name = query.NewName;
            currentRoom.Area = query.NewArea;
            currentRoom.GasConnected = query.NewGasConnected;
            currentRoom.Voltage = query.NewVoltage;

            var entry = _context.Entry(currentRoom);
            if (entry.State == EntityState.Detached)
                _context.Rooms.Update(currentRoom);

            await _context.SaveChangesAsync();
        }
    }
}