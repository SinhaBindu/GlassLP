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
        public int CampId_fk { get; set; }
        public string? ParticipantName { get; set; }
        public string? MobileNo { get; set; }
        public string? Age { get; set; }
        public string? SHGName { get; set; }
        public int OccupationId { get; set; }
        public string Occupation_Others { get; set; }
        public int VisionIssueIdentifiedId { get; set; }
        public int TypeofVisionIssueId { get; set; }
        public int GlassesProvidedId { get; set; }
        public int PowerofGlassId { get; set; }
        public int FeedbackonComfort { get; set; }
        public string Remarks { get; set; }
        public int FollowupRequiredId { get; set; }
        public int DigitalConsentId { get; set; }
        public bool? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
