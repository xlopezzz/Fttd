﻿using System;

namespace Fttd.Entities
{
    internal class Graphics
    {
        public Graphics()
        {
        }

        public Graphics(string nameGrap, string projectGrap, string dirGrap, string dateAddGrap)
        {
            NameGrap = nameGrap ?? throw new ArgumentNullException(nameof(nameGrap));
            ProjectGrap = projectGrap ?? throw new ArgumentNullException(nameof(projectGrap));
            DirGrap = dirGrap ?? throw new ArgumentNullException(nameof(dirGrap));
            DateAddGrap = DateTime.Parse(dateAddGrap);
        }

        public string NameGrap { get; set; }
        public string ProjectGrap { get; set; }
        public string DirGrap { get; set; }
        public DateTime DateAddGrap { get; set; }

        public override string ToString()
        {
            string description = "График: " + NameGrap + "\nПроект: " + ProjectGrap + "\nДата добавления: " + DateAddGrap;
            return description;
        }
    }
}
