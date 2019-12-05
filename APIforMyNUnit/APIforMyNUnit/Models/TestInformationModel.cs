using MyNUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIforMyNUnit.Models
{
    public class TestInformationModel
    {
        /// <summary>
        /// Initializes new instance of <see cref="TestInformationModel"/>
        /// </summary>
        public TestInformationModel() { }


        /// <summary>
        /// Initializes new instance of <see cref="TestInformationModel"/>
        /// from <see cref="TestMethodResult"/> object
        /// </summary>
        /// <param name="result">method test result</param>
        public TestInformationModel(TestInformation result)
        {
            Name = result.MethodName;
            ExecutionTime = result.Time;
            IgnoreMessage = result.Ignore;
        }

        /// <summary>
        /// Model id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Test name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Test execution time
        /// </summary>
        public long ExecutionTime { get; set; }

        /// <summary>
        /// Test ignore message
        /// </summary>
        public string IgnoreMessage { get; set; }
    }
}
