//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JustBook.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TaiKhoanKH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoanKH()
        {
            this.GioHangs = new HashSet<GioHang>();
            this.LichSuGDs = new HashSet<LichSuGD>();
        }
    
        public int MaKH { get; set; }
        public string TenKH { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string GioiTinh { get; set; }
        public System.DateTime NgaySinh { get; set; }

        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "The Confirm Password didn't match.")]
        public string XacNhanMatKhau { get; set; }
        public string DiaChi { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GioHang> GioHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichSuGD> LichSuGDs { get; set; }
        public string LoginErrorMessage { get; internal set; }
    }
}
