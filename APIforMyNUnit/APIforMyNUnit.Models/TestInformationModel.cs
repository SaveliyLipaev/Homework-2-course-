namespace APIforMyNUnit.Models
{
    /// <summary>
    /// Class for general test information
    /// </summary>
    public class TestInformationModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Test execution time
        /// </summary>
        public long Time { get; set; }

        /// <summary>
        /// Test ignore message
        /// </summary>
        public string Ignore { get; set; }
    }
}
