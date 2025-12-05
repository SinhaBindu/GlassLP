namespace GlassLP.Data
{
	public class Service
	{
		public class GlobalDataService
		{
			public string UserId { get; set; }
			public string UserName { get; set; }
			public string Name { get; set; }
			public string PhoneNumber { get; set; }
			public string Email { get; set; }
			public string Role { get; set; }
			public string RoleId { get; set; }
			public string DistrictIds { get; set; }
			public string BlockId { get; set; }
			public string CLFId { get; set; }
			public string DistrictName { get; set; }
			public string BlockName { get; set; }
			public string CLFName { get; set; }
			public string LoginTime { get; set; }
		}
		public interface IGlobalDataService
		{
			GlobalDataService UserData { get; set; }
		}
		public class GlobalDataServiceImpl : IGlobalDataService
		{
			public GlobalDataService UserData { get; set; } = new GlobalDataService();
		}
	}
}
