using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_cidade.Text))
            {
                await DisplayAlert("Atenção", "Preencha a cidade.", "OK");
                return;
            }

            try
            {
                Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                if (t != null)
                {
                    lbl_res.Text = $"Latitude: {t.lat}\n" +
                                   $"Longitude: {t.lon}\n" +
                                   $"Nascer do Sol: {t.sunrise}\n" +
                                   $"Por do Sol: {t.sunset}\n" +
                                   $"Temp Máx: {t.temp_max}°C\n" +
                                   $"Temp Min: {t.temp_min}°C\n" +
                                   $"Velocidade do Vento: {t.speed} m/s\n" +
                                   $"Visibilidade: {t.visibility} m\n" +
                                   $"Descrição: {t.description}";
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível obter a previsão.", "OK");
                }
            }
            catch
            {
                await DisplayAlert("Erro", "Falha ao conectar à API.", "OK");
            }
        }
    }
}
