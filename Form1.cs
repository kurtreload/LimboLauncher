using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LimboLauncher
{
    public partial class Form1 : Form
    {
        private List<PatchData> listaParches;
        private string urlPatchlist = "https://limbo.org.pe/PatchHD/plist.txt";
        private string urlParches = "https://limbo.org.pe/PatchHD/";
        public Form1()
        {
            InitializeComponent();
            listaParches = new List<PatchData>();
            construirPatchlist();
            //agrega el método de parchar todo al evento de click -> actualizar el cliente
            btnActualizar.Click += async (sender, e) => await ParcharTodo();
        }

        private void btnComprobar_Click(object sender, EventArgs e)
        {
            //comprobar si el cliente está actualizado
            if (necesitaUpdate())
            {
                btnActualizar.Enabled = true;
            }
            else
            {
                btnJugar.Enabled = true;
            }
        }
        
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            
        }

        private void btnJugar_Click(object sender, EventArgs e)
        {
            //abrir el wow y cerrar este programa
            if (File.Exists("Wow.exe"))
            {
                Process.Start(new ProcessStartInfo("Wow.exe")
                {
                    UseShellExecute = true
                });
                //Application.Current.Shutdown(); // close launcher, maybe make this an option?
                this.Close();
            }
            else
            {
                txtStatus.AppendText("Por favor ubica los archivos del parche en tu carpeta del WOW !!");
                this.Close();
            }
        }
        private void construirPatchlist()
        {
            if (!Directory.Exists("Data/temp"))
                Directory.CreateDirectory("Data/temp");
            if (File.Exists("Data/temp/plist.txt"))
                File.Delete("Data/temp/plist.txt");
            WebRequest request = WebRequest.Create(urlPatchlist);
            try
            {
                txtStatus.AppendText("Obteniendo Patchlist...\r\n");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(new Uri(urlPatchlist), "Data/temp/plist.txt");
                }
                //listaParches = PreparePatchList("Data/temp/plist.txt");
                txtStatus.AppendText("Patchlist descargado!\r\n");
            }
            catch(Exception)
            {
                txtStatus.AppendText("No se pudo obtener el Patchlist. Revise su conexión y el estado del servidor\r\n");
            }
        }
        private bool necesitaUpdate()
        {
            bool necesidad = false;
            int contar = 0;
            txtStatus.AppendText("Comprobando....\r\n");
            listaParches = PreparePatchList("Data/temp/plist.txt");
            try
            {
                if (listaParches.Count > 0)
                {
                    for (int i = 0; i < listaParches.Count(); i++)
                    {
                        if (!File.Exists($"Data/{listaParches[i].Filename}"))
                        {
                            File.Create($"Data/{listaParches[i].Filename}").Close();//crea un archivo vacío si no existe
                        }
                        string localhash = string.Empty;
                        using (MD5 crypto = MD5.Create())
                        {
                            FileStream fileStream = File.OpenRead($"Data/{listaParches[i].Filename}");
                            localhash = BitConverter.ToString(crypto.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
                            fileStream.Close();
                        }
                        if (!localhash.Equals(listaParches[i].Checksum)) {
                            contar++;
                        }
                    }
                    if (contar > 0)
                    {
                        necesidad = true;
                        txtStatus.AppendText("NECESITA ACTUALIZAR!!\r\n");
                    }
                    else
                    {
                        txtStatus.AppendText("No necesita actualizar\r\n");
                    }
                }
            }
            catch (Exception)
            {
                txtStatus.AppendText("Fallo al leer el Patchlist!\r\n");
            }
            return necesidad;
        }
        private struct PatchData
        {
            public string Filename;
            public string Checksum;
        }
        private List<PatchData> PreparePatchList(string ruta)
        {
            IEnumerable<string> lista = File.ReadLines(ruta);
            listaParches = new List<PatchData>();
            foreach (string parche in lista)
            {
                string[] data = parche.Split(' ');
                listaParches.Add(new PatchData()
                {
                    Filename = data[0],
                    Checksum = data[1]
                });
            }
            return listaParches;
        }
        private async Task ParcharTodo()
        {
            txtStatus.AppendText("Iniciando parchado. Por favor no cierres el launcher ni inicies el juego!!\r\n");
            try
            {
                for (int i = 0; i < listaParches.Count(); i++)
                {
                    string localhash = string.Empty;
                    using (MD5 crypto = MD5.Create())
                    {
                        FileStream fileStream = File.OpenRead($"Data/{listaParches[i].Filename}");
                        localhash = BitConverter.ToString(crypto.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
                        fileStream.Close();
                    }
                    if (!localhash.Equals(listaParches[i].Checksum))
                    {
                        //parchar
                        using (WebClient wc = new WebClient())
                        {
                            wc.DownloadProgressChanged += (s, e) => {
                                barraProgreso.Value = e.ProgressPercentage;
                            };
                            txtStatus.AppendText($"Parchando {listaParches[i].Filename}\r\n");
                            await DescargaUnoAUno(wc, $"{urlParches}{listaParches[i].Filename}", $"Data/{listaParches[i].Filename}");
                            txtStatus.AppendText($"Parchado!! {listaParches[i].Filename}\r\n");
                        }
                    }
                }
                btnJugar.Enabled = true;
            }
            catch (Exception ex) {
                while (ex != null)
                {
                    txtStatus.AppendText(ex.Message);
                    ex = ex.InnerException;
                }
            }
        }
        private async Task DescargaUnoAUno(WebClient webClient, string archivo, string ruta)
        {
            await webClient.DownloadFileTaskAsync(new Uri(archivo), ruta);
        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
        }
    }
}
