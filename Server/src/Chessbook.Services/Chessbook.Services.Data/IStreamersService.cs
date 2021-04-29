﻿using System.Threading.Tasks;

namespace Chessbook.Services.Data
{
    public interface IStreamersService
    {
        Task<string> GetByUserId(int userId);

        Task<string[]> GetAllLoginNames();

        Task<string> SaveUserLogin(string userLogin, int userId);

        Task<string> EditUserLogin(string userLogin, int userId);

    }
}
