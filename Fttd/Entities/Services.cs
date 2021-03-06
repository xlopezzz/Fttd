﻿using System;

namespace Fttd.Entities
{
    internal class Services
    {
        public Services()
        {
        }

        public Services(string nameServ, string dirServ, string note, string dateAddServ)
        {
            NameServ = nameServ ?? throw new ArgumentNullException(nameof(nameServ));
            DirServ = dirServ ?? throw new ArgumentNullException(nameof(dirServ));
            Note = note ?? throw new ArgumentNullException(nameof(note));
            DateAddServ = DateTime.Parse(dateAddServ);
        }
        
        public string NameServ { get; set; }
        public string DirServ { get; set; }
        public string Note { get; set; }
        public DateTime DateAddServ { get; set; }

        public override string ToString()
        {
            string description = "Документ: " + NameServ + "\nКороткое описание: " + Note + "\nДата добавления: " + Convert.ToString(DateAddServ);
            return description;
        }

    }
}
