using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static int[] sayilar = new int[15 * 15];
    static Random rnd = new Random();
      static void Main()
    {
        Console.WriteLine("SHA ile sayı üretme : 1 \nRNG ile sayı üretme : 2 ");
        string islem = Console.ReadLine();
        if (islem == "1")
        {
            sayiuret();
            tablolustur();

            Console.ReadKey();
        }
        else if (islem == "2")
        {
            int[,] sayilar = new int[15, 15];
            randomsayi(sayilar);
            tablo(sayilar);
            Console.ReadKey();
        }
       
    }
    static void sayiuret()
    {     
        HashSet<int> kullanilansayilar = new HashSet<int>();       
        for (int i = 0; i < sayilar.Length; i++)
        {
            string kelime1 = kelimeolustur();
            string kelime2 = kelimeolustur();
            string kelime3 = kelimeolustur();            
            string birlestir = $"{kelime1}{kelime2}{kelime3}";
            string sifrele = sha_hesapla(birlestir);
            BigInteger sifredonustur = sifrecevir(sifrele.Substring(0, 13));
            int sonuc = sayikucult(sifredonustur);

            while (kullanilansayilar.Contains(sonuc) || kullanilansayilar.Count(n => n == sonuc) >= 2)
            {
                kelime1 = kelimeolustur();
                kelime2 = kelimeolustur();
                kelime3 = kelimeolustur();

                birlestir = $"{kelime1}{kelime2}{kelime3}";
                sifrele = sha_hesapla(birlestir);
                sifredonustur = sifrecevir(sifrele.Substring(0, 13));
                sonuc = sayikucult(sifredonustur);
            }
           
                Console.WriteLine($"Sonuc {i + 1}:\t{kelime1} {kelime2} {kelime3}\nHash: {birlestir}\ndonustur: {sifrele}\tsayı: {sonuc}\n");
           
            sayilar[i] = sonuc;
            kullanilansayilar.Add(sonuc);
          
        }
    }
    static string kelimeolustur()
    {
        const string karakter = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(karakter, 15)
          .Select(s => s[rnd.Next(s.Length)]).ToArray());
    }
    static string sha_hesapla(string veri)
    {
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] hash = sha512.ComputeHash(Encoding.UTF8.GetBytes(veri));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }  
    static BigInteger sifrecevir(string hex)
    {
        BigInteger bsayi;
        if (BigInteger.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out bsayi))
        {return bsayi;}
        else{return 0;}
    }
    static int sayikucult(BigInteger s)
    {
        int sonuc = (int)(s % 256);
        sonuc = sonuc < 0 ? 256 + sonuc : sonuc % 256; 
        return sonuc;
    }

    static void tablolustur()
    {
        Console.WriteLine("\nSayılar :");

        for (int i = 0; i < sayilar.Length; i++)
        {
            Console.Write(sayilar[i] + "\t");
            if ((i + 1) % 15 == 0)
                Console.WriteLine();
        }
    }



    /***************************************RANDOM NUMBER GENERATOR KÜTÜPHANESİ************************************************************************/

    static void randomsayi(int[,] dizi)
    {
        using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
        {
            for (int i = 0; i < dizi.GetLength(0); i++)
            {
                for (int j = 0; j < dizi.GetLength(1); j++)
                {
                    byte[] rndb = new byte[1];
                    rng.GetBytes(rndb);
                    int random = rndb[0] % 256;
                    dizi[i, j] = random;

                }
            }
        }
    }
    static void tablo(int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int a = 0; a < array.GetLength(1); a++)
            {
                Console.Write(array[i, a] + "\t");
            }
            Console.WriteLine();
        }
    }

}
