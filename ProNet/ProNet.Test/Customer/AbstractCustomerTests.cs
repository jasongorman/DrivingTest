using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace ProNet.Test.Customer
{
    [TestFixture]
    public abstract class AbstractCustomerTests
    {
        private readonly string _filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                            @"\ProNet.xml";

        private IProNet _proNet;

        public IProNet ProNet
        {
            get { return _proNet; }
        }

        [SetUp]
        public void LoadNetwork()
        {
            _proNet = LoadProNet(_filename);
        }

        #region Happy Paths

        [Test]
        public void ListProgrammerSkills()
        {
            Assert.That(_proNet.Skills("Bill"), Is.EqualTo(new[] {"Ruby", "Perl", "PHP"}));
        }

        [Test]
        public void ListProgrammerRecommendations()
        {
            Assert.That(_proNet.Recommendations("Ed"), Is.EqualTo(new[] {"Liz", "Rick", "Bill"}));
        }

        [Test]
        public void GetProgrammerRank()
        {
            Assert.That(_proNet.Rank("Nick"), Is.EqualTo(2.63).Within(0.01));
        }

        [Test]
        public void GetDegreesOfSeparation()
        {
            Assert.That(_proNet.DegreesOfSeparation("Jill", "Rick"), Is.EqualTo(3));
        }

        [Test]
        public void GetTeamStrength()
        {
            const string leader = "Jason";
            Assert.That(_proNet.TeamStrength("Ruby", new[] {leader, "Bill", "Frank"}), Is.EqualTo(0.36).Within(0.01));
        }

        [Test]
        public void FindStrongestTeam()
        {
            const string leader = "Nick";
            Assert.That(_proNet.FindStrongestTeam("Java", 3), Is.EqualTo(new[] {leader, "Dave", "Jason"}));
        }

        #endregion

        #region Edge Cases


        [Test]
        public void TeamSizeMustBeGreaterThanZero()
        {
            CheckException(() => ProNet.FindStrongestTeam("Java", 0), "Team size must be greater than zero");
        }

        [Test]
        public void SkillsThrowsExceptionWhenProgrammerNotFound()
        {
            CheckException(() => ProNet.Skills("blah"), "Programmer blah was not found");
        }

        [Test]
        public void RecommendationsThrowsExceptionWhenProgrammerNotFound()
        {
            CheckException(() => ProNet.Recommendations("blah"), "Programmer blah was not found");
        }

        [Test]
        public void RankThrowsExceptionWhenProgrammerNotFound()
        {
            CheckException(() => ProNet.Rank("blah"), "Programmer blah was not found");
        }

        [Test]
        public void DegreesOfSeparationThrowsExceptionWhenProgrammerNotFound()
        {
            CheckException(() => ProNet.DegreesOfSeparation("blah", "Bill"), "Programmer blah was not found");
        }

        [Test]
        public void TeamStrengthThrowsExceptionWhenProgrammerNotFound()
        {
            CheckException(() => ProNet.TeamStrength("Ruby", new[] { "blah", "Bill" }), "Programmer blah was not found");
        }

        [Test]
        public void AttemptToLoadNonExistentFileThrowsException()
        {
            CheckException(() => LoadProNet(@"C:\nonexistentfile.xml"), @"File C:\nonexistentfile.xml was not found");
        }

        [Test]
        public void AttemptToLoadInvalidDataThrowsException()
        {
            string invalidFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                                            @"\InvalidData.xml";
            CheckException(() => LoadProNet(invalidFile), "File " + invalidFile + " is not a valid ProNet data file");
        }

        #endregion

        protected abstract IProNet LoadProNet(string filename);

        private void CheckException(TestDelegate @delegate, string message)
        {
            Exception exception = Assert.Throws<ArgumentException>(@delegate);
            Assert.That(exception.Message, Is.EqualTo(message));
        }
    }
}
