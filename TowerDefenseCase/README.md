# Tower Defense Oyunu

Bu proje, Unity 2022.3.39f1 kullanılarak geliştirilmiş modüler bir Tower Defense oyunudur. Zenject Dependency Injection framework'ü kullanılarak SOLID prensiplerine uygun, test edilebilir ve sürdürülebilir bir kod yapısı oluşturulmuştur.

## Özellikler

- **Modüler Yapı**: Zenject DI framework'ü ile bağımlılık enjeksiyonu
- **ScriptableObject Veri Yönetimi**: Düşman ve kule verileri ScriptableObject'ler ile yönetilir
- **Dalga Sistemi**: 3-5 dalga halinde düşman saldırıları
- **Kule Yerleştirme**: Mouse ile kule yerleştirme sistemi
- **UI Sistemi**: Can, para ve dalga bilgilerini gösteren UI
- **Projeksiyon Sistemi**: Kulelerin düşmanlara projeksiyon atması

## Teknik Detaylar

### Kullanılan Framework: Zenject

**Neden Zenject Seçildi?**
- Unity için özel olarak tasarlanmış
- Performanslı ve hafif
- Factory pattern desteği
- Interface-based dependency injection
- Test edilebilirlik için mükemmel

**Nasıl Kullanıldı?**
1. **Interface-based Design**: Tüm bileşenler interface'ler üzerinden tanımlandı
2. **Factory Pattern**: Enemy ve Tower oluşturumu için factory pattern kullanıldı
3. **Dependency Injection**: Bağımlılıklar constructor injection ile enjekte edildi
4. **ScriptableObject Integration**: Veri yönetimi ScriptableObject'ler ile sağlandı

### Kod Yapısı

```
Assets/Scripts/
├── Core/
│   ├── Interfaces/          # Interface tanımları
│   ├── Data/               # ScriptableObject veri sınıfları
│   ├── Enemy/              # Düşman sistemi
│   ├── Tower/              # Kule sistemi
│   └── GameManager.cs      # Oyun yöneticisi
├── UI/                     # UI bileşenleri
└── DI/                     # Zenject installer'ları
```

### SOLID Prensipleri

1. **Single Responsibility**: Her sınıf tek bir sorumluluğa sahip
2. **Open/Closed**: ScriptableObject'ler ile genişletilebilir yapı
3. **Liskov Substitution**: Interface'ler ile değiştirilebilirlik
4. **Interface Segregation**: Küçük ve odaklanmış interface'ler
5. **Dependency Inversion**: Bağımlılıklar interface'ler üzerinden

## Kurulum

1. Unity 2022.3.39f1 sürümünü açın
2. Projeyi import edin
3. SampleScene'i açın
4. Play tuşuna basın

## Oynanış

- **Sol Tık**: Kule yerleştirme modunu başlat
- **Sol Tık (Yerleştirme Modunda)**: Kuleyi yerleştir
- **Sağ Tık**: Yerleştirme modunu iptal et

## Veri Düzenleme

Düşman ve kule verilerini düzenlemek için:
1. Assets > Create > Tower Defense > Enemy Data
2. Assets > Create > Tower Defense > Tower Data
3. Assets > Create > Tower Defense > Wave Data

Bu ScriptableObject'leri düzenleyerek oyun davranışlarını kod değiştirmeden ayarlayabilirsiniz.

## Test Edilebilirlik

Zenject sayesinde tüm bileşenler mock'lanabilir ve unit test'ler yazılabilir. Interface'ler sayesinde bağımlılıklar kolayca değiştirilebilir.

## Gelecek Geliştirmeler

- Farklı kule türleri
- Farklı düşman türleri
- Upgrade sistemi
- Farklı haritalar
- Ses efektleri
- Particle efektleri 