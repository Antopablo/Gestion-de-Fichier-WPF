using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;
using System.Runtime.Serialization.Json;

namespace WPF_exo4
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Files> Liste_FILES_CollectionObserv = new ObservableCollection<Files>();
        public MainWindow()
        {
            InitializeComponent();
            ListView.ItemsSource = Liste_FILES_CollectionObserv;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TXT Files|*.txt"; //limite aux fichier .txt
            openFileDialog.Multiselect = true; //permet de selectionner plusieurs fichiers avec CTRL

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string item in openFileDialog.FileNames)
                {
                    Liste_FILES_CollectionObserv.Add(new Files(item));
                }
            }
        }

        private void Bouton_joindre_Click(object sender, RoutedEventArgs e)
        {
            if (Liste_FILES_CollectionObserv.Count > 0)
            {
                string total = "";
                foreach (Files item in Liste_FILES_CollectionObserv)
                {
                    StreamReader stm = new StreamReader(item.Path);
                    total += stm.ReadToEnd() + "\r\n";
                    stm.Close();
                }

                StreamWriter stmWritter = new StreamWriter("resultat.txt");
                stmWritter.Write(total);
                stmWritter.Close();

                Result.Text = total;
            } else
            {
                Result.Text = "Rien à joindre";
            }
            
        }

        private void RAZ_Click(object sender, RoutedEventArgs e)
        {
            Result.Text = "";
            Liste_FILES_CollectionObserv.Clear();
        }

        private void Supr_Click(object sender, RoutedEventArgs e)
        {
            if (Liste_FILES_CollectionObserv.Count >= 1)
            {
                Liste_FILES_CollectionObserv.Remove((Files)ListView.SelectedItem);
            } else
            {
                Result.Text = "Liste déjà vide";
            }
        }

        private void SauvJSON_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (Files item in Liste_FILES_CollectionObserv)
                {
                    string total = "";
                    StreamReader stm = new StreamReader(item.Path);
                    total = stm.ReadToEnd();
                    stm.Close();
                    item.Contenu = total;
                }

                Stream stm2 = new StreamWriter("Save.json", true).BaseStream;
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<Files>));
                ser.WriteObject(stm2, Liste_FILES_CollectionObserv);
                stm2.Close();

                Result.Text = "Sauvegarde Faite";
            }
            catch (Exception)
            {
                MessageBox.Show("erreur de Sauvegarde", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }

        private void ChargeJSON_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON Files|*.json";
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<Files>));
                if (openFileDialog.ShowDialog() == true)
                {
                    Stream stm = new StreamReader(openFileDialog.FileNames[0], true).BaseStream;
                    ObservableCollection<Files> tmp = (ObservableCollection<Files>)ser.ReadObject(stm);
                    stm.Close();
                    foreach (Files item in tmp)
                    {
                        Liste_FILES_CollectionObserv.Add(item);
                        Result.Text += item.Contenu + "\r\n";
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Fichier non trouvé", "ERREUR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }
    }
}
