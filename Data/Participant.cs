using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GlassLP.Data
{
	[Table("Participants")]
	public class Participant
	{
		[Key]
		public int ParticipantId { get; set; }
		public int TypeofModule { get; set; }
		public int NoGlassesReceived { get; set; }
		public int fk_CampId { get; set; }
		public int DistricId { get; set; }
		public int BlockId { get; set; }

		public int CLFId { get; set; }

		[StringLength(500)]
		public string VOName { get; set; }
		public int PanchayatId { get; set; }
		public DateTime CampDate { get; set; }

		[StringLength(500)]
		public string? CampLocationType { get; set; }

		[StringLength(500)]
		public string? CRPName { get; set; }
		public int ParticipantMobilized { get; set; }
		public int TotalScreened { get; set; }
		public int TotalGlassesDistributed { get; set; }
		public int DistributionType { get; set; }

		[StringLength(500)]
		public string CampPhoto { get; set; }


		[StringLength(500)]
		public string ParticipantName { get; set; }

		[StringLength(15)]
		public string ParticipantMob { get; set; }
		public int Age { get; set; }

		[StringLength(500)]
		public string SHGName { get; set; }
		public int OccupationId { get; set; }

		[StringLength(500)]
		[ValidateNever]
		public string? OccupationOther { get; set; }
		public bool VisionIssueIdentified { get; set; }
		public int TypeofVisionIssueId { get; set; }
		public bool GlassesProvided { get; set; }

		[StringLength(10)]
		public string PowerofGlasses { get; set; }
		public int FeedbackonComfortId { get; set; }

		[StringLength(500)]
		public string Remarks { get; set; }

		public bool IsFollowupRequired { get; set; }
		public bool DigitalConsent { get; set; }

		[StringLength(130)]
		[ValidateNever]
		public string? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }

		[StringLength(130)]
		[ValidateNever]
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }
	}
}
