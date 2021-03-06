﻿using System.Collections.Generic;
using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public interface ITaxiRepository
    {
        void Save(Taxi taxi);
        bool Exists(string taxiId);
        void Delete(string taxiId);
        List<Taxi> GetAll();
        void Update(Taxi taxi);
    }
}