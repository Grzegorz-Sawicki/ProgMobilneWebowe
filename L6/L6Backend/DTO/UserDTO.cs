﻿namespace L6Backend.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ChatGroupIds { get; set; }
    }
}