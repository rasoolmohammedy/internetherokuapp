using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TestApp
{
    class New
    {

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'radius')]")]
        public IWebElement logout_button;
    }

    class Program
    {
        [Required]
        [MaxLength(30)]
        [Display(Name = "Player Name")]
        public static string PlayerName { get; set; }

        [MaxLength(100)]
        [Display(Name = "Player Description")]
        public static string PlayerDescription { get; set; }

        [FindsBy(How = How.XPath, Using = "//a[contains(@class, 'radius')]")]
        private static IWebElement logout_button;

        static void Main(string[] args)
        {
            New n = new New();

            Console.WriteLine("value = " + ((DisplayAttribute)
  (n
    .GetType()
    .GetProperty("Using")
    .GetCustomAttributes(typeof(DisplayAttribute), true)[0])).Name);
            //var name = PlayerName.GetAttributeFrom<DisplayAttribute>(nameof(PlayerName.PlayerDescription)).Name;
            //var maxLength = player.GetAttributeFrom<MaxLengthAttribute>(nameof(player.PlayerName)).Length;
        }
    }
}
