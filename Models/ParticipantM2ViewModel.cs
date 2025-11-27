using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlassLP.Models
{
    public class ParticipantM2ViewModel
    {
        public int ParticipantId_pk { get; set; } = 0;

        [DisplayName("Type of Participant")]
        public int TypeofParticipantId{ get; set; }
		
		[DisplayName("Capm Code")]
		public int CampId_fk { get; set; }

		[DisplayName("District")]
		public int? DistrictId { get; set; }

		[DisplayName("Block")]
		public int? BlockId { get; set; }

		[DisplayName("CLF")]
		public int? CLFId { get; set; }

		[DisplayName("Panchayat")]
		public int? PanchayatId { get; set; }

		[DisplayName("Participant Name")]
        public string? ParticipantName { get; set; }

        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }

        [DisplayName("Age")]
        [Range(35, 59, ErrorMessage = "Age must be between 35 and 59")]
        public int Age { get; set; }

        [DisplayName("Screening Date")]
        public DateTime? ScreeningDate { get; set; }

        [DisplayName("SHG Name")]
        public string? SHGName { get; set; }

        [DisplayName("Occupation")]
        public int OccupationId { get; set; }

        [DisplayName("Occupation Others")]
        [ValidateNever]
        public string? Occupation_Others { get; set; }

        [DisplayName("Vision Issue Identified")]
        public int VisionIssueIdentifiedId { get; set; }

        [DisplayName("Type of Vision Issue")]
        public int TypeofVisionIssueId { get; set; }
        [DisplayName("Type of Vision Issue (Other)")]
        public string TypeofVisionIssue_Others { get; set; }

        [DisplayName("Glasses Sold")]
        public int GlassesSold { get; set; }

        [DisplayName("Power of Glass")]
        public int PowerofGlassId { get; set; }

        [DisplayName("Feedbackon Comfort")]
        public int FeedbackonComfort { get; set; }

        [DisplayName("Remarks")]
        public string? Remarks { get; set; }

        [DisplayName("Referral Required")]
        public int FollowupRequiredId { get; set; }

        [DisplayName("Digital Consent")]
        public int DigitalConsentId { get; set; }

        [DisplayName("Location")]
        public int Location{ get; set; }

        [DisplayName("Screening Cost")]
        [Range(0, 99, ErrorMessage = "Screening Cost must be a two-digit number")]
        public int ScreeningCost { get; set; }

		[DisplayName("Glasses Cost")]
        [Range(0, 999, ErrorMessage = "Glasses Cost must be a three-digit number (0-999)")]
        public int GlassesCost { get; set; }

        [DisplayName("Remarks/Action Taken")]
        public string? RemarksActionTaken{ get; set; }
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