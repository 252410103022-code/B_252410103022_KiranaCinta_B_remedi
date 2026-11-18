
using System;
using System.Collections.Generic;

namespace E_3022_Kirana_Cinta_Lestari_B_remedi
{
    class Agen
    {
        public string idAgen;
        public string namaAgen;
        public int stokAgen;

        public Agen(string idAgen, string namaAgen, int stokAgen)
        {
            this.idAgen = idAgen;
            this.namaAgen = namaAgen;
            this.stokAgen = stokAgen;
        }

        public void TampilInfo()
        {
            Console.WriteLine("ID Agen : " + idAgen);
            Console.WriteLine("Nama Agen : " + namaAgen);
            Console.WriteLine("Stok Agen : " + stokAgen + " galon");
        }
    }

    abstract class ProdukGalon
    {
        private string idProduk;
        private string namaProduk;
        private int stokGudang;

        public string IdProduk
        {
            get { return idProduk; }
            set { idProduk = value; }
        }

        public string NamaProduk
        {
            get { return namaProduk; }
            set { namaProduk = value; }
        }

        public int StokGudang
        {
            get { return stokGudang; }
        }

        public ProdukGalon(string idProduk, string namaProduk, int stokGudang)
        {
            this.idProduk = idProduk;
            this.namaProduk = namaProduk;
            this.stokGudang = stokGudang >= 0 ? stokGudang : 0;
        }

        public bool KurangiStok(int jumlah)
        {
            if (jumlah <= 0 || jumlah > stokGudang)
            {
                return false;
            }

            stokGudang -= jumlah;
            return true;
        }

        public abstract int HitungBiaya(int jumlah);

        public virtual void TampilInfo()
        {
            Console.WriteLine("ID Produk : " + idProduk);
            Console.WriteLine("Nama Produk : " + namaProduk);
            Console.WriteLine("Stok Gudang : " + stokGudang + " galon");
        }
    }

    class GalonAkua : ProdukGalon
    {
        private const int HARGA = 20000;

        public GalonAkua(string idProduk, string namaProduk, int stokGudang)
            : base(idProduk, namaProduk, stokGudang)
        {
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HARGA;
        }
    }

    class GalonBerkeran : ProdukGalon
    {
        private const int HARGA = 12000;
        public string jenisKeran;

        public GalonBerkeran(string idProduk, string namaProduk, int stokGudang, string jenisKeran)
            : base(idProduk, namaProduk, stokGudang)
        {
            this.jenisKeran = jenisKeran;
        }

        public override int HitungBiaya(int jumlah)
        {
            return jumlah * HARGA;
        }

        public override void TampilInfo()
        {
            base.TampilInfo();
            Console.WriteLine("Jenis Keran : " + jenisKeran);
        }
    }

    interface DapatDidistribusikan
    {
        void KirimPaket();
    }

    class DistribusiGalon : DapatDidistribusikan
    {
        private Agen agenTujuan;
        private ProdukGalon produk;
        private int jumlah;

        public DistribusiGalon(Agen agenTujuan, ProdukGalon produk, int jumlah)
        {
            this.agenTujuan = agenTujuan;
            this.produk = produk;
            this.jumlah = jumlah;
        }

        public void KirimPaket()
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Pengiriman gagal: jumlah galon harus lebih dari 0.");
                return;
            }

            if (jumlah > produk.StokGudang)
            {
                Console.WriteLine("Pengiriman gagal: stok gudang tidak mencukupi.");
                return;
            }

            if (produk.KurangiStok(jumlah))
            {
                agenTujuan.stokAgen += jumlah;

                Console.WriteLine("Paket berhasil dikirimkan ke agen " + agenTujuan.namaAgen);
                Console.WriteLine("Jumlah galon : " + jumlah);
                Console.WriteLine("Total biaya : Rp" + produk.HitungBiaya(jumlah));
            }
        }
    }

    class SindikatGalon
    {
        private List<Agen> daftarAgen = new List<Agen>();
        private List<ProdukGalon> daftarProduk = new List<ProdukGalon>();

        public void TambahAgen(Agen agen)
        {
            daftarAgen.Add(agen);
        }

        public void TambahProduk(ProdukGalon produk)
        {
            daftarProduk.Add(produk);
        }

        public void TampilkanSindikat()
        {
            Console.WriteLine("=== DATA SINDIKAT MAFIA GALON ===");

            Console.WriteLine("\nDaftar Agen:");
            foreach (Agen agen in daftarAgen)
            {
                agen.TampilInfo();
                Console.WriteLine();
            }

            Console.WriteLine("Daftar Produk:");
            foreach (ProdukGalon produk in daftarProduk)
            {
                produk.TampilInfo();
                Console.WriteLine();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Agen agen1 = new Agen("A001", "Budi", 10);
            Agen agen2 = new Agen("A002", "Andi", 15);

            GalonAkua akua = new GalonAkua("P001", "Galon Akua", 50);
            GalonBerkeran berkeran = new GalonBerkeran("P002", "Galon Berkeran", 30, "Keran Putar");

            Console.WriteLine("=== DATA AGEN ===");
            agen1.TampilInfo();
            Console.WriteLine();
            agen2.TampilInfo();

            Console.WriteLine("\n=== POLYMORPHISM ===");

            ProdukGalon produk1 = akua;
            ProdukGalon produk2 = berkeran;

            Console.WriteLine("Biaya 2 Galon Akua : Rp" + produk1.HitungBiaya(2));
            Console.WriteLine("Biaya 2 Galon Berkeran : Rp" + produk2.HitungBiaya(2));

            Console.WriteLine("\n=== DISTRIBUSI GALON ===");

            DistribusiGalon kirim1 = new DistribusiGalon(agen1, akua, 5);
            kirim1.KirimPaket();

            Console.WriteLine();

            DistribusiGalon kirim2 = new DistribusiGalon(agen2, berkeran, 0);
            kirim2.KirimPaket();

            Console.WriteLine();

            DistribusiGalon kirim3 = new DistribusiGalon(agen2, akua, 100);
            kirim3.KirimPaket();

            SindikatGalon sindikat = new SindikatGalon();

            sindikat.TambahAgen(agen1);
            sindikat.TambahAgen(agen2);

            sindikat.TambahProduk(akua);
            sindikat.TambahProduk(berkeran);

            Console.WriteLine();
            sindikat.TampilkanSindikat();

            Console.WriteLine("\n=== PROGRAM SELESAI ===");
        }
    }
}
