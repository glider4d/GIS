using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace testXamarin.Views
{
    class FirstPage : ContentPage
    {


        Label label1;
        Entry entry;
        public FirstPage()
        {
            entry = new Entry
            {
                Placeholder = "Напиши что-нибудь)",
                TextColor = Color.Lime
            };

            Button button1 = new Button
            {
                Text = "Нажми меня"
            };

            button1.FontAttributes = FontAttributes.Bold;
            button1.Clicked += button1_clicked;

            label1 = new Label();
            label1.FontSize = 27;


            Content = new StackLayout()
            {
                Children = { entry, button1, label1 }
            };
        }

        public void button1_clicked(object sender, EventArgs e)
        {
            label1.Text = entry.Text;
        }

        public void button2_clicked(object sender, EventArgs e)
        {

        }

        

    }
}

