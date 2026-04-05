using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "6135072afe7f6cec1537d5cb08a5a1a2";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}&lang=pt_br"; /*Adicionei &lang=pt_br para a descrição vir em português*/


            using (HttpClient client = new HttpClient())/*permite que se faça uma consulta na internet
                                                  * através do protocolo http*/
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (resp.IsSuccessStatusCode) /*verificação para saber se foi obtido uma resposta positiva do servidor, ou seja, 
                                               * ele não deu erro e entregou uma resposta*/ 
                { 
                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);
                    /* substituição do termo time no cálculo para epoca para evitar o erro de 1970 */
                    DateTime epoca = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    DateTime sunrise = epoca.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DateTime sunset = epoca.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();
                    /* todo esse processo foi feito por que o valor no Json esva vindo em segundos
                     a partir da famosa data 01/01/1970 chamada timestamp */

                    t = new()
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],
                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],
                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],
                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],
                        sunrise = sunrise.ToString(),
                        sunset = sunset.ToString(),
                    }; //Fecha objeto do tempo
                } // Fecha o if caso o status do servidor seja de sucesso
                else if (resp.StatusCode == HttpStatusCode.NotFound) /* caso a API responda mas não obtenha sucesso, verifica se 
                                                                      * o código é 404, ou seja, cidade inexistente */
                {
                    t = null; /* retornamos null para que a tela saiba que a cidade não existe */
                }
            } // fecha o laço using

            return t;
        }
    }
}
