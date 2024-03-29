﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppListaDeTarefas.Modelos;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppListaDeTarefas.Telas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Inicio : ContentPage
    {
        public Inicio()
        {
            InitializeComponent();
            CarregarTarefas();
            DataHoje.Text = DateTime.Now.DayOfWeek.ToString() + "," + DateTime.Now.ToString("dd/MM");
        }
        public void ActionGoCadastro(object sender,EventArgs args)
        {
            Navigation.PushAsync(new Cadastro());
        }
        private void CarregarTarefas()
        {
            SLTarefas.Children.Clear();

            List<Tarefa> Lista = new GerenciadorTarefa().Listagem();
            int i = 0;



            foreach (var tarefa in Lista)
            {
                LinhaStackLayout(tarefa,i);
                i++;
            }
        }

        public void LinhaStackLayout(Tarefa tarefa,int index)
        {
            Image Delete = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("Delete.png")
            };

            if (Device.RuntimePlatform == Device.UWP)
            {
                Delete.Source = ImageSource.FromFile("Resources/Delete.png");
            }
            TapGestureRecognizer DeleteTap = new TapGestureRecognizer();
            DeleteTap.Tapped += delegate
            {
                new GerenciadorTarefa().Remover(index);
                CarregarTarefas();


            };
            Delete.GestureRecognizers.Add(DeleteTap);


            Image Prioridade = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("p"+tarefa.Prioridade + ".png")
            };

            if (Device.RuntimePlatform == Device.UWP)
            {
                Prioridade.Source = ImageSource.FromFile("Resources/p" + tarefa.Prioridade + ".png");
            }
            View StackCentral = null;
            if(tarefa.dataFinalizacao == null)
            {
                StackCentral = new Label()
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Text = tarefa.Nome
                };
            }
            else
            {
                StackCentral = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.Center,
                    Spacing = 0,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                ((StackLayout)StackCentral).Children.Add(new Label() { Text = tarefa.Nome, TextColor = Color.Gray });
                ((StackLayout)StackCentral).Children.Add(new Label()
                {
                    Text = "Finalizado em " + tarefa.dataFinalizacao.Value.ToString("dd/MM/yyyy - hh:mm" + "h"),
                    TextColor = Color.Gray,
                    FontSize = 10
                });
            };


    

            Image Check = new Image()
            {
                VerticalOptions = LayoutOptions.Center,
                Source = ImageSource.FromFile("CheckOff.png")
            };

            if(Device.RuntimePlatform == Device.UWP)
            {
                Check.Source = ImageSource.FromFile("Resources/CheckOff.png");
            }
            if(tarefa.dataFinalizacao != null)
            {
                Check.Source = ImageSource.FromFile("CheckOn.png");
                if (Device.RuntimePlatform == Device.UWP)
                {
                    Check.Source = ImageSource.FromFile("Resources/CheckOn.png");
                }
            }
            TapGestureRecognizer CheckTap = new TapGestureRecognizer();
            CheckTap.Tapped += delegate
            {
                new GerenciadorTarefa().Finalizar(index,tarefa);
                CarregarTarefas();


            };
            Check.GestureRecognizers.Add(CheckTap);
            StackLayout linha = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 15
            };

            linha.Children.Add(Check);
            linha.Children.Add(StackCentral);
            linha.Children.Add(Prioridade);
            linha.Children.Add(Delete);

            SLTarefas.Children.Add(linha);
        }
    }
}