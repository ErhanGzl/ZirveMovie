using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ZirveMovie.Models
{
    public class IEntityBaseClass
    {
        #region Properties
        [Key]
        [DataMember]
        [Browsable(true)]
        [Description("AutoID")]
        [DisplayName("Kayıt No")]
        public int AutoID { get; set; }
    

        [DataMember]
        [Browsable(false)]
        [Description("CreatedBy")]
        [DisplayName("Oluşturan")]
        public int CreatedBy { get; set; }

        [DataMember]
        [Browsable(false)]
        [Description("CreatedDate")]
        [DisplayName("Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }

        [DataMember]
        [Browsable(false)]
        [Description("ModifiedBy")]
        [DisplayName("Değiştiren")]
        public int? ModifiedBy { get; set; }

        [DataMember]
        [Browsable(false)]
        [Description("ModifiedDate")]
        [DisplayName("Değiştirilme Tarihi")]
        public DateTime? ModifiedDate { get; set; }

        [DataMember]
        [Browsable(false)]
        [Description("DeletedBy")]
        [DisplayName("Silen")]
        public int? DeletedBy { get; set; }

        [DataMember]
        [Browsable(false)]
        [Description("DeletedDate")]
        [DisplayName("Silinme Tarihi")]
        public DateTime? DeletedDate { get; set; }

        [DataMember]
        [Browsable(false)]
        [Description("DataStatus")]
        [DisplayName("Kayıt Durumu")]
        public int DataStatus { get; set; }
        #endregion
    }
}
