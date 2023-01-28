using FluentAssertions;
using FsTask.Domain;
using FsTask.Domain.Contract;
using FsTask.Domain.Exception;

namespace FsTask.UnitTests
{
    public class SensorReceivedEventsAggregateTests
    {
        [Fact]
        public void create_everythingOk_successfullyCreated()
        {
            // Context
            var humanId = "1";
            var positionX = 1.5;
            var positionY = 2.5;

            var dto = new StoreSensorDataCommandDto
            {
                Timestamp = DateTime.Now.Ticks,
                Instances = new List<InstanceDto>
                {
                    new()
                    {
                        HumanId = humanId,
                        PositionX = positionX,
                        PositionY = positionY,
                        VelX = 0,
                        VelY = 0
                    }
                }
            };

            // Act
            var sensorReceivedEventAggregate = CreateSensorReceivedEventAggregate(dto);

            // Outcome
            sensorReceivedEventAggregate.At
                .Should().Be(dto.Timestamp);

            sensorReceivedEventAggregate.Id
                .Should().Be(dto.Id);

            sensorReceivedEventAggregate.HumanEnvironmentalStatistics
                .Count()
                .Should().Be(dto.Instances.Count);

            sensorReceivedEventAggregate.HumanEnvironmentalStatistics
                .OfHuman(humanId)
                .Position
                .Should().BeEquivalentTo(Position.New(positionX, positionY));
        }

      

        [Fact]
        public void postionEqualityTest()
        {
            Position.New(1.5, 1.5)
                .Should()
                .BeEquivalentTo(Position.New(1.5, 1.5));

            Position.New(1.5, 1.5)
                .Should()
                .NotBeEquivalentTo(Position.New(1.5, 1));
        }

        [Fact]
        public void humanBeingInDifferentPositionAtTheSameTime_ExceptionWllBeThrown()
        {
            // Context
            var humanId = "1";
            var positionX = 1.5;
            var positionY = 2.5;

            var position2X = 5.2;
            var position2Y = 2.5;

            var dto = new StoreSensorDataCommandDto
            {
                Timestamp = DateTime.Now.Ticks,
                Instances = new List<InstanceDto>
                {
                    new()
                    {
                        HumanId = humanId,
                        PositionX = positionX,
                        PositionY = positionY,
                        VelX = 0,
                        VelY = 0
                    },
                    new()
                    {
                        HumanId = humanId,
                        PositionX = position2X,
                        PositionY = position2Y,
                        VelX = 0,
                        VelY = 0
                    }
                }
            };

            Action action = () => 
                CreateSensorReceivedEventAggregate(dto);

            action.Should().Throw<HumanBeingInDifferentPositionAtTheSameTimeException>();
        }

        [Fact]
        public void velocityEqualityTest()
        {
            Velocity.New(1.5, 1.5)
                .Should()
                .BeEquivalentTo(Velocity.New(1.5, 1.5));

            Velocity.New(1.5, 1.5)
                .Should()
                .NotBeEquivalentTo(Velocity.New(1.5, 1));
        }
        [Fact]
        public void testCalibbratePostionX()
        {
            Assert.Fail("write a test here please!");
        }
        [Fact]
        public void testCalibbratePostionY()
        {
            Assert.Fail("write a test here please!");
        }

        private static SensorReceivedEventsAggregate CreateSensorReceivedEventAggregate(StoreSensorDataCommandDto dto)
        {
            return new SensorReceivedEventsAggregate(
                dto.Id,
                at: dto.Timestamp,
                new StoreSensorDataCommand { Instances = dto.Instances });
        }
    }
}