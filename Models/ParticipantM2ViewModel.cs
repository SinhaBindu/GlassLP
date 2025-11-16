using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace GlassLP.Models
{
    public class ParticipantM2ViewModel
    {
        public int ParticipantId_pk { get; set; } = 0;

        [DisplayName("Type of Participant")]
        public int TypeofParticipantId{ get; set; }
		
		[DisplayName("Capm Code")]
		public int CampId_fk { get; set; }

		[DisplayName("Participant Name")]
        public string? ParticipantName { get; set; }

        [DisplayName("Mobile No")]
        public string? MobileNo { get; set; }

        [DisplayName("Age")]
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

        [DisplayName("Glasses Sold")]
        public int GlassesSold { get; set; }

        [DisplayName("Power of Glass")]
        public int PowerofGlassId { get; set; }

        [DisplayName("Feedbackon Comfort")]
        public int FeedbackonComfort { get; set; }

        [DisplayName("Remarks")]
        public string? Remarks { get; set; }

        [DisplayName("Followup Required")]
        public int FollowupRequiredId { get; set; }

        [DisplayName("Digital Consent")]
        public int DigitalConsentId { get; set; }

        [DisplayName("Location")]
        public int Location{ get; set; }

        [DisplayName("Screening Cost")]
        public int ScreeningCost { get; set; }

		[DisplayName("Glasses Cost")]
        public int GlassesCost { get; set; }

        [DisplayName("Remarks/Action Taken")]
        public string? RemarksActionTaken{ get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}