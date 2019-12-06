using System.Collections.Generic;

namespace APIforMyNUnit.Models
{
    /// <summary>
    /// Class for storing successful, failed and ignored tests of a specific assembly
    /// </summary>
    public class TestedAssemblyModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// List of succeeded tests
        /// </summary>
        public List<TestInformationModel> Succeeded { get; set; }

        /// <summary>
        /// List of failed tests
        /// </summary>
        public List<TestInformationModel> Failed { get; set; }

        /// <summary>
        /// List of ignored tests
        /// </summary>
        public List<TestInformationModel> Ignored { get; set; }
    }
}
