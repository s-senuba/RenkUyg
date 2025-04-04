Imports System.Windows.Media.Imaging
Imports System.Windows.Media
Imports System.IO
Imports Microsoft.Win32

Class MainWindow

    ' Seçilen rengin ortalama rengini tutacak değişken
    Private _secilenRenk As Color

    ' Fotoğraf Yükleme ve İşleme
    Private Sub YuklenenResmiIsle(dosyaYolu As String)
        ' Fotoğrafı yükle
        Dim resim As BitmapImage = New BitmapImage(New Uri(dosyaYolu))

        ' Ortalama rengi hesapla
        _secilenRenk = OrtalamaRenkHesapla(resim)

        ' HEX ve RGB kodlarını UI'ye yaz
        txtHexKodu.Text = $"#{_secilenRenk.R:X2}{_secilenRenk.G:X2}{_secilenRenk.B:X2}"
        txtRgbKodu.Text = $"{_secilenRenk.R}, {_secilenRenk.G}, {_secilenRenk.B}"

        ' Renk göstergesini güncelle
        rectRenkGoster.Fill = New SolidColorBrush(_secilenRenk)
    End Sub

    ' Ortalama rengi hesaplamak için yardımcı fonksiyon
    Private Function OrtalamaRenkHesapla(resim As BitmapImage) As Color
        Dim genislik As Integer = resim.PixelWidth
        Dim yukseklik As Integer = resim.PixelHeight

        Dim toplamKirmizi As Integer = 0
        Dim toplamYesil As Integer = 0
        Dim toplamMavi As Integer = 0

        ' BitmapImage'ı WriteableBitmap'e dönüştür
        Dim writeableBitmap As New WriteableBitmap(resim)

        ' Piksel verilerini almak için bir byte dizisi oluştur
        Dim pikselVerisi As Byte() = New Byte(writeableBitmap.PixelWidth * writeableBitmap.PixelHeight * 4 - 1) {}

        ' Fotoğrafın tüm piksellerini byte dizisine kopyala
        writeableBitmap.CopyPixels(pikselVerisi, writeableBitmap.PixelWidth * 4, 0)

        ' Her pikselin rengini al
        For y As Integer = 0 To yukseklik - 1
            For x As Integer = 0 To genislik - 1
                ' Pikselin veri indeksini hesapla
                Dim index As Integer = (y * genislik + x) * 4

                ' Piksel rengini al (ARGB: Alpha, Red, Green, Blue)
                Dim alpha As Byte = pikselVerisi(index + 3)
                Dim red As Byte = pikselVerisi(index + 2)
                Dim green As Byte = pikselVerisi(index + 1)
                Dim blue As Byte = pikselVerisi(index)

                ' Renk bileşenlerini toplama
                toplamKirmizi += red
                toplamYesil += green
                toplamMavi += blue
            Next
        Next

        ' Ortalama renk hesapla
        Dim pikselSayisi As Integer = genislik * yukseklik
        Dim ortalamaKirmizi As Byte = CByte(toplamKirmizi / pikselSayisi)
        Dim ortalamaYesil As Byte = CByte(toplamYesil / pikselSayisi)
        Dim ortalamaMavi As Byte = CByte(toplamMavi / pikselSayisi)

        Return Color.FromRgb(ortalamaKirmizi, ortalamaYesil, ortalamaMavi)
    End Function

    ' Resim Yükle Butonuna tıklanınca çalışacak metod
    Private Sub BtnResimYukle_Click(sender As Object, e As RoutedEventArgs)
        ' Dosya seçim penceresi aç
        Dim openFileDialog As New OpenFileDialog()
        openFileDialog.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp"

        If openFileDialog.ShowDialog() = True Then
            ' Seçilen resmin yolunu al ve işle
            YuklenenResmiIsle(openFileDialog.FileName)
        End If
    End Sub

End Class
