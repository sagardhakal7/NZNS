using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NZNA.Areas.Admin.Models
{
    public class Nzna
    {
    }
    public class Siteset : MandatoryFields
    {
        public int SitesetId { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string FbId { get; set; }
        public string TwitterId { get; set; }
        public string GoogleId { get; set; }
        public string InstagramId { get; set; }
        public string LinkedinId { get; set; }
        public string Copyright { get; set; }

    }

    public class Album : MandatoryFields
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Banner : MandatoryFields
    {
        public int BannerId { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public string ImageUrl { get; set; }
       
    }
    public class Gallery : MandatoryFields
    {
        public int GalleryId { get; set; }
        public string Title { get; set; }
        public string Tagline { get; set; }
        public string ImageUrl { get; set; }
        public string AlbumId { get; set; }
        public Album Album { get; set; }
    }

    public class Aboutpage : MandatoryFields
    {
        public int AboutpageId { get; set; }
        public string Tagline { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Saugat : MandatoryFields
    {
        public int SaugatId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
    }

    public class Member : MandatoryFields
    {
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
        public string IsExcom { get; set; }
        //public string Year { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public string UniqueId { get; set; }
    }

    public class PastExComMember : MandatoryFields
    {
        public int PastExComMemberId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string ImageUrl { get; set; }
        public string Year { get; set; }
        public string Linkedin { get; set; }
        public string Facebook { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string Address { get; set; }
        public string DOB { get; set; }
        public string UniqueId { get; set; }
    }

    public class RelatedLink : MandatoryFields
    {
        public int RelatedLinkId { get; set; }
        public string Title { get; set; }
        public string TitleLink { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class Event : MandatoryFields
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string EventDate { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Timing { get; set; }
        public string Address { get; set; }
        public string IsCompleted { get; set; }
    }

    public class News : MandatoryFields
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string LinkUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }

    }



    public class Contact : MandatoryFields
    {
        public int ContactId { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class MandatoryFields
    {
        [ScaffoldColumn(false)]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        [ScaffoldColumn(false)]
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        [ScaffoldColumn(false)]
        public DateTime? ModifiedDate { get; set; }
        [ScaffoldColumn(false)]
        public string ModifiedBy { get; set; }
        [ScaffoldColumn(false)]
        public Boolean DelFlg { get; set; }
        //public Boolean Active { get; set; }
    }

}