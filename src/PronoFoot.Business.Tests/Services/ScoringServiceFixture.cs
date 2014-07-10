using Microsoft.VisualStudio.TestTools.UnitTesting;
using PronoFoot.Business.Services;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Tests.Services
{
    [TestClass]
    public class ScoringServiceFixture
    {
        [TestMethod]
        public void WhenWinAndExactForecast_ThenGet3Points()
        {
            Assert.AreEqual(Rating.Exact, new ScoringService().GetRating(2,1,2,1));
        }

        [TestMethod]
        public void WhenWinAndGood1N2With1GoalDifference_ThenGet2Points()
        {
            Assert.AreEqual(Rating.Close, new ScoringService().GetRating(2, 1, 2, 0));
        }
        
        [TestMethod]
        public void WhenWinAndGood1N2With1GoalDifference_ThenGet2Points2()
        {
            Assert.AreEqual(Rating.Close, new ScoringService().GetRating(2, 1, 3, 1));
        }

        [TestMethod]
        public void WhenWinAndGood1N2WithExactGoalDifferenceButShiftedBy1_ThenGet1PointAndHalf()
        {
            Assert.AreEqual(Rating.CorrectDifference, new ScoringService().GetRating(2, 1, 1, 0));
        }

        [TestMethod]
        public void WhenWinAndGood1N2WithExactGoalDifferenceButShiftedBy1_ThenGet1PointAndHalf2()
        {
            Assert.AreEqual(Rating.CorrectDifference, new ScoringService().GetRating(2, 1, 3, 2));
        }

        [TestMethod]
        public void WhenWinAndGood1N2_ThenGet1Point()
        {
            Assert.AreEqual(Rating.CorrectOutcome, new ScoringService().GetRating(2, 1, 4, 1));
        }

        [TestMethod]
        public void WhenWinAndGood1N2_ThenGet1Point2()
        {
            Assert.AreEqual(Rating.CorrectOutcome, new ScoringService().GetRating(2, 1, 4, 3));
        }

        [TestMethod]
        public void WhenWinAndForecastDraw_ThenGet0Point()
        {
            Assert.AreEqual(Rating.Wrong, new ScoringService().GetRating(2, 1, 1, 1));
        }

        [TestMethod]
        public void WhenWinAndForecastLoss_ThenGet0Point()
        {
            Assert.AreEqual(Rating.Wrong, new ScoringService().GetRating(2, 1, 1, 2));
        }

        [TestMethod]
        public void WhenDrawAndExactForecast_ThenGet3Points()
        {
            Assert.AreEqual(Rating.Exact, new ScoringService().GetRating(1, 1, 1, 1));
        }

        [TestMethod]
        public void WhenDrawAndGood1N2With1GoalDifference_ThenGet2Points()
        {
            Assert.AreEqual(Rating.Close, new ScoringService().GetRating(1, 1, 0, 0));
        }

        [TestMethod]
        public void WhenDrawAndGood1N2With1GoalDifference_ThenGet2Points2()
        {
            Assert.AreEqual(Rating.Close, new ScoringService().GetRating(1, 1, 2, 2));
        }

        [TestMethod]
        public void WhenDrawAndGood1N2WithMoreThan1GoalDifference_ThenGet1Point()
        {
            Assert.AreEqual(Rating.CorrectOutcome, new ScoringService().GetRating(1, 1, 3, 3));
        }

        [TestMethod]
        public void WhenDrawAndForecastWin_ThenGet0Point()
        {
            Assert.AreEqual(Rating.Wrong, new ScoringService().GetRating(1, 1, 1, 0));
        }

        [TestMethod]
        public void WhenDrawAndForecastLoss_ThenGet0Point()
        {
            Assert.AreEqual(Rating.Wrong, new ScoringService().GetRating(1, 1, 1, 2));
        }
    }
}
