using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sunucu
{
    public class Kullanici
    {
        public int IstemciId { get; set; }
        public string KullaniciAdi { get; set; }
        public IPEndPoint UcNoktasi { get; set; }
        public NetworkStream NetworkStream { get; set; }  // Kullanıcının ağ akışını tutmak için.
        public void MesajGonder(String _mesaj)
        {
            StreamWriter writer = new StreamWriter(NetworkStream);
            writer.WriteLine(_mesaj);
            writer.Flush();  // Mesajın hemen gönderilmesini sağlar.
        }
        public Kullanici(int id, string kullaniciAdi, IPEndPoint ucNoktasi, NetworkStream networkStream)
        {
            IstemciId = id;
            KullaniciAdi = kullaniciAdi;
            UcNoktasi = ucNoktasi;
            NetworkStream = networkStream;
        }
    }
}
