using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WPF_exo4
{
    [DataContract]
    public class Files : INotifyPropertyChanged
    {

        public Files(string Path)
        {
            _Path = Path;
            _NomFichier = Path.Substring(_Path.LastIndexOf('\\') + 1);
        }

        private string _contenu;
        [DataMember(Order = 2)] 
        public string Contenu
        {
            get { return _contenu; }
            set { _contenu = value; }
        }


        private string _Path;

        [DataMember(Order = 1)]
        public string Path
        {
            get { return _Path; }
            set {
                if (this._Path != value)
                {
                    this._Path = value;
                    this.NotifyPropertyChanged("Path");
                }
                ;}
        }

        private string _NomFichier;
        [DataMember(Order = 0)]
        public string NomFichier
        {
            get { return _NomFichier; }
            set {
                if (this._NomFichier != value)
                {
                    this._NomFichier = value;
                    this.NotifyPropertyChanged("Nom_Fichier");
                }
            }
        }

        


        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public override string ToString()
        {
            return _NomFichier;
        }
    }
}
