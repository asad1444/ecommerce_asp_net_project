﻿namespace loginlogout.Models
{
    public class Contact
    {
        public int ContactId { get; set; }

        public string Email { get; set; }
        public string Message { get; set; }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }

}
