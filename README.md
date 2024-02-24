Derleyip dll dosyasını kullanmak istediğiniz projeye import edin.

          Örnek Kod;

            Sunucum sunucum = new Sunucum("192.168.1.20",4141,15000);
            sunucum.Baglan();
            sunucum.IstemciBaglandi += Sunucum_IstemciBaglandi;
            sunucum.IstemciDinle += Sunucum_IstemciDinle;
            sunucum.IstemciAyrildi += Sunucum_IstemciAyrildi;


        private void Sunucum_IstemciBaglandi(Kullanici kullanici)
        {
            throw new NotImplementedException();
            //kodlarınız buraya
            MessageBox.Show(kullanici.KullaniciAdi+"- bağlandı");
        }


        private void Sunucum_IstemciDinle(Kullanici kullanici, string mesaj)
        {
            throw new NotImplementedException();
                  //kodlarınız buraya
            MessageBox.Show(kullanici.KullaniciAdi+"- "+mesaj);
        }

            
        private void Sunucum_IstemciAyrildi(Kullanici kullanici, bool dustumu)
        {
            throw new NotImplementedException();
           
            if (dustumu)
                 MessageBox.Show(kullanici.KullaniciAdi+"- düştü");
            else
             MessageBox.Show(kullanici.KullaniciAdi+"- ayrıldı");
        }
