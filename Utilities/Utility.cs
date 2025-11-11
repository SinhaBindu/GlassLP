namespace GlassLP.Utilities
{
    public static class Utility
    {
        public static string GenerateCampCode(string district, string block, string panchayat)
        {
            if (string.IsNullOrWhiteSpace(district) ||
                string.IsNullOrWhiteSpace(block) ||
                string.IsNullOrWhiteSpace(panchayat))
            {
                throw new ArgumentException("District, Block, and Panchayat are required.");
            }

            // Take first 3 characters from each, uppercase and trim
            string distCode = district.Trim().ToUpper().Substring(0, Math.Min(3, district.Length));
            string blockCode = block.Trim().ToUpper().Substring(0, Math.Min(3, block.Length));
            string panchCode = panchayat.Trim().ToUpper().Substring(0, Math.Min(3, panchayat.Length));

            // Get today's date in yyyyMMdd format
            string datePart = DateTime.Now.ToString("yyyyMMdd");

            // Combine all parts
            string campCode = $"{distCode}{blockCode}{panchCode}{datePart}";

            return campCode;
        }

    }
}
