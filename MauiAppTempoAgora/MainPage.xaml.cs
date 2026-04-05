using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(txt_cidade.Text)) 
                {
                    if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet) /*etapa de verificação se 
                                                                   o usuário possui conexão com a internet */
                    {
                        await DisplayAlert("Sem Conexão", "Você precisa de internet para buscar o clima.", "OK");
                        return; // Para a execução aqui
                    }
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if (t != null) 
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Cidade: {txt_cidade.Text} \n" +
                                         $"Clima: {t.description} \n" +
                                         $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Por do Sol: {t.sunset} \n" +
                                         $"Temp Máx: {t.temp_max} \n" +
                                         $"Temp Min: {t.temp_min} \n" +
                                         $"Vento: {t.speed} km/h \n" + 
                                         $"Visibilidade: {t.visibility} m \n";

                        lbl_res.Text = dados_previsao;

                    } else 
                    {
                        /* mensagem espefífica para a cidade não encontrada */
                        /* caso o t for nulo, significa que o StatusCode foi 404 */
                        lbl_res.Text = "Erro: Cidade não encontrada. Verifique o nome.";
                        await DisplayAlert("Aviso", "Não encontramos a cidade digitada.", "OK");
                    }

                } else 
                {
                    lbl_res.Text = "Preencha a cidade";
                }

            }
            catch (Exception ex) 
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }

        }
    }

}
