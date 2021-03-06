﻿using TakeMeThere.Models;

namespace TakeMeThere.Repositories
{
    public interface ICustomerRepository
    {
        void Update(Customer customer);
        void Save(Customer customer);
    }
}