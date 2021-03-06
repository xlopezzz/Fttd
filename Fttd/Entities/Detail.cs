﻿using System;

namespace Fttd.Entities
{
    internal class Detail
    {
        public double Inventory { get; set;}
        public string Index { get; set; }
        public string DetailName { get; set; }
        public string Project { get; set; }
        public string Developer { get; set; }
        public string Task { get; set; }
        public string DataAdd { get; set; }

        public Detail()
        {
        }

        public Detail(string inventory, string index, string detailName, string project, string developer,string task, string dataAdd)
        {
            Inventory = Convert.ToDouble(inventory);
            Index = index ?? throw new ArgumentNullException(nameof(index));
            DetailName = detailName ?? throw new ArgumentNullException(nameof(detailName));
            Project = project ?? throw new ArgumentNullException(nameof(project));
            Developer = developer ?? throw new ArgumentNullException(nameof(developer));
            Task = task ?? throw new ArgumentNullException(nameof(task));
            DataAdd = dataAdd ?? throw new ArgumentNullException(nameof(dataAdd));
        }

        public override string ToString()
        {
            string description = "Деталь: " + DetailName + "\nИндекс: " + Index + "\nИнв.№ " + Convert.ToString(Inventory) + "\nПроект: " + Project + "\n№ Задания: " + Task + "\nРазработал: " + Developer + "\nДата добавления: " + DataAdd;
            return description;
        }       
    }
}
