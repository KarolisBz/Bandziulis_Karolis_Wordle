using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace Wordle_Karolis_G00417529
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}