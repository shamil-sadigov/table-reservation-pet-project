using System;
using BuildingBlocks.Domain.BusinessRule;
using Xunit;
using Xunit.Abstractions;

namespace Reservation.Domain.Tests
{
    public class WorkingHoursTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WorkingHoursTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(10,00, 55,00, "finishTime should not be greater that 23:59:59")]
        [InlineData(03,00, 22,00, "startTime should be in range 06:00:00-23:59:59")]
        [InlineData(18,00, 10,00, "startTime should not be greater than finishTime")]
        public void Cannot_create_workingHours_when_startTime_or_finishTime_are_invalid(
            int startHour,
            int startMinutes,
            int finishHour,
            int finishMinutes,
            string expectedErrorMessage)
        {
            var startTime = new TimeSpan(startHour, startMinutes, seconds: 00);
            var finishTime = new TimeSpan(finishHour, finishMinutes, seconds: 00);

            _testOutputHelper.WriteLine(finishTime.Hours.ToString());
            
            Result<WorkingHours> result = WorkingHours.Create(startTime, finishTime);
            
            result.ShouldFail();
            
            result.Errors!.ShouldContain(expectedErrorMessage);
        }   
        
        
        
        [Theory]
        [InlineData(10,00, 19,00)]
        [InlineData(06,00, 22,30)]
        [InlineData(09,00, 10,00)]
        public void Can_create_workingHours_when_startTime_or_finishTime_are_valid(
            int startHour,
            int startMinutes,
            int finishHour,
            int finishMinutes)
        {
            var startTime = new TimeSpan(startHour, startMinutes, seconds: 00);
            var finishTime = new TimeSpan(finishHour, finishMinutes, seconds: 00);

            _testOutputHelper.WriteLine(finishTime.Hours.ToString());
            
            Result<WorkingHours> result = WorkingHours.Create(startTime, finishTime);
            
            result.ShouldSucceed();
        } 
    }
}