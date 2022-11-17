using CodeLouisvilleUnitTestProject;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit.Abstractions;

namespace CodeLouisvilleUnitTestProjectTests
{
    public class VehicleTests
    {

        //Verify the parameterless constructor successfully creates a new
        //object of type Vehicle, and instantiates all public properties
        //to their default values.
        [Fact]
        public void VehicleParameterlessConstructorTest()
        {
            //act
            Vehicle v = new Vehicle();

            //assert
            v.Make.Should().Be("");
            v.Model.Should().Be("");
            v.MilesPerGallon.Should().Be(0);
            v.GasTankCapacity.Should().Be(0);
            v.NumberOfTires.Should().Be(0);

        }

        //Verify the parameterized constructor successfully creates a new
        //object of type Vehicle, and instantiates all public properties
        //to the provided values.
        [Fact]
        public void VehicleConstructorTest()
        {
            //arrange
            //throw new NotImplementedException();

            //act
            Vehicle v = new Vehicle(4, 100, "Skoda", "Rapid", 15);

            //assert
            v.Make.Should().Be("Skoda");
            v.Model.Should().Be("Rapid");
            v.MilesPerGallon.Should().Be(15);
            v.GasTankCapacity.Should().Be(100);
            v.NumberOfTires.Should().Be(4);

        }

        //Verify that the parameterless AddGas method fills the gas tank
        //to 100% of its capacity
        [Fact]
        public void AddGasParameterlessFillsGasToMax()
        {
            //arrange
            //throw new NotImplementedException();
            //act
            Vehicle v = new Vehicle(4, 100, "Skoda", "Rapid", 15);
            //assert
            v.AddGas().Should().Be(100);

        }

        //Verify that the AddGas method with a parameter adds the
        //supplied amount of gas to the gas tank.
        [Fact]
        public void AddGasWithParameterAddsSuppliedAmountOfGas()
        {
            //arrange
            //throw new NotImplementedException();
            //act
            Vehicle v = new Vehicle(4, 100, "Skoda", "Rapid", 15);
            //assert
            v.AddGas(50).Should().Be(50);
            Action action = () => v.AddGas(60);
            action.Should().Throw<GasOverfillException>();

        }

        //Verify that the AddGas method with a parameter will throw
        //a GasOverfillException if too much gas is added to the tank.
        [Fact]
        public void AddingTooMuchGasThrowsGasOverflowException()
        {
            //arrange
            //throw new NotImplementedException();
            //act
            Vehicle v = new Vehicle(4, 100, "Skoda", "Rapid", 15);
            Action action = () => v.AddGas(120);
            //assert
            action.Should().Throw<GasOverfillException>().WithMessage("Unable to add 120 gallons to tank because it would exceed the capacity of 100 gallons");

        }

        //Using a Theory (or data-driven test), verify that the GasLevel
        //property returns the correct percentage when the gas level is
        //at 0%, 25%, 50%, 75%, and 100%.
        [Theory]
        [InlineData(0, "0%")]
        [InlineData(25.0f, "25%")]
        [InlineData(50.0f, "50%")]
        [InlineData(75.0f, "75%")]
        [InlineData(100.0f, "100%")]
        public void GasLevelPercentageIsCorrectForAmountOfGas(float gasToFill, String gasLevel)
        {
            //arrange
            //throw new NotImplementedException();
            //act
            Vehicle v = new Vehicle(4, 100, "Skoda", "Rapid", 15);
            v.AddGas(gasToFill);
            v.GasLevel.Should().Be(gasLevel);
            //assert

        }

        /*
         * Using a Theory (or data-driven test), or a combination of several 
         * individual Fact tests, test the following functionality of the 
         * Drive method:
         *      a. Attempting to drive a car without gas returns the status 
         *      string “Cannot drive, out of gas.”.
         *      b. Attempting to drive a car with a flat tire returns 
         *      the status string “Cannot drive due to flat tire.”.
         *      c. Drive the car 10 miles. Verify that the correct amount 
         *      of gas was used, that the correct distance was traveled, 
         *      that GasLevel is correct, that MilesRemaining is correct, 
         *      and that the total mileage on the vehicle is correct.
         *      d. Drive the car 100 miles. Verify that the correct amount 
         *      of gas was used, that the correct distance was traveled,
         *      that GasLevel is correct, that MilesRemaining is correct, 
         *      and that the total mileage on the vehicle is correct.
         *      e. Drive the car until it runs out of gas. Verify that the 
         *      correct amount of gas was used, that the correct distance 
         *      was traveled, that GasLevel is correct, that MilesRemaining
         *      is correct, and that the total mileage on the vehicle is 
         *      correct. Verify that the status reports the car is out of gas.
        */
        [Theory]
        [InlineData("MysteryParamValue")]
        public void DriveNegativeTests(params object[] yourParamsHere)
        {
            Vehicle v = new(4, 100, "Skoda", "Rapid", 2);
            v.Drive(10).Should().Be("Cannot drive, out of gas.");
            v.AddGas();
            v._hasFlatTire = true;
            v.Drive(10).Should().Be("Cannot drive due to flat tire.");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(100)]
        [InlineData(500)]
        public void DrivePositiveTests(double miles)
        {
            double capacity = 100;
            double milesPerGallon = 2;
            double totalMiles = capacity * milesPerGallon;
            Vehicle v = new(4, capacity, "Skoda", "Rapid", milesPerGallon);
            double gasRemaning = v.AddGas();
            string msg = v.Drive(miles);
            
            if (totalMiles > miles)
            {
                gasRemaning -= (miles / milesPerGallon);
                String gasLevel = (gasRemaning / capacity * 100) + "%";
                double milesRemaining = gasRemaning * milesPerGallon;
                double gasUsed = capacity - gasRemaning;
                msg.Should().Contain("Drove " + miles + " miles using " + gasUsed + " gallons of gas.");
                v.GasLevel.Should().Be(gasLevel);
                v.MilesRemaining.Should().Be(milesRemaining);
                v.Mileage.Should().Be(miles);
            } 
            else
            {
                msg.Should().Contain("Drove " + totalMiles + " miles, then ran out of gas.");
                v.GasLevel.Should().Be("0%");
                v.MilesRemaining.Should().Be(0);
                v.Mileage.Should().Be(totalMiles);
            }
        }

        //Verify that attempting to change a flat tire using
        //ChangeTireAsync will throw a NoTireToChangeException
        //if there is no flat tire.
        [Fact]
        public async Task ChangeTireWithoutFlatTest()
        {
            Vehicle v = new(4, 100, "Skoda", "Rapid", 2);
            v._hasFlatTire = false;
            Func<Task> f = async () => { await v.ChangeTireAsync(); };
            await f.Should().ThrowAsync<NoTireToChangeException>();

        }

        //Verify that ChangeTireAsync can successfully
        //be used to change a flat tire
        [Fact]
        public async Task ChangeTireSuccessfulTest()
        {
            Vehicle v = new(4, 100, "Skoda", "Rapid", 2);
            v._hasFlatTire = true;
            await v.ChangeTireAsync();
            v._hasFlatTire.Should().BeFalse();
        }

        //BONUS: Write a unit test that verifies that a flat
        //tire will occur after a certain number of miles.
        [Theory]
        [InlineData(100, 186789012, true)]
        [InlineData(100, 10000000, false)]
        public void GetFlatTireAfterCertainNumberOfMilesTest(double miles, int range, Boolean result)
        {
            //throw new NotImplementedException();
            double capacity = 100;
            double milesPerGallon = 2;
            Vehicle v = new(4, capacity, "Skoda", "Rapid", milesPerGallon);

            v.GotFlatTire(miles, range).Should().Be(result);

        }
    }
}