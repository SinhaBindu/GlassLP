using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassLP.Models
{
    public class ParticipantM1ViewModel
    {
        public int ParticipantId_pk { get; set; } = 0;

        [DisplayName("Camp Code")]
        public int CampId_fk { get; set; }

        [DisplayName("Participant Name")]
        public string? ParticipantName { get; set; }

        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }

        [DisplayName("Age")]
        public int Age { get; set; }

        [DisplayName("SHG Name")]
        public string? SHGName { get; set; }

        [DisplayName("Occupation")]
        public int OccupationId { get; set; }

        [DisplayName("Occupation Others")]
		public string? Occupation_Others { get; set; }

        [DisplayName("Vision Issue Identified")]
        public int VisionIssueIdentifiedId { get; set; }

        [DisplayName("Type of Vision Issue")]
        public int TypeofVisionIssueId { get; set; }
        [DisplayName("Type of Vision Issue (Other)")]
        public string TypeofVisionIssue_Others { get; set; }

        [DisplayName("Glasses Provided")]
        public int GlassesProvidedId { get; set; }

        [DisplayName("Power of Glass")]
        public int PowerofGlassId { get; set; }

        [DisplayName("Feedback on Comfort")]
        public int FeedbackonComfort { get; set; }

        [DisplayName("Remarks")]
        public string? Remarks { get; set; }

        [DisplayName("Referral Required")]
        public int FollowupRequiredId { get; set; }

        [DisplayName("Digital Consent")]
        public int DigitalConsentId { get; set; }

        [NotMapped]
        [ValidateNever]
        public IEnumerable<SelectListItem> VSIdList { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Version { get; set; }
        public DateTime SynDate { get; set; }
        [NotMapped]
        [ValidateNever]
        public string uuid { get; set; }
    }
}
