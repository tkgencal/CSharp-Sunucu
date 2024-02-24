using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sunucu
{
    public class Sunucum
    {
        private TcpListener dinleyici;
        private int sonIstemciId = 0;
        private String ip;
        private int port;
        private int timeout;
        
        public delegate void BaglandiHandler(Kullanici kullanici);
        public event BaglandiHandler IstemciBaglandi;
        public delegate void IstemciDinleHandler(Kullanici kullanici,String mesaj);
        public event IstemciDinleHandler IstemciDinle;
        public delegate void IstemciAyrildiHandler(Kullanici kullanici,Boolean dustumu);
        public event IstemciAyrildiHandler IstemciAyrildi;

        public Sunucum(String _ip,int _port,int _timeout)
        {
            port = _port;
            ip = _ip;
            timeout = _timeout;
        }
        public void Baglan()
        {
            IPAddress ipadresi = IPAddress.Parse(ip);
            dinleyici = new TcpListener(ipadresi, port);
            dinleyici.Start();

            Thread acceptThread = new Thread(new ThreadStart(IstemciKabulEt));
            acceptThread.Start();
        }
        private void IstemciKabulEt()
        {
            while (true)
            {
                Socket istemciSoketi = dinleyici.AcceptSocket();
                if (istemciSoketi.Connected)
                {
                    Thread clientThread = new Thread(() => Dinle(istemciSoketi));
                    clientThread.Start();
                }
            }
        }
        private void Dinle(Socket istemciSoketi)
        {
            NetworkStream stream = new NetworkStream(istemciSoketi);
            StreamReader oku = new StreamReader(stream);
            StreamWriter yaz = new StreamWriter(stream);
            Boolean dustumu = false;

            string kullaniciAdi = oku.ReadLine(); // İlk mesaj ile kullanıcı adını al

            //kullanıcı oluştur
            var kullanici = new Kullanici(++sonIstemciId, kullaniciAdi, (IPEndPoint)istemciSoketi.RemoteEndPoint, stream);
        
            //istemci bağlandı metodunu kullanıcıyı göndererek tetikle
            IstemciBaglandi?.Invoke(kullanici);

            try
            {
                //istemciyi dinle
                while (istemciSoketi.Connected)
                {
                    istemciSoketi.ReceiveTimeout = timeout; //istemciden belirlenen saniyede mesaj gelmesse bağlantı kopmuştur
                    string mesaj = oku.ReadLine();
                    if (mesaj != null) //mesaj dolu ise istemcidinle metodunu kullanıcı ve mesajı ekleyerek tetikle
                    {
                        IstemciDinle?.Invoke(kullanici, mesaj);
                    }
                    else //istemci kapandı
                    {
                        break;
                    }
                }
            }
            catch (Exception) //istemci bağlantısı koptu
            {
                dustumu = true;
            }
            finally
            {
                IstemciAyrildi?.Invoke(kullanici, dustumu);
            }
        }
    }
}
