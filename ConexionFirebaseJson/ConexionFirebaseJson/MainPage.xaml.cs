using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net;
using System.Collections.ObjectModel;
using System.IO;
using static ConexionFirebaseJson.MainPage;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Xml.Linq;

namespace ConexionFirebaseJson
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public class ConsumoAgua
        {
            public Dictionary<string, MedidaAgua> Historial { get; set; }
        }

        public class MedidaAgua
        {
            public bool Estado { get; set; }
            public DateTime Fecha { get; set; }
            public int Flujo { get; set; }
            public string Nombre { get; set; }
        }

        public async Task<ConsumoAgua> GetConsumoAgua()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://consumodeaguapi-default-rtdb.firebaseio.com/Consumodeagua.json");
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ConsumoAgua>(json);
        }



        private async void Button_Clicked(object sneder,EventArgs e)
        {
            await Task.Run(async () =>
            {
                var consumoAgua = await GetConsumoAgua();
                var medidasAgua = new List<MedidaAgua>(consumoAgua.Historial.Values);
                Device.BeginInvokeOnMainThread(() =>
                {
                    miListView.ItemsSource = medidasAgua;
                });
            });
        }
        private async void Button_Clicked1(object sneder,EventArgs e)
        {
            DateTime dateValue;
            string dateString = "03/17/2023 01:24:00 PM";
                dateValue = DateTime.Parse(dateString);
            MedidaAgua model = new MedidaAgua
            {
                Nombre = "Abelardo",
                Fecha = dateValue,
                Flujo = 555,
                Estado = false
            };

            Uri RequestUri = new Uri("https://consumodeaguapi-default-rtdb.firebaseio.com/Consumodeagua/Historial.json");
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(model);
            var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(RequestUri, contentJson);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                await DisplayAlert("Datos", "Se creo correctamente el registro ", "ok");
            }
            else
            {
                await DisplayAlert("Datos", "Ocurrio un error ", "ok");
            }
        }
    }
}
