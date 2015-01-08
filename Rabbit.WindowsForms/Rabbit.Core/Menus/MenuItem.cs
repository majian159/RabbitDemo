using System;

namespace Rabbit.Core.Menus
{
    public class MenuItem
    {
        public string Text { get; set; }

        public Action ClickAction { get; set; }

        public MenuItem[] Childs { get; set; }
    }
}