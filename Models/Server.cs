using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerInventoryApp.Models
{
    public class Server
    {
        [Key]
        public int Id { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Sunucu adı zorunludur.")]
        [StringLength(500, ErrorMessage = "Sunucu adı en fazla 500 karakter olabilir.")]
        [Display(Name = "Sunucu Adı")]
        public string Name { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "IP adresi zorunludur.")]
        [Display(Name = "IP Adresi")]
        public string IPAddress { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçin.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Display(Name = "Şifreli Sunucu Adı")]
        public string? EncryptedName { get; set; }

        [Display(Name = "Şifreli IP Adresi")]
        public string? EncryptedIPAddress { get; set; }

        public void EncryptData()
        {
            EncryptedName = string.IsNullOrEmpty(Name) ? string.Empty : EncryptionHelper.Encrypt(Name);
            EncryptedIPAddress = string.IsNullOrEmpty(IPAddress) ? string.Empty : EncryptionHelper.Encrypt(IPAddress);
        }

        public void DecryptData()
        {
            Name = string.IsNullOrEmpty(EncryptedName) ? string.Empty : EncryptionHelper.Decrypt(EncryptedName);
            IPAddress = string.IsNullOrEmpty(EncryptedIPAddress) ? string.Empty : EncryptionHelper.Decrypt(EncryptedIPAddress);
        }
    }
}
