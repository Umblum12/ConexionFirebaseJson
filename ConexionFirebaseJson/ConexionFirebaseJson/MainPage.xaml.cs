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

namespace ConexionFirebaseJson
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public class Historial
        {
            public string Nombre { get; set; }
            public DateTime? Fecha { get; set; }
            public int Flujo { get; set; }
            public bool Estado { get; set; }
        }

        private async void Button_Clicked(object sneder,EventArgs e)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://consumodeaguapi-default-rtdb.firebaseio.com/Historial.json");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject <List<Historial>> (content);
                listahistorial.ItemsSource = resultado;
            }
        }
        private async void Button_Clicked1(object sneder,EventArgs e)
        {
            DateTime dateValue;
            string dateString = "03/17/2023 01:24:00 PM";
                dateValue = DateTime.Parse(dateString);
                Historial his = new Historial
            {
                Nombre = "Abelardo",
                Fecha = dateValue,
                Flujo = 555,
                Estado = false
            };

            Uri RequestUri = new Uri("https://consumodeaguapi-default-rtdb.firebaseio.com/Historial.json");
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(his);
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
