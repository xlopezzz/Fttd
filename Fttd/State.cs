﻿using Fttd.Entities;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fttd
{
    internal static class State
    {
        internal static bool stateTreeView; //Нуждаются ли коллекции в обновлении из БД
        internal static Employees employee; //Сотрудник запустивший приложение
        internal static Collection<Detail> detailColl = new Collection<Detail>(); //Коллекция деталей
        internal static Collection<TaskDet> taskColl = new Collection<TaskDet>(); //Коллекция заданий
        internal static Collection<TaskStatus> taskStatusColl = new Collection<TaskStatus>(); //Коллекция статусов заданий
        internal static Collection<Project> projectColl = new Collection<Project>(); //Коллекция проектов
        internal static Collection<Developer> developerColl = new Collection<Developer>(); //Коллекция разработчиков технологий
        internal static Collection<Device> deviceColl = new Collection<Device>(); //Коллекция приспособлений
        internal static Collection<Graphics> graphicsColl = new Collection<Graphics>(); //Коллекция графиков
        internal static Collection<Services> servicesColl = new Collection<Services>(); //Коллекция документов
        internal static Collection<Message> messageColl = new Collection<Message>(); //Коллекция сообщений чата
        internal static Collection<Employees> employeeColl = new Collection<Employees>(); //Коллекция сотрудников
        internal static string DirDb
        {
            get { return ConfigurationManager.AppSettings.Get("DirDB"); }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var entry = config.AppSettings.Settings["DirDB"];
                if (entry == null) config.AppSettings.Settings.Add("DirDB", value);
                else config.AppSettings.Settings["DirDB"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        } //Директория базы данных
        internal static string DirFiles
        {
            get { return ConfigurationManager.AppSettings.Get("DirFiles"); }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var entry = config.AppSettings.Settings["DirFiles"];
                if (entry == null) config.AppSettings.Settings.Add("DirFiles", value);
                else config.AppSettings.Settings["DirFiles"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        } //Директория папки с файлами
        internal static string FTTDBackup
        {
            get { return ConfigurationManager.AppSettings.Get("FTTDBackup"); }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var entry = config.AppSettings.Settings["FTTDBackup"];
                if (entry == null) config.AppSettings.Settings.Add("FTTDBackup", value);
                else config.AppSettings.Settings["FTTDBackup"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
            }
        } //День бэкапа 


        public static Grid GridMessage(Hardcodet.Wpf.TaskbarNotification.TaskbarIcon taskbarIcon, string message)
        {
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition() { MinHeight = 40 });
            grid.RowDefinitions.Add(new RowDefinition() { MinHeight = 40, Height = new GridLength(0, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(40, GridUnitType.Pixel) });
            Rectangle rectangle = new Rectangle()
            {
                Stroke = Brushes.Gray,
                Fill = Brushes.White,
                RadiusY = 10,
                RadiusX = 10,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Opacity = 0.9
            };
            rectangle.AddHandler(Expander.MouseDownEvent, new RoutedEventHandler((object sender, RoutedEventArgs e) =>
            {

                taskbarIcon.CloseBalloon();
            }));
            TextBlock textBlockHeadline = new TextBlock() { Text = "Новое сообщение", FontSize = 17, Margin = new Thickness(10, 0, 0, 0), Foreground = Brushes.Green };
            TextBlock textBlockMessage = new TextBlock() { Text = message, FontSize = 17, MaxWidth = 300, Margin = new Thickness(5, 0, 5, 5), TextWrapping = TextWrapping.Wrap };
            Button buttonClose = new Button()
            {
                Foreground = Brushes.Gray,
                BorderBrush = Brushes.Gray,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                BorderThickness = new Thickness(1, 1, 1, 1),
                Padding = new Thickness(-1, -4, 0, 0),
                Margin = new Thickness(0, 5, 7, 0),
                Width = 15,
                Height = 15,
                Background = null,
                FontWeight = FontWeights.Bold,
                Content = "x"
            };
            buttonClose.Click += (object sender, RoutedEventArgs e) =>
            {
                taskbarIcon.CloseBalloon();
            };

            grid.Children.Add(rectangle);
            Grid.SetRow(rectangle, 0);
            Grid.SetColumn(rectangle, 0);
            Grid.SetColumnSpan(rectangle, 2);
            Grid.SetRowSpan(rectangle, 2);

            grid.Children.Add(textBlockHeadline);
            Grid.SetRow(textBlockHeadline, 0);
            Grid.SetColumn(textBlockHeadline, 0);

            grid.Children.Add(textBlockMessage);
            Grid.SetRow(textBlockMessage, 1);
            Grid.SetColumn(textBlockMessage, 0);
            Grid.SetColumnSpan(textBlockMessage, 2);

            grid.Children.Add(buttonClose);
            Grid.SetRow(buttonClose, 0);
            Grid.SetColumn(buttonClose, 1);

            return grid;
        }
        
        /// <summary>
        /// Метод определяет и заполняет сотрудника запустившего приложение и заполняет коллекцию сотрудников из БД 
        /// </summary>
        public static void UpdateEmployee()
        {
            Dbaccess dbaccess = new Dbaccess();
            dbaccess.Dbselect("SELECT [Firstname], [Lastname], [Patronymic], [Shortname], [Ip], [Position], [Access], [Tabel] FROM [employees]");
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                employeeColl.Add(new Employees(vs[0], vs[1], vs[2], vs[3], vs[4], vs[5], Convert.ToInt16(vs[6]), Convert.ToInt16(vs[7])));
            }
            if (employeeColl != null) { employee = employeeColl.First(item => item.Ip == Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString()); }
        }

        /// <summary>
        /// Метод обновляет коллекцию сообщений
        /// </summary>
        /// <returns>Возвращает true если появились новые сообщения</returns>
        public static bool UpdateMessageColl()
        {
            bool result;
            int count = messageColl.Count;
            Dbaccess dbaccess2 = new Dbaccess();
            dbaccess2.Db2select("SELECT [FromEmployee], [WhereEmployee], [DateTime], [Message] FROM [chat] WHERE [FromEmployee] = '" + employee.ShortName + "' OR [FromEmployee] = 'Всем' OR [WhereEmployee] = '" + employee.ShortName + "'");
            if (count != dbaccess2.Querydata.Count)
            {
                messageColl.Clear();
                for (int i = 0; i < dbaccess2.Querydata.Count; i++)
                {
                    string[] vs = dbaccess2.Querydata[i];
                    messageColl.Add(new Message(vs[0], vs[1], vs[3], vs[2]));
                }
                result = true;
            }
            else result = false;
            return result;
        }

        public static void UpdateTaskStatusColl()
        {
            Dbaccess dbaccess = new Dbaccess();
            dbaccess.Db2select("SELECT [id], [task], [detail], [employee], [status], [problem], [solution], [data] FROM [task_status]");
            taskStatusColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                taskStatusColl.Add(new TaskStatus(Convert.ToInt16(vs[0]), vs[1], vs[2], vs[3], vs[4], vs[5], vs[6], vs[7]));
            }
        }

        /// <summary>
        /// Метод обновляет коллекции сущностей из БД
        /// </summary>
        public static void UpdateDataTreeView()
        {
            Dbaccess dbaccess = new Dbaccess();
            dbaccess.Dbselect("SELECT [detail_index], [detail_name], [inventory], [razrabotal], [project], [number_task], [data_add] FROM [detail_db] ORDER BY [detail_name]");
            detailColl.Clear();
            developerColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                detailColl.Add(new Detail(vs[2], vs[0], vs[1], vs[4], vs[3], vs[5], vs[6]));
                developerColl.Add(new Developer(vs[3]));
            }
            developerColl.Distinct().GroupBy(developerColl => developerColl.DeveloperName);
            dbaccess.Dbselect("SELECT [task], [project], [dir], [note], [iscurrent], [datein], [dateout], [leading] FROM [task] ORDER BY [task]");
            taskColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                taskColl.Add(new TaskDet(vs[0], vs[1], vs[2], vs[3], vs[4], vs[5], vs[6], vs[7]));
            }
            dbaccess.Dbselect("SELECT [project] FROM [project] ORDER BY [project]");
            projectColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                projectColl.Add(new Project(vs[0]));
            }
            dbaccess.Dbselect("SELECT [indexdev], [namedev], [razrab], [data_add] FROM [device] ORDER BY [indexdev]");
            deviceColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                deviceColl.Add(new Device(vs[1], vs[0], vs[2], vs[3]));
            }
            dbaccess.Dbselect("SELECT [namegrap], [project], [dir], [data_add] FROM [graphics] ORDER BY [namegrap]");
            graphicsColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                graphicsColl.Add(new Graphics(vs[0], vs[1], vs[2], vs[3]));
            }
            dbaccess.Dbselect("SELECT [nameserv], [dir], [note], [data_add] FROM [service] ORDER BY [nameserv]");
            servicesColl.Clear();
            for (int i = 0; i < dbaccess.Querydata.Count; i++)
            {
                string[] vs = dbaccess.Querydata[i];
                servicesColl.Add(new Services(vs[0], vs[1], vs[2], vs[3]));
            }
            stateTreeView = false;
        }

        /// <summary>
        /// Метод бэкапит базу данных через 6 дней  
        /// </summary>
        public static void BackupFTTDDB()
        {
            if (Math.Abs(DateTime.Now.Day - Convert.ToInt32(FTTDBackup)) > 6)
            {
                Directory.CreateDirectory(Directory.GetParent(DirDb).ToString() + "\\backup");
                if (!File.Exists(Directory.GetParent(DirDb).ToString() + "\\backup\\backup_from_" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + new DirectoryInfo(DirDb).Name)) File.Copy(DirDb, Directory.GetParent(DirDb).ToString() + "\\backup\\backup_from_" + DateTime.Now.ToString("dd.MM.yyyy") + "_" + new DirectoryInfo(DirDb).Name);
                FTTDBackup = Convert.ToString(DateTime.Now.Day);
            }
        }
    }
}
