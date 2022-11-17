using CodeLouisvilleUnitTestProject;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Abstractions;

namespace CodeLouisvilleUnitTestProjectTests
{
    public class CarTest
    {
        Car carWithoutParams;
        Car carWithParams;
        public CarTest()
        {
            carWithoutParams = new Car();
            carWithParams = new Car(20, "Honda", "Civic", 10);
        }
        [Fact]
        public void CarConstructorTest()
        {
            carWithoutParams.NumberOfTires.Should().Be(4);
            carWithParams.NumberOfTires.Should().Be(4);
            carWithParams.GetType().Should().Be(typeof(Car));
        }

        [Theory]
        [InlineData("Honda", "Civic", true)]
        [InlineData("Honda", "Camry", false)]
        public async Task IsValidModelForMakeAsyncTest(string make, string model, bool result)
        {
            Car car = new Car(20, make, model, 10);
            bool res = await car.IsValidModelForMakeAsync();
            res.Should().Be(result);
        }

        [Fact]
        public async Task WasModelMadeInYearAsyncNegativeTest()
        {
            Car car= new Car(20, "Honda", "Civic", 10);

            Func<Task> f = async () => { await car.WasModelMadeInYearAsync(1980); };
            await f.Should().ThrowAsync<ArgumentException>();
        }

        [Theory]
        [InlineData("Honda", "Civic", 2015, true)]
        [InlineData("Honda", "Camry", 2015, false)]
        public async Task WasModelMadeInYearAsyncPositiveTest(string make, string model, int year, bool result)
        {
            Car car = new Car(20, make, model, 10);
            bool res = await car.WasModelMadeInYearAsync(year);
            res.Should().Be(result);
        }
        [Fact]
        public void AddPassengersTest()
        {
            Car car = new Car(20, "Honda", "Civic", 10);
            car.AddPassengers(4);
            car.MilesPerGallon.Should().Be(9.2);
            car.RemovePassengers(2);
            car.MilesPerGallon.Should().Be(9.6);
        }

        [Theory]
        [InlineData(5, 21, 3)]
        [InlineData(5, 21, 5)]
        [InlineData(5, 21, 25)]
        public void RemovePassengersTest(int add, double mpg, int remove)
        {
            Car car = new Car(20, "Honda", "Civic", mpg);
            car.AddPassengers(add);
            double newMpg = mpg - (double)(0.2 * add); ;
            int passengersRemaining = add;
            car.RemovePassengers(remove);
            newMpg = newMpg + (double)(0.2 * remove);
            passengersRemaining = passengersRemaining - remove;
            if (passengersRemaining >= 0)
            {
                car.NumberOfPassengers.Should().Be(passengersRemaining);
                car.MilesPerGallon.Should().Be(newMpg);
            } 
            else
            {
                car.NumberOfPassengers.Should().Be(0);
                car.MilesPerGallon.Should().Be(mpg);
            }
            
        }
    }
}