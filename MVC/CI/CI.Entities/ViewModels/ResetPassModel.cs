﻿namespace CI.Entities.ViewModels
{
    public class ResetPassModel
    {
        public string? Email { get; set; }

        public string? Token { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
