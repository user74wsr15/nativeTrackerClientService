﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace nativeTrackerClientService.Entities
{
    [Table("InstallationAttachment", Schema = "Track")]
    public partial class InstallationAttachment
    {
        [Key]
        public int ID { get; set; }
        public int IMEI { get; set; }
        [Required]
        public byte[] Data { get; set; }

        [ForeignKey("IMEI")]
        [InverseProperty("InstallationAttachments")]
        public virtual Installation IMEINavigation { get; set; }
    }
}